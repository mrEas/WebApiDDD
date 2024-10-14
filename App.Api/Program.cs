using App.Api;
using App.Infrastructure;
using App.Application;
using App.Api.Middlwares;
using App.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();
 
var app = builder.Build();

 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.ApplyMigrationAsync(); 
    //app.UseDeveloperExceptionPage(); 
}

//app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<GlobalExeptionHandler>();
app.MapControllers();  

app.Run();
