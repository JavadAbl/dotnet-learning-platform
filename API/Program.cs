using Contracts;
using Users;

var builder = WebApplication.CreateBuilder(args);
var mvcBuilder = builder.Services.AddControllers();

builder.Services.AddContractsModule();
builder.Services.AddUsersModule(builder.Configuration, mvcBuilder);

var app = builder.Build();

app.UseRouting();
app.MapControllers();

await app.RunAsync();
