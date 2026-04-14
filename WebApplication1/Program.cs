using System.Text;
using Dapper;
using Quartz;
using WebApplication1.Clients;
using WebApplication1.Configuration;
using WebApplication1.Helpers;
using WebApplication1.Middleware;
using WebApplication1.Repositories;
using WebApplication1.Services;

Console.OutputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.Configure<NewsClientOptions>(builder.Configuration.GetSection(nameof(NewsClientOptions)));
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(nameof(DatabaseOptions)));
builder.Services.Configure<NewsJobOptions>(builder.Configuration.GetSection(nameof(NewsJobOptions)));

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddHttpClient<INewsClient, NewsClient>();

builder.Services.AddQuartz(options =>
{
    var newsJobOptions = builder.Configuration.GetSection(nameof(NewsJobOptions)).Get<NewsJobOptions>();
    var jobKey = JobKey.Create(newsJobOptions.JobName);
    options.AddJob<BackgroundGetNewsJob>(jobKey) 
        .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
                .StartAt(DateTimeOffset.Now.AddSeconds(newsJobOptions.StartDelayInSeconds))
                .WithSimpleSchedule(s => s
                    .WithIntervalInSeconds(newsJobOptions.IntervalInSeconds).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
SqlMapper.AddTypeHandler(new SqliteDateTimeOffsetHandler());

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();
app.Run();