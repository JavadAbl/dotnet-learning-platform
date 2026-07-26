using API.ExceptionHandlers;
using Contracts;
using Users;

var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddControllers();

builder.Services.AddContractsModule();
builder.Services.AddUsersModule(builder.Configuration, mvcBuilder);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseRouting();
app.MapControllers();

await app.RunAsync();
