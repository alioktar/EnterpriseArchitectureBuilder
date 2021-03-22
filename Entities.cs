using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnterpriseArchitectureBuilder
{
    public class Entities
    {
        public Entities()
        {

        }

        public static void writeDtos(string path, string modelName)
        {
            string code = "using Core.Entities;\n" +
                    "using System;\n" +
                    "using System.ComponentModel.DataAnnotations;\n\n" +
                    "namespace Entities.Dtos\n" +
                    "{\n\n" +
                    "public class " + modelName + "Dto : Dto\n" +
                    "{\n" +
                    "}\n" +
                    "}";

            File.writeFile(code, path, modelName + "Dto.cs");
        }
    }
}
