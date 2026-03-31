using MyWebApp;
using MyWebApp.Interfaces;
using System.Text;
using Quartz;
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<INewsPageParser, NewsPageParser>();
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
                    .WithIntervalInSeconds(10).RepeatForever()));
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var app = builder.Build();

app.MapControllers();
app.Run();