using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace Apress.SqlServer2005.SecurityChapter
{
   public class FileReader
   {
      public static string[] ReadFile(string filename)
      {
         FileIOPermission perm = new FileIOPermission(
                                        FileIOPermissionAccess.Read, filename);
         perm.Demand();

         ArrayList names = new ArrayList();
         FileStream fs = new FileStream(filename, FileMode.Open,
                                        FileAccess.Read);
         StreamReader sr = new StreamReader(fs);
         while (sr.Peek() >= 0)
            names.Add(sr.ReadLine());
         sr.Close();
         fs.Close();

         return (string[])names.ToArray(typeof(string));
      }
   }
}
