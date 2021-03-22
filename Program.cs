using System;
using System.Collections.Generic;
using System.IO;

namespace EnterpriseArchitectureBuilder
{
    class Program
    {
        static List<string> files = new List<string>();
        static void Main(string[] args)
        {
            string path = @"E:\Users\Ali Oktar\source\repos\DepoTakip\Entities\Concrete";
            string destPath = @"C:\Users\Ali Oktar\Desktop\Destination";

            Console.WriteLine("Model okuma işlemi başladı...\n");
            var allFiles = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                Console.WriteLine("\t\t----->"+Path.GetFileNameWithoutExtension(file));
                files.Add(Path.GetFileNameWithoutExtension(file));
            }

            files.Add("Email");
            files.Add("Log");
            files.Add("OperationClaim");
            files.Add("Password");
            files.Add("User");
            files.Add("UserOperationClaim");

            Console.WriteLine("\nModel okuma işlemi tamamlandı...");

            Console.WriteLine("Dosya yazma işlemi başladı...");
            foreach (var item in files)
            {
                Entities.writeDtos(destPath + @"\Entities\Dtos\", item);

                DataAccess.writeAbstract(destPath + @"\DataAccess\Abstract\", item);
                DataAccess.writeConcrete(destPath + @"\DataAccess\Concrete\", item);

                Business.writeAbstract(destPath + @"\Business\Services\Abstract\", item);
                Business.writeConcrete(destPath + @"\Business\Services\Concrete\", item);
                Business.writeConcrete(destPath + @"\Business\ValidationRules\FluentValidation\", item);
            }

            DataAccess.writeContext(destPath + @"\DataAccess\Concrete\EntityFramework\Contexts\", files);

            Business.writeBusinessProfile(destPath + @"\Business\AutoMapperProfile\", files);
            Business.writeMessages(destPath + @"\Business\Constants\", files);
            Business.writeAutofacBusinessProfile(destPath + @"\Business\DependencyResolvers\Autofac\", files);
            Business.writeIncludeStatic(destPath + @"\Business\Statics\", files);

            Console.WriteLine("Dosya yazma işlemi tamamlandı...");
        }

        //static void subDirectories(string path)
        //{
        //    foreach (var item in Directory.GetDirectories(path))
        //    {
        //        DirectoryInfo directoryInfo = new DirectoryInfo(item);
        //        subDirectories(item);
        //        getFiles(item);
        //    }
        //}

        //static void getFiles(string path)
        //{
        //    foreach (var item in new DirectoryInfo(path).GetFiles())
        //    {
        //        string[] fileName = item.Name.Split('.');

        //        files.Add(fileName[0]);
        //    }
        //}
    }
}
