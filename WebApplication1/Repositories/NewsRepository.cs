using System.Data;
using Dapper;
using WebApplication1.Models;

namespace WebApplication1.Repositories;


public class NewsRepository: INewsRepository
{
    private readonly IDbConnection _db;
    public NewsRepository(IDbConnection db)
    {
        _db = db;
    }
    
    public async Task<bool> SaveNewsToDbAsync(IEnumerable<NewsItem> newsItems)
    {
        _db.Open();
        using (var transaction = _db.BeginTransaction())
        {
            var sqlQuery = $"INSERT INTO news (title, release_time, view_count, is_auto_added) VALUES(@Title, @ReleaseTime, @ViewCount, @IsAutoAdded)";
            await _db.ExecuteAsync(sqlQuery, newsItems, transaction);
            transaction.Commit();
        }
        Console.WriteLine("(TEST) news item created in DATABASE!");
        //TODO: only for tests now!
        return true;
    }

    public async Task<IEnumerable<GetNewsResponseDTO>> GetNewsAsync(GetNewsRequest request)
    {
        
        Console.WriteLine($"Try to get news from {request.From} to {request.To}");
           return await _db.QueryAsync<GetNewsResponseDTO>($"SELECT * FROM news WHERE (@From IS NULL OR release_time >= @From) AND (@To IS NULL OR release_time <= @To)",
               new { From = request.From?.ToUniversalTime(),To = request.To?.ToUniversalTime()} );
    }

    public async Task<bool> AddNewsToDbAsync(AddNewsRequest request)
    {
        var sqlQuery = $"INSERT INTO news (title, release_time, view_count, is_auto_added) VALUES(@Title, @ReleaseTime, @ViewCount, @IsAutoAdded)";
        DateTimeOffset dt = DateTimeOffset.Now;
        DateTimeOffset currentUniversalTime = dt.AddTicks(-(dt.Ticks % TimeSpan.TicksPerSecond)).ToUniversalTime();
            
        var lines = await _db.ExecuteAsync(sqlQuery, new
        {
            request.Title,
            ReleaseTime = currentUniversalTime,
            request.ViewCount,
            IsAutoAdded = false
        });
        return lines > 0;
    }

    public async Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request)
    {
        var sqlQuery = $"DELETE FROM news WHERE id = @Id";
        var deletedLines = await _db.ExecuteAsync(sqlQuery, request);
        return deletedLines > 0;
    }

    public async Task<int> DeleteNewsByPeriodAsync(DeleteByPeriodRequest request)
    {
        var sqlQuery = $"DELETE FROM news WHERE (@From IS NULL OR release_time >= @From) AND (@To IS NULL OR release_time <= @To)";
        return await _db.ExecuteAsync(sqlQuery, new
        {
            From = request.From?.ToUniversalTime(),
            To = request.To?.ToUniversalTime()
        });
    }

    public async Task<DateTimeOffset?> GetLastAddedTime()
    {
        var sqlQuery = $"SELECT MAX(release_time) FROM news where is_auto_added = 1";
        var fieldResult = await _db.QueryFirstOrDefaultAsync<DateTime?>(sqlQuery);
        DateTimeOffset? result = fieldResult.HasValue ? new DateTimeOffset(fieldResult.Value, TimeSpan.Zero) : null;
        Console.WriteLine($"Last added time is {result}");
        return result;
    }
}