using dev_in_house_basic_jwt;
using dev_in_house_basic_jwt.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Chave do Secrets do appsetins.json
JWTConfiguracaoAppsetings.Secrets = builder.Configuration.GetValue<string>("JWT:Secret");
JWTConfiguracaoAppsetings.Issuer = builder.Configuration.GetValue<string>("JWT:Issuer");
JWTConfiguracaoAppsetings.Audience = builder.Configuration.GetValue<string>("JWT:Audience");

//Enconding em ASCII da chave Secrets [array de bytes]
JWTConfiguracaoAppsetings.Key = Encoding.ASCII.GetBytes(JWTConfiguracaoAppsetings.Secrets);

/// <summary>
/// Configuração do JWT
/// </summary>
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JWTConfiguracaoAppsetings.Issuer,
        ValidAudience = JWTConfiguracaoAppsetings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(JWTConfiguracaoAppsetings.Key)
    };
});

builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, ExampleAuthorizationMiddlewareResultHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(async swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Exemplo Criado",
        Description = "JWT Descrição criada"
    });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Utilizando JWT Authorization para autenticar"
    });

    var OpenApiReference = new OpenApiReference()
    {
        Type = ReferenceType.SecurityScheme,
        Id = "Bearer"
    };

    var openApiSecurityScheme = new OpenApiSecurityScheme()
    {
        Reference = OpenApiReference
    };

    var openApiSecuryRequeriment = new OpenApiSecurityRequirement()
    {
        {
            openApiSecurityScheme,
            Array.Empty<string>()
        }
    };

    swagger.AddSecurityRequirement(openApiSecuryRequeriment);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();

app.Run();
