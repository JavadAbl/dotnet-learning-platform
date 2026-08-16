using API.ExceptionHandlers;
using Shared;
using Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Users;

var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddContractsModule();
builder.Services.AddUsersModule(builder.Configuration, mvcBuilder);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

var app = builder.Build();
app.UseExceptionHandler();
app.UseRouting();
app.MapControllers();

await app.RunAsync();
