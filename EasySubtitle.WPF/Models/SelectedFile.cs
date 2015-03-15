using System;
using System.Collections.Generic;
using System.IO;
using EasySubtitle.Business.Models;

namespace EasySubtitle.WPF.Models
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
        public IEnumerable<FoundSubtitle> Subtitles { get; set; }

        public string DirectoryPath
        {
            get { return Path.GetDirectoryName(File); }
        }

        public bool Searched { get; set; }
    }
}