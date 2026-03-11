using Ticaga.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Ticaga API is running.");

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    service = "Ticaga.Api",
    utcTime = DateTime.UtcNow
}));

app.MapEndpoints();

app.Run();
