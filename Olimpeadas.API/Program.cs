using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Olimpeadas.Aplication;
using Olimpeadas.Infraestructure;
using System.Text.Json.Serialization;
using Olimpeadas.API.Middleware;
using Olimpeadas.Infraestructure.Data.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Configure PostgreSQL DbContext, Repositories, and Security (Infrastructure layer)
builder.Services.AddInfrastructureServices(builder.Configuration);

//builder.Services.AddScoped<DbSeeder>();-------
builder.Services.AddScoped<Olimpeadas.Infraestructure.Data.Migrations.DbSeeder>();


// Configure Application services (AuthService, etc.)
builder.Services.AddApplicationServices();

// Configure JWT Authentication
var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"] 
    ?? throw new InvalidOperationException("JwtSettings:SecretKey no está configurado.");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
        ValidIssuer = jwtIssuer,
        ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
/*
 Esto permite que el frontend de React (corriendo en Vite) llame a esta API
Sin esto, el navegador bloquea los pedidos por política de CORS (el verdadero si no me deci quien sos, no pasas :> esto)*/
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendReact", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<Olimpeadas.Infraestructure.Data.Migrations.DbSeeder>();
    await seeder.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("FrontendReact");
//UseCors para conservar los headers de CORS en las respuestas, y que el frontend pueda leerlos. Esto es necesario para que el frontend pueda recibir los datos de la API sin problemas de CORS
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
