using System.Text;
using Quartz;
using MyWebApp;
using MyWebApp.Configuration;

Console.OutputEncoding = Encoding.UTF8;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<NewsClientSettings>(builder.Configuration.GetSection("NewsClientSettings"));
builder.Services.AddControllers();
builder.Services.AddTransient<INewsPageParser, NewsPageParser>();
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

var app = builder.Build();

app.MapControllers();
app.Run();