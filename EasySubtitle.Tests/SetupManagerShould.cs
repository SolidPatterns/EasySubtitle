using System.Diagnostics;
using EasySubtitle.Business;
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
            RegistryConfig.GetEasySubtitleConfig().ResetToDefaults();
            _setupManager = new SetupManager();
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