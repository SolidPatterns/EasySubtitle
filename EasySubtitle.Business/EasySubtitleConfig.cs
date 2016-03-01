using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using EasySubtitle.Business.Models;

namespace EasySubtitle.Business
{
    public class EasySubtitleConfig : IEasySubtitleConfig
    {
        private string _userAgent;
        private const string ConfigFilePath = "config.cfg";
        private const string SubtitleClientUserAgentKey = "SubtitleClientUserAgent";
        private const string DefaultSubtitleLanguageKey = "DefaultSubtitleLanguage";
        private const string SelectedSubtitleLanguagesKey = "SelectedSubtitleLanguages";
        private readonly IDictionary<String, String> _configDictionary = new Dictionary<string, string>();

        public EasySubtitleConfig()
        {
            InitializeConfig();
        }

        private void InitializeConfig()
        {
            if (!File.Exists(ConfigFilePath))
            {
                IList<string> configList = new List<string>() { "DefaultSubtitleLanguage = eng" };
                File.WriteAllLines(ConfigFilePath, configList);
                return;
            }

            using (var streamReader = new StreamReader(ConfigFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var configLine = streamReader.ReadLine();
                    if (configLine == null) continue;

                    var configSplit = configLine.Split('=');
                    if (configSplit.Length < 1)
                    {
                        throw new InvalidOperationException("Config is corrupted.");
                    }

                    var configKey = configSplit[0].Trim();
                    var configValue = configSplit[1];

                    if (string.IsNullOrEmpty(configKey) || string.IsNullOrEmpty(configValue))
                    {
                        continue;
                    }

                    _configDictionary.Add(configKey, configValue);
                }
            }
        }


        public string UserAgent
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_userAgent)) return _userAgent;

                var userAgent = GetApplicationConfigurationValue(SubtitleClientUserAgentKey);

                if (string.IsNullOrWhiteSpace(userAgent))
                    return _userAgent = "EasySubtitleQuemarlos";

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
                var defaultSubtitleLanguage = GetConfigurationValue(DefaultSubtitleLanguageKey);
                if (string.IsNullOrWhiteSpace(defaultSubtitleLanguage))
                    return SubtitleLanguages.English;

                return defaultSubtitleLanguage;
            }
            set
            {
                if (value.Equals(UserAgent)) return;
                if (string.IsNullOrWhiteSpace(value)) return;

                SetConfigurationValue(DefaultSubtitleLanguageKey, value);
            }
        }

        public void SetDefaultSubtitileLanguage(String language)
        {
            SetConfigurationValue(DefaultSubtitleLanguageKey, language);
        }

        public void SetSelectedSubtitleLanguages(IList<string> languages)
        {
            if (languages == null || languages.Count == 0)
            {
                throw new ArgumentException("languages");
            }

            var languagesCommaSeparated = String.Join(",", languages);
            SetConfigurationValue(SelectedSubtitleLanguagesKey, languagesCommaSeparated);
        }

        public IList<string> SelectedSubtitleLanguages
        {
            get
            {
                var selectedSubtitleLanguages = GetConfigurationValue(SelectedSubtitleLanguagesKey);
                if (string.IsNullOrWhiteSpace(selectedSubtitleLanguages))
                    return new[] { SubtitleLanguages.English };

                String[] subtitleLanguages;
                try
                {
                    subtitleLanguages = selectedSubtitleLanguages.Split(',');
                }
                catch
                {
                    subtitleLanguages = new[] { SubtitleLanguages.English };
                }

                return subtitleLanguages;
            }
            set
            {
                if (value == null || value.Any()) return;
                if (value.Equals(SelectedSubtitleLanguages)) return;

                SetConfigurationValue(SelectedSubtitleLanguagesKey, string.Join(",", value));
            }
        }

        public string ApplicationDirectoryPath { get; set; }

        public void ResetToDefaults(string targetDir)
        {
            throw new NotImplementedException();
        }

        public static IEasySubtitleConfig GetEasySubtitleConfig()
        {
            return new EasySubtitleConfig();
        }

        private static string GetApplicationConfigurationValue(string key)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }

        private void SetConfigurationValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentException("key");
            if (string.IsNullOrEmpty(value)) throw new ArgumentException("value");

            var trimmedKey = key.Trim();
            if (_configDictionary.ContainsKey(trimmedKey))
            {
                _configDictionary.Remove(trimmedKey);
            }

            _configDictionary.Add(trimmedKey, value);
            File.WriteAllLines(ConfigFilePath, _configDictionary.Select(x => String.Format("{0}={1}", x.Key, x.Value)));
        }

        private String GetConfigurationValue(string key)
        {
            string value;
            _configDictionary.TryGetValue(key, out value);
            return value;
        }
    }
}