using MyWebApp;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHostedService<NewsParseWorker>();

var app = builder.Build();

app.MapControllers();
app.Run();