using Microsoft.EntityFrameworkCore;
using UserStudentMgmt.Infrastructure.Data;
using UserStudentMgmt.Domain.Entities;

namespace UserStudentMgmt.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Aplica migraciones si aún no están
            await context.Database.MigrateAsync();

            // 🔹 Seed: Tipos de Documento
            if (!context.Set<DocumentType>().Any())
            {
                var docTypes = new List<DocumentType>
                {
                    new DocumentType { Name = "Citizenship Card" },
                    new DocumentType { Name = "Identity Card" },
                    new DocumentType { Name = "Foreigner ID" },
                    new DocumentType { Name = "Other" }
                };

                await context.Set<DocumentType>().AddRangeAsync(docTypes);
                await context.SaveChangesAsync();
            }

            // 🔹 Seed: Usuario administrador (solo si no existe)
            if (!context.Set<User>().Any(u => u.UserName == "admin"))
            {
                var firstDocType = await context.Set<DocumentType>().FirstAsync();
                if (firstDocType == null)
                {
                    throw new InvalidOperationException("Fatal error: DocumentType not found after attempting to seed.");
                }
                var adminUser = new User
                {
                    Name = "Administrador",
                    LastName = "General",
                    DocTypeId = firstDocType.Id,
                    DocumentNumber = "1000000000",
                    Email = "admin@mail.com",
                    PhoneNumber = "3000000000",
                    UserName = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123*")
                };

                context.Set<User>().Add(adminUser);
                await context.SaveChangesAsync();
            }
        }
    }
}