using System;
using System.Linq;

namespace EasySubtitle.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("EasySubtitle setup manager started.");
            try
            {
                String type;
                String targetDir;
                if (args == null || !args.Any())
                {
                    System.Console.WriteLine("Please enter type:");
                    type = System.Console.ReadLine();

                    System.Console.WriteLine("Please enter targetdir:");
                    targetDir = System.Console.ReadLine();
                }
                else
                {
                    type = args[0];
                    targetDir = args[1];
                }


                if (String.IsNullOrEmpty(type))
                {
                    throw new InvalidOperationException("Invalid type.");
                }
                if (String.IsNullOrEmpty(targetDir))
                {
                    throw new InvalidOperationException("Invalid target dir.");
                }

                System.Console.WriteLine("Type : {0}", type);
                System.Console.WriteLine("Targetdir : {0}", targetDir);

                var setupManager = new SetupManager(targetDir);
                System.Console.WriteLine("Initialized setup manager.");


                if (type.Equals("install", StringComparison.OrdinalIgnoreCase))
                {
                    System.Console.WriteLine("Starting install.");

                    setupManager.Install();
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("EasySubtitle successfully installed.");
                }
                if (type.Equals("uninstall", StringComparison.OrdinalIgnoreCase))
                {
                    System.Console.WriteLine("Starting uninstall.");

                    setupManager.Uninstall();
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("EasySubtitle successfully uninstalled.");
                }
            }
            catch (Exception exception)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("An error occurred. Exception details: {0}", exception.Message);
                System.Console.Read();
            }
            //System.Console.Read();
        }
    }
}
