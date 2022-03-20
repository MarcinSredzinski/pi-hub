using SensorDataStore.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureApi();

Log.Logger.Information("We are up and running!");
app.Run();