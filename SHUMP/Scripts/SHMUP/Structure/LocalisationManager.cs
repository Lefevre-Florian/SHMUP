using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.Structure.Architecture;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class LocalizationManager
	{
		private static Languages lang = Languages.FR;

		private const string FILE_PATH = "res://Ressources/Settings.cfg";
		
		private const string SECTION_NAME = "Settings";
		private const string KEY_LANGUAGE_NAME = "Language";

		[Signal]
		public delegate void LanguageChanged();

		public static void InitLocalizationManager()
        {
			LoadLanguageSetting();
			TranslationServer.SetLocale(lang.ToString().ToLower());
		}

		public static void SaveLanguageChange(Languages pLanguage)
        {
			TranslationServer.SetLocale(pLanguage.ToString().ToLower());
			lang = pLanguage;

			ConfigFile lFile = new ConfigFile();
			lFile.SetValue(SECTION_NAME, KEY_LANGUAGE_NAME, lang);
			lFile.Save(FILE_PATH);
        }

		private static void LoadLanguageSetting()
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if (lError != Error.Ok)
				SaveLanguageChange(lang);
			lang = (Languages)lFile.GetValue(SECTION_NAME, KEY_LANGUAGE_NAME);
        }

		public static Languages GetCurrentLanguage()
        {
			return lang;
        }

	}

}