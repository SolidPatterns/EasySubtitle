using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OSDBnet;

namespace EasySubtitle.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            IAnonymousClient client = Osdb.Login("OSTestUserAgent");

            const string filePath = "F:/Videos/Series/Arrow Season 2 DIMENSION_/Arrow.S02E12.720p.HDTV.X264-DIMENSION.mkv";
            var subtitles = client.SearchSubtitlesFromFile("tur", filePath);
            foreach (var subtitle in subtitles)
            {
                Debug.WriteLine(subtitle.SubtitleFileName);
                Debug.WriteLine(subtitle.SubTitleDownloadLink);
                string directoryName = Path.GetDirectoryName(filePath);
                Debug.WriteLine(directoryName);
                client.DownloadSubtitleToPath(directoryName, subtitle);
            }
        }

        [Test]
        public void Register()
        {
            const string registerDllName = "D:/Sources/Workspaces/GitHub/EasySubtitle/EasySubtitle.Core/bin/Release/EasySubtitle.Core.dll";
            const string srmFileLocation = "D:/Sources/Workspaces/GitHub/EasySubtitle/EasySubtitle.Core/bin/Release/srm.exe";
            Process.Start(new ProcessStartInfo()
            {
                FileName = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + "RegAsm.exe",
                Verb = "runas",
                Arguments = registerDllName
            });
            Process.Start(new ProcessStartInfo()
            {
                FileName = srmFileLocation,
                Verb = "runas",
                Arguments = String.Format("install {0} -codebase", registerDllName)
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
    }
}
