using RetirementCalculator.Application;
using RetirementCalculator.Domain;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Retirement Calculator API", Version = "v1" });
});

// Register domain and application services
builder.Services.AddScoped<IRetirementCalculator, SuperannuationCalculator>();
builder.Services.AddScoped<ICompoundInterestCalculator, CompoundInterestCalculator>();
builder.Services.AddScoped<IProjectionService, ProjectionService>();

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Retirement Calculator API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();

// API endpoints
app.MapPost("/api/projection/calculate", async (ProjectionRequest request, IProjectionService service) =>
{
    var result = await service.CalculateProjectionAsync(request);
    return result.IsSuccess ? Results.Ok(result) : Results.BadRequest(result);
})
.WithName("CalculateProjection")
.WithTags("Projections");

app.MapGet("/api/projection/scenarios", (int age = 30, decimal balance = 50000) =>
{
    var scenarios = new[]
    {
        new { name = "Conservative", returnRate = 5.5, description = "Low risk, steady growth" },
        new { name = "Balanced", returnRate = 7.5, description = "Moderate risk, balanced approach" },
        new { name = "Growth", returnRate = 9.5, description = "Higher risk, aggressive growth" }
    };
    
    return Results.Ok(scenarios);
})
.WithName("GetScenarios")
.WithTags("Scenarios");

app.MapGet("/api/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
.WithName("HealthCheck")
.WithTags("Health");

// Fallback to serve static files (index.html)
app.MapFallbackToFile("index.html");

app.Run();
