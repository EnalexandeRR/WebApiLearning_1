using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace MyWebApp;

public interface INewsRepository
{
    Task SaveNewsToDb(IEnumerable<NewsItem> newsItem);
    void GetNews();
}

public class NewsRepository: INewsRepository
{
    private readonly string _dbConnectionString;
    private readonly string _tableName;
    public NewsRepository(IConfiguration config)
    {
        _dbConnectionString = config.GetConnectionString("DefaultConnection");
        _tableName = config.GetConnectionString("DefaultTableNam");
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

    public void GetNews()
    {
        Console.WriteLine("(TEST) got the news from DATABASE!");
    }
}