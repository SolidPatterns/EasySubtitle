using System.Collections.Generic;
using System.Linq;
using EasySubtitle.Business;
using EasySubtitle.WPF.Models;

namespace EasySubtitle.WPF.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            GetCountries();
        }

        public IEnumerable<SubtitleLanguage> Languages { get; set; }

        public void GetCountries()
        {
            using (var client = EasySubtitleClientFactory.GetSubtitleClient())
            {
                Languages = client.GetSubLanguages().Select(language => new SubtitleLanguage { Checked = false, Language = language });
            }
        }
    }
}
