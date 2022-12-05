using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// OutputCathing!!
builder.Services.AddOutputCache(ops =>
{
    ops.AddBasePolicy(builder =>
    {
        builder.Expire(TimeSpan.FromSeconds(7));
    });

    // Özel Policy
    ops.AddPolicy("Custom", policy =>
    {
        policy.Expire(TimeSpan.FromSeconds(5));
    });
});





var app = builder.Build();

// OutputCathing!!
app.UseOutputCache();
app.MapGet("/", [OutputCache()] () =>
{
    return Results.Ok(DateTime.UtcNow);
}).CacheOutput();
// Özelleþtirme
app.MapGet("/", [OutputCache()] () =>
{
    return Results.Ok(DateTime.UtcNow);
}).CacheOutput("Custom");




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
