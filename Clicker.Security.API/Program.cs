using Carter;
using Clicker.Security.API.Extensions;
using Clicker.Security.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlConnection(builder);
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureCors();



var app = builder.Build();
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAny");
app.UseAuthentication();
app.UseAuthorization();
app.MapCarter();
app.Run();

