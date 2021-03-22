using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EnterpriseArchitectureBuilder
{
    public class File
    {
        public static void writeFile(string code, string path, string fileName)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileInfo fi = new FileInfo(path + fileName);
            using (StreamWriter sw = new StreamWriter(path + fileName))
            {
                sw.WriteLine(code);
            }
        }
    }
}
