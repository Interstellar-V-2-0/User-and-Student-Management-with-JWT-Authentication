using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserStudentMgmt.Infrastructure.Data;
using UserStudentMgmt.Domain.Interfaces;
using UserStudentMgmt.Infrastructure.Repositories;
using UserStudentMgmt.Infrastructure.Seed; // 👈 Importamos el seeder

var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// Configuración de servicios
// ---------------------------------------------------------

// Controladores
builder.Services.AddControllers();

// Configuración avanzada de Swagger con soporte para JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "User & Student Management API",
        Description = "API REST para gestión de usuarios y estudiantes con autenticación JWT, desarrollada en .NET 8 con arquitectura en capas.",
        Contact = new OpenApiContact
        {
            Name = "Equipo Interstellar V 2.0",
            Email = "interstellar.team@example.com"
        }
    });

    // Definición del esquema de seguridad para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT así: Bearer {token}"
    });

    // Requisito de seguridad global para Swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ---------------------------------------------------------
// Configuración de Entity Framework Core con MySQL
// ---------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) // Ajustar si se usa otra versión de MySQL
    )
);

// ---------------------------------------------------------
// Inyección de dependencias (Repositorios)
// ---------------------------------------------------------
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();

// ---------------------------------------------------------
// Configuración del pipeline HTTP
// ---------------------------------------------------------
var app = builder.Build();

// 🔹 Ejecutar el Seeder antes de correr la app
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(dbContext);
}

if (app.Environment.IsDevelopment())
{
    // Swagger disponible en entorno de desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User & Student Management API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();

// Middleware para autenticación y autorización (se habilitará con JWT)
app.UseAuthorization();

// Mapeo de controladores
app.MapControllers();

app.Run();
