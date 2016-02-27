using System;
using System.Diagnostics;
using System.IO;

namespace EasySubtitle.Business
{
    public class SetupManager
    {
        private readonly IEasySubtitleConfig _config;

        private const String InstallerExecutableName = "srm.exe";
        private const String ShellExtensionDllName = "EasySubtitle.ShellExtension.dll";

        public SetupManager()
        {
            _config = RegistryConfig.GetEasySubtitleConfig();
            
        }

        public void Install()
        {
            System.Console.WriteLine(InstallerExecutableLocation);
            System.Console.WriteLine(ShellExtensionDllLocation);
            Process.Start(new ProcessStartInfo()
                {
                    FileName = InstallerExecutableLocation,
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true,

                    Arguments = String.Format(@"install {0} -codebase", ShellExtensionDllLocation),
                });
        }

        public void Uninstall()
        {
            System.Console.WriteLine(InstallerExecutableLocation);
            System.Console.WriteLine(ShellExtensionDllLocation);
            Process.Start(new ProcessStartInfo()
                {
                    FileName = InstallerExecutableLocation,
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true,

                    Arguments = String.Format(@"uninstall {0}", ShellExtensionDllLocation),
                });
        }

        public string InstallerExecutableLocation
        {
            get { return Path.GetFullPath(String.Format("{0}\\{1}", _config.ApplicationDirectoryPath, InstallerExecutableName)); }
        }

        public string ShellExtensionDllLocation
        {
            get { return Path.GetFullPath(String.Format("{0}\\{1}", _config.ApplicationDirectoryPath, ShellExtensionDllName)); }
        }
    }
}
