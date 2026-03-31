using MyWebApp;
using MyWebApp.Interfaces;
using System.Text;
using Quartz;
Console.OutputEncoding = Encoding.UTF8;
//TODO: absolute path for now! fix later!
string dbConnectionString = @"Data Source=C:\Users\ayenu\RiderProjects\WebApiLearning_1\db\test.db;";
string tableName = "test";
//
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<INewsPageParser, NewsPageParser>();
builder.Services.AddScoped<INewsRepository, NewsRepository>(provider => new NewsRepository(dbConnectionString, tableName));
builder.Services.AddHttpClient<INewsClient, NewsClient>(client =>
{
    client.BaseAddress = new Uri("https://khabar.kz/");
    client.Timeout = TimeSpan.FromSeconds(15);
} );

builder.Services.AddQuartz(options =>
{
    var jobKey = JobKey.Create("GetNewsJob");
    options.AddJob<BackgroundGetNewsJob>(jobKey) 
        .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
                .StartAt(DateTimeOffset.Now.AddSeconds(5))
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(15).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

app.MapControllers();
app.Run();