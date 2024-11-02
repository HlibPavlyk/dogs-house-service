using DogsHouseService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
        {
            NamingStrategy = new Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy()
        };
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport(); 


builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Migrates the database if the application is running in production.
if (app.Environment.IsProduction())
{
    app.MigrateDatabase();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();