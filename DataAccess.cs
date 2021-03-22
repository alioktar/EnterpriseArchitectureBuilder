using System.Collections.Generic;

namespace EnterpriseArchitectureBuilder
{
    public class DataAccess
    {
        private DataAccess()
        {

        }

        public static void writeContext(string path, List<string> models)
        {

            string code = "using Core.Entities.Concrete;\n" +
                "using Entities.Concrete;\n" +
                "using Microsoft.EntityFrameworkCore;\n" +
                "using Microsoft.Extensions.Configuration;\n\n" +
                "namespace DataAccess.Concrete.EntityFramework.Contexts\n" +
                "{\n" +
                "public class EfContext : DbContext\n" +
                "{\n\n" +
                "protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)\n" +
                "{\n" +
                "var config = new ConfigurationBuilder()\n" +
                ".AddJsonFile(\"appsettings.json\")\n" +
                ".AddJsonFile($\"appsettings.Development.json\", true);\n\n" +
                "IConfigurationRoot _configuration = config.Build();\n" +
                "optionsBuilder.UseSqlServer(_configuration[\"SqlConnectionString\"]);\n" +
                "}\n\n" +
                "public DbSet<OperationClaim> OperationClaims { get; set; }\n" +
                "public DbSet<User> Users { get; set; }\n" +
                "public DbSet<UserOperationClaim> UserOperationClaims { get; set; }\n" +
                "public DbSet<Log> Logs { get; set; }\n\n";

            foreach (var model in models)
            {
                code += "public DbSet<" + model + "> " + model + " { get; set; }";
            }

            code += "\n}\n}";

            File.writeFile(code, path, "EfContext.cs");
        }

        public static void writeAbstract(string path, string modelName)
        {
            string code = "namespace DataAccess.Abstract\n" +
                    "{\n" +
                    "using Core.DataAccess;\n" +
                    "using Entities.Concrete.EntityFramework;\n" +
                    "public interface I" + modelName + "Dal : IEntityRepository<" + modelName + ">\n" +
                    "{\n}\n}";

            File.writeFile(code, path, "I" + modelName + "Dal.cs");
        }

        public static void writeConcrete(string path, string modelName)
        {
            string code = "namespace DataAccess.Concrete.EntityFramework\n" +
                    "{\n" +
                    "using Core.DataAccess.EntityFramework;\n" +
                    "using Entities.Concrete.EntityFramework;\n" +
                    "using DataAccess.Abstract;\n" +
                    "using DataAccess.Concrete.EntityFramework.Contexts;\n\n" +
                    "public class Ef" + modelName + "Dal : EfEntityRepositoryBase<" + modelName + ", EfContext>, I" + modelName + "Dal\n" +
                    "{\n}\n}";

            File.writeFile(code, path, "Ef" + modelName + "Dal.cs");
        }

    }
}
