using System;
using System.Linq;
using EasySubtitle.Business;

namespace EasySubtitle.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || !args.Any())
                return;

            var arg = args[0];

            var setupManager = new SetupManager();

            if (arg.Equals("install", StringComparison.OrdinalIgnoreCase))
            {
                setupManager.Install();
            }
            if (arg.Equals("uninstall", StringComparison.OrdinalIgnoreCase))
            {
                setupManager.Uninstall();
            }
        }
    }
}
