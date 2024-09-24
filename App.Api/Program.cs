using App.Api;
using App.Infrastructure;
using App.Application;
using App.Api.Middlwares;
using App.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services
    .AddPresentation()
    .AddInfrastructure(builder.Configuration)
    .AddApplication();
 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}

await app.ApplyMigrationAsync();
//app.UseExceptionHandler("/error");

app.UseMiddleware<GlobalExeptionHandler>();
app.MapControllers(); //Middlware Pipline

app.Run();
