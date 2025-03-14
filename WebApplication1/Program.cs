var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

var app = builder.Build();

app.UseWebSockets(); 
app.MapControllers();
app.UseHttpsRedirection();
app.Run();