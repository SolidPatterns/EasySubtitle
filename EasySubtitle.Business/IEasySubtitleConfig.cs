﻿using System;
using System.Collections.Generic;

namespace EasySubtitle.Business
{
    public interface IEasySubtitleConfig
    {
        string UserAgent { get; set; }
        string DefaultSubtitleLanguage { get; set; }
        IList<string> SelectedSubtitleLanguages { get; set; }
        void SetDefaultSubtitileLanguage(String language);
        void SetSelectedSubtitleLanguages(IList<string> languages);
    }
}