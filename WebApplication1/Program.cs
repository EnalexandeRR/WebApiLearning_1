using MyWebApp;
using WebApplication1.Interfaces;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpClient<INewsClient, NewsClient>(client =>
{
    client.BaseAddress = new Uri("https://khabar.kz/");
    client.Timeout = TimeSpan.FromSeconds(15);
} );
builder.Services.AddHostedService<NewsParseWorker>();

var app = builder.Build();

app.MapControllers();
app.Run();