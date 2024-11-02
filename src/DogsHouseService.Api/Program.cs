using System.Threading.RateLimiting;
using DogsHouseService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Request.Path.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = builder.Configuration.GetValue<int>("RateLimiting:PermitLimit"),
                Window = TimeSpan.FromSeconds(builder.Configuration.GetValue<int>("RateLimiting:WindowInSeconds")),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = builder.Configuration.GetValue<int>("RateLimiting:QueueLimit")
            }));
    options.RejectionStatusCode = 429;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport(); 

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
{
    app.MigrateDatabase();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.MapControllers();

app.Run();