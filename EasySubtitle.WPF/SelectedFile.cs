using System;
using System.Collections.Generic;
using System.IO;

namespace EasySubtitle.WPF
{
    public class SelectedFile
    {
        public SelectedFile()
        {

        }

        public SelectedFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("filePath");

            File = filePath;
            Checked = true;
            Subtitles = new List<FoundSubtitle>();

            try
            {
                FileName = Path.GetFileName(filePath);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("File path is not correct.");
            }
        }

        public string File { get; set; }
        public string FileName { get; set; }
        public bool Checked { get; set; }
        public IList<FoundSubtitle> Subtitles { get; set; }

        public void CheckSubtitles()
        {
            var random = new Random();
            Subtitles.Add(new FoundSubtitle
            {
                Checked = true,
                SubtitleName = String.Format("subtitle {0}.srt", random.Next(0, 20))
            });

            Subtitles.Add(new FoundSubtitle
            {
                Checked = false,
                SubtitleName = String.Format("subtitle {0}.srt", random.Next(0, 20))
            });
        }
    }
}