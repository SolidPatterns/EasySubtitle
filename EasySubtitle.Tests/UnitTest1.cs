using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            var subtitle = subtitles.FirstOrDefault();
            if (subtitle != null)
            {
                Debug.WriteLine(subtitle.SubtitleFileName);
                Debug.WriteLine(subtitle.SubTitleDownloadLink);
                string directoryName = Path.GetDirectoryName(filePath);
                Debug.WriteLine(directoryName);

                var directoryPath = Path.GetDirectoryName(filePath);
                client.DownloadSubtitleToPath(directoryPath, subtitle);
                File.Move(String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(), subtitle.SubtitleFileName)
                    , String.Concat(directoryPath, Path.DirectorySeparatorChar.ToString(), Path.GetFileNameWithoutExtension(filePath), ".srt"));
            }


        }

        [Test]
        public void Register()
        {
            const string registerDllName = "D:/Sources/Workspaces/GitHub/EasySubtitle/EasySubtitle.ShellExtension/bin/Debug/EasySubtitle.ShellExtension.dll";
            const string srmFileLocation = "D:/Sources/Workspaces/GitHub/EasySubtitle/EasySubtitle.ShellExtension/bin/Debug/srm.exe";
            var dir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Console.WriteLine(dir);
            Process.Start(new ProcessStartInfo()
            {
                FileName = dir + "RegAsm.exe",
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
