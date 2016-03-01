using System;
using System.Diagnostics;
using System.IO;
using EasySubtitle.Business;

namespace EasySubtitle.Console
{
    public class SetupManager
    {
        private readonly string _targetDir;

        private const String InstallerExecutableName = "srm.exe";
        private const String ShellExtensionDllName = "EasySubtitle.ShellExtension.dll";

        public SetupManager(String targetDir)
        {
            try
            {
                _targetDir = Path.GetFullPath(targetDir);
                System.Console.WriteLine("Initializing registery config.");
                RegistryConfig.Instance.ResetToDefaults(_targetDir);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Setup manager initialization failed. Details : {0}", ex.Message);
                throw;
            }
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

                    Arguments = String.Format("install \"{0}\" -codebase", ShellExtensionDllLocation),
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

                    Arguments = String.Format("uninstall \"{0}\"", ShellExtensionDllLocation),
                });
        }

        public string InstallerExecutableLocation
        {
            get { return Path.GetFullPath(String.Format("{0}\\{1}", _targetDir, InstallerExecutableName)); }
        }

        public string ShellExtensionDllLocation
        {
            get { return Path.GetFullPath(String.Format("{0}\\{1}", _targetDir, ShellExtensionDllName)); }
        }
    }
}
