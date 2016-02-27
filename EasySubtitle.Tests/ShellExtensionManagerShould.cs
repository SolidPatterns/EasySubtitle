using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class ShellExtensionManagerShould
    {

        private readonly string _registerDllName = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..\\..\\..\\EasySubtitle.ShellExtension\\bin\\Debug\\EasySubtitle.ShellExtension.dll"));
        private readonly string _srmFileLocation = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
            "..\\..\\..\\EasySubtitle.ShellExtension\\bin\\Debug\\srm.exe"));

        [SetUp]
        public void SetUp()
        {
            Debug.WriteLine(_registerDllName);
            Debug.WriteLine(_srmFileLocation);
        }

        [Test]
        public void Install()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = _srmFileLocation,
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true,
                
                Arguments = String.Format("install \"{0}\" -codebase", _registerDllName),
            });
        }

        [Test]
        public void Uninstall()
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = _srmFileLocation,
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true,

                Arguments = String.Format("uninstall \"{0}\"", _registerDllName),
            });
        }
    }
}