using System.Text;
using Dapper;
using Quartz;
using WebApplication1.Clients;
using WebApplication1.Configuration;
using WebApplication1.Helpers;
using WebApplication1.Repositories;
using WebApplication1.Services;

Console.OutputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<NewsClientSettings>(builder.Configuration.GetSection(nameof(NewsClientSettings)));
builder.Services.AddControllers();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddHttpClient<INewsClient, NewsClient>((sp, client) =>
{
    var settings = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<NewsClientSettings>>().Value;
    
    client.BaseAddress = new Uri(settings.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
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
SqlMapper.AddTypeHandler(new SqliteDateTimeOffsetHandler());

var app = builder.Build();

app.MapControllers();
app.Run();