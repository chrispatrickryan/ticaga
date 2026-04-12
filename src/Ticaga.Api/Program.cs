using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Ticaga.Api;
using Ticaga.Api.Jwt;
using Ticaga.Application;
using Ticaga.Application.Abstractions.Security;
using Ticaga.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);

var app = builder.Build();
ConfigureApp(app);

app.Run();

#region Service Configuration

static void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    ConfigureJwtAuthentication(builder);

    builder.Services.AddAuthorization();

    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter: Bearer {your JWT token}"
        });

        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });
    });
}

static void ConfigureJwtAuthentication(WebApplicationBuilder builder)
{
    builder.Services
        .AddOptions<JwtOptions>()
        .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
        .Validate(options => !string.IsNullOrWhiteSpace(options.Issuer), "JWT issuer is required.")
        .Validate(options => !string.IsNullOrWhiteSpace(options.Audience), "JWT audience is required.")
        .Validate(options => !string.IsNullOrWhiteSpace(options.SigningKey), "JWT signing key is required.")
        .Validate(options => options.SigningKey.Length >= 32, "JWT signing key must be at least 32 characters.")
        .Validate(options => options.ExpirationMinutes > 0, "JWT expiration minutes must be greater than zero.")
        .ValidateOnStart();

    var jwtOptions = builder.Configuration
        .GetSection(JwtOptions.SectionName)
        .Get<JwtOptions>() ?? throw new InvalidOperationException("JWT configuration is missing.");

    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtOptions.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();
}

#endregion

#region Application Configuration

static void ConfigureApp(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGet("/", () => "Ticaga API is running.");

    app.MapGet("/health", () => Results.Ok(new
    {
        status = "Healthy",
        service = "Ticaga.Api",
        utcTime = DateTime.UtcNow
    }));

    app.MapEndpoints();
}

#endregion