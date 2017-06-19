using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunAsAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            
            Directory.SetCurrentDirectory(appDir);

            if (args.Length == 0)
            {
                var fileList = Directory.GetFiles(".", "*.bat");
                if (fileList.Length == 1)
                {
                    var fileName = fileList.Single();
                    Process.Start(fileName);
                }
                else
                {
                    Console.WriteLine("Could not find a valid command to execute.");
                }
            }
            else
            {
                var cmd = args[0];

                var cmdArgs = args
                    .Skip(1)
                    .Select(a => !a.StartsWith("\"") && a.Contains(" ") ? a.Quote() : a);

                if (Debugger.IsAttached)
                {
                    Console.WriteLine(string.Join(":", cmdArgs));
                }

                Process.Start("cmd.exe", $"/c {cmd} {string.Join(" ", cmdArgs)}");
            }
        }


    }

    public static class StringExtensions
    {
        public static string Quote(this string s, int number = 1)
        {
            while (number-- > 0)
            {
                s = $"\"{s}\"";
            }

            return s;
        }
    }
}
