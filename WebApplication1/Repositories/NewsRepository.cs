using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace MyWebApp;

public interface INewsRepository
{
    Task SaveNewsToDb(IEnumerable<NewsItem> newsItem);
    Task<IEnumerable<NewsItem>> GetNews(DateTimeOffset from,DateTimeOffset to);
}

public class NewsRepository: INewsRepository
{
    private readonly string _dbConnectionString;
    private readonly string _tableName;
    public NewsRepository(IConfiguration config)
    {
        _dbConnectionString = config.GetConnectionString("DefaultConnection");
        _tableName = config.GetConnectionString("DefaultTableName");
    }
    
    public async Task SaveNewsToDb(IEnumerable<NewsItem> newsItems)
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                var sqlQuery = $"INSERT INTO {_tableName} (id, title, releaseTime, viewCount) VALUES(@Id, @Title, @ReleaseTime, @ViewCount)";
                await db.ExecuteAsync(sqlQuery, newsItems, transaction);
                transaction.Commit();
            }
        }
        Console.WriteLine("(TEST) news item created in DATABASE!");
    }

    public async Task<IEnumerable<NewsItem>> GetNews(DateTimeOffset from,DateTimeOffset to)
    {
        
        Console.WriteLine($"Tru to get news from {from} to {to}");
        using (var db = new SqliteConnection(_dbConnectionString))
        {
           return await db.QueryAsync<NewsItem>("SELECT * FROM news WHERE releaseTime >= @from and releaseTime <= @to",
               new { from = from.ToUniversalTime(),to = to.ToUniversalTime()} );
        }
    }
}