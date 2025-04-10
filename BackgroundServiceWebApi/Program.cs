using BackgroundServiceWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<BackgroundTaskQueue>(provider => new BackgroundTaskQueue(500));
builder.Services.AddHostedService<BackgroundTaskService>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("addToTask", 
    async ([FromServices] BackgroundTaskQueue backgroundTaskQueue) =>
{
    for (int i = 0; i < 500; i++)
    {
        await backgroundTaskQueue.QueueBackgroundWorkItemAsync(async token =>
        {
            Console.WriteLine($"Mock image uploader started...");
            int reference = Random.Shared.Next(1, 1001);
            Console.WriteLine($"Image reference: {reference}");
            Console.WriteLine($"Uploaded to bucket:");
            Console.WriteLine($"Work completed!");
            Console.WriteLine($"==========================");
        });
    }
});

app.Run();
