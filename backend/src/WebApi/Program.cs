using Interpreter;
using Interpreter.Factories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using WebApi.Repositories;
using WebApi.Repositories.HistoryItem;
using WebApi.Services;
using WebApi.Services.Interfaces;
using WebApi.Services.SignalRHubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddHostedService<RequestHandlerHostedService>();
builder.Services.AddHostedService<BroadcastNewsHostedService>();
builder.Services.AddSingleton<ICommunicationHubServerNotifier, CommunicationHubServerNotifier>();
builder.Services.AddSingleton(typeof(IMessageQueue<>), typeof(MessageQueue<>));
builder.Services.AddSingleton(typeof(IConsumer<>), typeof(Consumer<>));
builder.Services.AddSingleton(typeof(IQueueProcessor<>), typeof(QueueProcessor<>));
builder.Services.AddSingleton<IAsyncActionItemRepository, AsyncActionItemRepository>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddSingleton<IEntityRecognizersFactory, DefaultEntityRecognizersFactory>();
builder.Services.AddSingleton<IRecognizersFactory, DefaultRecognizersFactory>();
builder.Services.AddSingleton<IInterpreterDispatcher, InterpreterDispatcher>();
builder.Services.AddScoped<IInterpreter, JohnTitor>();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

var staticOptions = new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "ClientApp")),
    RequestPath = "",
    OnPrepareResponse = ctx =>
    {
        var durationInSeconds = ctx.Context.Request.Path.ToString().ToLower().Contains("index.html")
        ? 1                  // No cache for the index.html to apply authentication pattern at the refresh
        : 60 * 60 * 24 * 7; // 1 week
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=" + durationInSeconds;
    }
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(cpb => cpb
//                    .WithOrigins("http://localhost:9000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true)
                    );

app.UseAuthorization();

app.UseStaticFiles(staticOptions);

app.MapControllers();

app.MapFallbackToFile("/index.html", staticOptions);

app.MapHub<CommunicationHub>("/communication-hub");

app.Run();
