using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace EasySubtitle.Business
{
    public class RegistryConfig : IEasySubtitleConfig
    {
        private RegistryKey _registryKey;

        private const string DefaultUserAgent = "OSTestUserAgent";
        private const string SubtitleClientUserAgentKey = "SubtitleClientUserAgent";
        private const string DefaultSubtitleLanguageKey = "DefaultSubtitleLanguage";
        private const string SelectedSubtitleLanguagesKey = "SelectedSubtitleLanguages";

        public RegistryConfig()
        {
            _registryKey = Registry.CurrentUser.CreateSubKey("EasySubtitle");

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

        public void ResetToDefaults()
        {
            _registryKey.SetValue(SubtitleClientUserAgentKey, DefaultUserAgent);
            _registryKey.SetValue(DefaultSubtitleLanguageKey, "en");
            _registryKey.SetValue(SelectedSubtitleLanguagesKey, "en");
        }
    }
}