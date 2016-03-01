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
        private static volatile RegistryConfig _instance = null;
        private static Object _synchronized = new object();

        private const string DefaultUserAgent = "EasySubtitleQuemarlos";
        private const string SubtitleClientUserAgentKey = "SubtitleClientUserAgent";
        private const string DefaultSubtitleLanguageKey = "DefaultSubtitleLanguage";
        private const string SelectedSubtitleLanguagesKey = "SelectedSubtitleLanguages";
        private const string ApplicationDirectoryPathKey = "ApplicationDirectoryPath";

        private RegistryConfig()
        {
            Console.WriteLine("Creating the registery sub key.");
            _registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\SolidPatterns\\EasySubtitle");

            if (_registryKey == null)
                throw new InvalidOperationException("Program failed to start.");
        }

        public static IEasySubtitleConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_synchronized)
                    {
                        if (_instance == null)
                        {
                            _instance = new RegistryConfig();
                        }
                    }   
                }

                return _instance;
            }
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
                return _registryKey.GetValue(DefaultSubtitleLanguageKey).ToString();
            }
            set
            {
                _registryKey.SetValue(DefaultSubtitleLanguageKey, value);
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


        public void ResetToDefaults(String targetDir)
        {
            Console.WriteLine("Resetting registery config to defaults. Target dir : {0}", targetDir);
            UserAgent = DefaultUserAgent;
            DefaultSubtitleLanguage =  SubtitleLanguages.Turkish;
            SelectedSubtitleLanguages = new List<String> { SubtitleLanguages.English, SubtitleLanguages.Turkish };
            ApplicationDirectoryPath = targetDir;
        }
    }
}