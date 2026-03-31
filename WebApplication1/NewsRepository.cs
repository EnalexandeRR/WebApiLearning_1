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
    public NewsRepository(string dbConnectionString, string tableName)
    {
        _dbConnectionString = dbConnectionString;
        _tableName = tableName;
    }
    
    public async Task SaveNewsToDb(IEnumerable<NewsItem> newsItems)
    {
        using (IDbConnection db = new SqliteConnection(_dbConnectionString))
        {
            db.Open();
            using (var transaction = db.BeginTransaction())
            {
                var sqlQuery = $"INSERT INTO {_tableName} (id, title, releaseTime, viewCount) VALUES(@Id, @Title, @ReleaseTime, @ViewCount)";
                var isAdded = await db.ExecuteAsync(sqlQuery, newsItems, transaction);
                Console.WriteLine($"db isAdded? = {isAdded}");
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