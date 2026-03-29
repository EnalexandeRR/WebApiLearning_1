using MyWebApp;
using MyWebApp.Interfaces;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<INewsPageParser, NewsPageParser>();
builder.Services.AddHttpClient<INewsClient, NewsClient>(client =>
{
    client.BaseAddress = new Uri("https://khabar.kz/");
    client.Timeout = TimeSpan.FromSeconds(15);
} );
builder.Services.AddHostedService<NewsParseWorker>();


var app = builder.Build();

app.MapControllers();
app.Run();