using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using WebApplication1.Models;

namespace WebApplication1.Repositories;


public class NewsRepository: INewsRepository
{
    private readonly string _dbConnectionString;
    private readonly string _tableName;
    public NewsRepository(IConfiguration config)
    {
        _dbConnectionString = config.GetConnectionString("DefaultConnection");
        _tableName = config.GetConnectionString("DefaultTableName");
    }
    
    public async Task<bool> SaveNewsToDbAsync(IEnumerable<NewsItem> newsItems)
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                var sqlQuery = $"INSERT INTO {_tableName} (title, releaseTime, viewCount, isAutoAdded) VALUES(@Title, @ReleaseTime, @ViewCount, @IsAutoAdded)";
                await db.ExecuteAsync(sqlQuery, newsItems, transaction);
                transaction.Commit();
            }
        }
        Console.WriteLine("(TEST) news item created in DATABASE!");
        //TODO: only for tests now!
        return true;
    }

    public async Task<IEnumerable<NewsItem>> GetNewsAsync(GetNewsRequest request)
    {
        
        Console.WriteLine($"Try to get news from {request.from} to {request.to}");
        using (var db = new SqliteConnection(_dbConnectionString))
        {
           return await db.QueryAsync<NewsItem>("SELECT * FROM news WHERE releaseTime >= @from and releaseTime <= @to",
               new { from = request.from.ToUniversalTime(),to = request.to.ToUniversalTime()} );
        }
    }

    public async Task<bool> AddNewsToDbAsync(AddNewsRequest request)
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            var sqlQuery = $"INSERT INTO {_tableName} (title, releaseTime, viewCount, isAutoAdded) VALUES(@Title, @ReleaseTime, @ViewCount, @IsAutoAdded)";
            DateTimeOffset dt = DateTimeOffset.Now;
            DateTimeOffset currentUniversalTime = dt.AddTicks(-(dt.Ticks % TimeSpan.TicksPerSecond)).ToUniversalTime();
            
            var lines = await db.ExecuteAsync(sqlQuery, new
            {
                request.Title,
                ReleaseTime = currentUniversalTime,
                request.ViewCount,
                IsAutoAdded = false
            });
            return lines > 0;
        }
    }

    public async Task<bool> DeleteNewsByIdAsync(DeleteByIdRequest request)
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            var sqlQuery = $"DELETE FROM {_tableName} WHERE id = @Id";
            var deletedLines = await db.ExecuteAsync(sqlQuery, request);
            return deletedLines > 0;
        }
    }

    public async Task<DateTimeOffset?> GetLastAddedTime()
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            var sqlQuery = $"SELECT MAX(releaseTime) FROM {_tableName} where isAutoAdded = 1";
            var result = await db.QueryFirstOrDefaultAsync<DateTimeOffset?>(sqlQuery);
            return result;
        }
    }
}