using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class ShellExtensionRegisterShould
    {
        [Test]
        public void Register()
        {
            const string registerDllName = "D:/Projects/SolidPatterns/EasySubtitle/EasySubtitle.ShellExtension/bin/Debug/EasySubtitle.ShellExtension.dll";
            const string srmFileLocation = "f:/Users/Kemal/Downloads/ServerManager (2)/ServerManager.exe";
            var dir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Console.WriteLine(dir);

            Process.Start(new ProcessStartInfo()
            {
                FileName = dir + "RegAsm.exe",
                Verb = "runas",
                Arguments = registerDllName
            });   

            //if (!Process.GetProcessesByName("ServerManager").Any())
            //{
 
            //}
            
            Process.Start(new ProcessStartInfo()
            {
                FileName = srmFileLocation,
                Verb = "runas",
                Arguments = String.Format("uninstall {0} -codebase unregister {0} -codebase install {0} -codebase register {0} -codebase", registerDllName),
            });
            
            Process[] explorers = Process.GetProcessesByName("explorer");
            foreach (Process explorer in explorers)
            {
                try
                {
                    explorer.Kill();
                }
                catch { }
            }
        }

        public void kill_explorer()
        {
            Process[] explorers = Process.GetProcessesByName("explorer");
            foreach (Process explorer in explorers)
            {
                try
                {
                    explorer.Kill();
                }
                catch { }
            }
        }
    }
}
