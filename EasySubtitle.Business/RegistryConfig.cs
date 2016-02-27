using System;
using System.Collections.Generic;
using System.Linq;
using EasySubtitle.Business.Models;
using Microsoft.Win32;

namespace EasySubtitle.Business
{
    public class RegistryConfig : IEasySubtitleConfig
    {
        private readonly RegistryKey _registryKey;

        private const string DefaultUserAgent = "OSTestUserAgent";
        private const string SubtitleClientUserAgentKey = "SubtitleClientUserAgent";
        private const string DefaultSubtitleLanguageKey = "DefaultSubtitleLanguage";
        private const string SelectedSubtitleLanguagesKey = "SelectedSubtitleLanguages";
        private const string ApplicationDirectoryPathKey = "ApplicationDirectoryPath";

        public RegistryConfig()
        {
            _registryKey = Registry.CurrentUser.CreateSubKey("EasySubtitle");

            if (_registryKey == null)
                throw new InvalidOperationException("Program failed to start.");

            if (_registryKey.ValueCount == 0)
            {
                ResetToDefaults();
            }
        }

        public static IEasySubtitleConfig GetEasySubtitleConfig()
        {
            return new RegistryConfig();
        }

        public string UserAgent
        {
            get
            {
                return _registryKey.GetValue(SubtitleClientUserAgentKey).ToString();
            }
            set
            {
                _registryKey.SetValue(SubtitleClientUserAgentKey, value);
            }
        }

        public string DefaultSubtitleLanguage
        {
            get
            {
                return _registryKey.GetValue(SubtitleClientUserAgentKey).ToString();
            }
            set
            {
                _registryKey.SetValue(SubtitleClientUserAgentKey, value);
            }
        }
        public IList<string> SelectedSubtitleLanguages
        {
            get
            {

                return _registryKey.GetValue(SelectedSubtitleLanguagesKey)
                    .ToString().Split(',').ToList();
            }
            set
            {
                _registryKey.SetValue(SelectedSubtitleLanguagesKey, string.Join(",", value));
            }
        }

        public string ApplicationDirectoryPath
        {
            get
            {
                return _registryKey.GetValue(ApplicationDirectoryPathKey).ToString();
            }
            set
            {
                _registryKey.SetValue(ApplicationDirectoryPathKey, value);
            }
        }


        public void ResetToDefaults()
        {
            _registryKey.SetValue(SubtitleClientUserAgentKey, DefaultUserAgent);
            _registryKey.SetValue(DefaultSubtitleLanguageKey, SubtitleLanguages.Turkish);
            _registryKey.SetValue(SelectedSubtitleLanguagesKey, new List<String> { SubtitleLanguages.English, SubtitleLanguages.Turkish });

            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            const string companyDirectoryName = "SolidPatterns";
            const string applicationDirectoryName = "EasySubtitle";

            _registryKey.SetValue(ApplicationDirectoryPathKey, String.Format("{0}\\{1}\\{2}", programFiles, companyDirectoryName, applicationDirectoryName));
        }
    }
}