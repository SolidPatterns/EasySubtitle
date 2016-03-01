using System;
using System.Diagnostics;
using EasySubtitle.Business;
using EasySubtitle.Console;
using NUnit.Framework;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class SetupManagerShould
    {
        private SetupManager _setupManager;

        [SetUp]
        public void SetUp()
        {
            RegistryConfig.Instance.ResetToDefaults(AppDomain.CurrentDomain.BaseDirectory);
            _setupManager = new SetupManager(AppDomain.CurrentDomain.BaseDirectory);
            Debug.WriteLine(_setupManager.InstallerExecutableLocation);
            Debug.WriteLine(_setupManager.ShellExtensionDllLocation);
        }

        [Test]
        public void Install_When_Called_Install()
        {
            _setupManager.Install();
        }

        [Test]
        public void Uninstall_When_Called_Uninstall()
        {
            _setupManager.Uninstall();
        }
    }
}