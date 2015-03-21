using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Xml;

namespace EasySubtitle.Business
{
    public class EasySubtitleConfig : IEasySubtitleConfig
    {
        private string _userAgent;
        private string _defaultSubtitleLanguage;
        private IList<string> _selectedSubtitleLanguages;
        public const string SubtitleClientUserAgentKey = "SubtitleClientUserAgent";
        public const string DefaultSubtitleLanguageKey = "DefaultSubtitleLanguage";
        public const string SelectedSubtitleLanguagesKey = "SelectedSubtitleLanguages";
    
        public EasySubtitleConfig()
        {
            _selectedSubtitleLanguages = new List<string>();
        }


        public string UserAgent
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_userAgent)) return _userAgent;

                var userAgent = GetConfigurationValue(SubtitleClientUserAgentKey);

                if (string.IsNullOrWhiteSpace(userAgent))
                    return _userAgent = "OSTestUserAgent";

                return _userAgent = userAgent;
            }
            set
            {
                if (value.Equals(UserAgent)) return;
                if (string.IsNullOrWhiteSpace(value)) return;

                _userAgent = value;
                SetConfigurationValue(SubtitleClientUserAgentKey, _userAgent);
            }
        }

        public string DefaultSubtitleLanguage
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_defaultSubtitleLanguage)) return _defaultSubtitleLanguage;

                var defaultSubtitleLanguage = GetConfigurationValue(DefaultSubtitleLanguageKey);
                if (string.IsNullOrWhiteSpace(defaultSubtitleLanguage))
                    return _defaultSubtitleLanguage = "eng";

                return _defaultSubtitleLanguage = defaultSubtitleLanguage;
            }
            set
            {
                if (value.Equals(UserAgent)) return;
                if (string.IsNullOrWhiteSpace(value)) return;

                _defaultSubtitleLanguage = value;
                SetConfigurationValue(DefaultSubtitleLanguageKey, _defaultSubtitleLanguage);
            }
        }

        public IList<string> SelectedSubtitleLanguages
        {
            get
            {
                if (_selectedSubtitleLanguages.Any()) return _selectedSubtitleLanguages;

                var selectedSubtitleLanguages = GetConfigurationValue(SelectedSubtitleLanguagesKey);
                if (string.IsNullOrWhiteSpace(selectedSubtitleLanguages))
                    return _selectedSubtitleLanguages = new[] { "eng" };

                try
                {
                    _selectedSubtitleLanguages = selectedSubtitleLanguages.Split(',');
                }
                catch
                {
                    _selectedSubtitleLanguages = new[] {"eng"};
                }

                return _selectedSubtitleLanguages;
            }
            set
            {
                if (value == null || value.Any()) return;
                if (value.Equals(SelectedSubtitleLanguages)) return;

                _selectedSubtitleLanguages = value;
                SetConfigurationValue(SelectedSubtitleLanguagesKey, string.Join(",", _selectedSubtitleLanguages));
            }
        }

        public static IEasySubtitleConfig GetEasySubtitleConfig()
        {
            return new EasySubtitleConfig();
        }

        private static string GetConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        private static void SetConfigurationValue(string key, string value)
        {
            //XmlDocument doc = new XmlDocument();
            
            //doc.Load(Directory.GetParent(Environment.CurrentDirectory).Parent.GetFiles("AppSettings.config").First().FullName);
            //doc.DocumentElement.GetElementsByTagName("appSettings")
            //var elements = doc.GetElementsByTagName("appSettings");

            //foreach (var element in doc.DocumentElement)
            //{
            //    element
            //}
            
            const string sectionName = "appSettings";
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = config.GetSection(sectionName);//OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = (AppSettingsSection) section;

            appSettings.Settings[key].Value = value;
            appSettings.SectionInformation.UnprotectSection();
            appSettings.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Modified);

            ConfigurationManager.RefreshSection(sectionName);
        }
    }
}