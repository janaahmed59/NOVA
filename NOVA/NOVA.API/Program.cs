
using NOVA.API.Extension;
using NOVA.API.Extensions;
using NOVA.API.Middlewares;
using NOVA.Application;
using NOVAInfrastructure;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerService();

builder.Services.AddApplicationServices();
builder.Services.AddAuthenticationService(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.ConfigureNovaSwaggerUI();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
