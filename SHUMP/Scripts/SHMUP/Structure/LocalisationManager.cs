using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.SHMUP.Structure.Architecture;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class LocalizationManager
	{
		private static Languages lang = Languages.FR;

		private static Dictionary<Languages, Dictionary<string, string>> translator = new Dictionary<Languages, Dictionary<string, string>>()
		{
			{Languages.FR, new Dictionary<string, string>()
				{
					{"Pause", "Pause"},
					{"MainMenu" , "Menu principal"},
					{"Resume" , "Reprendre"},
					{"Exit" , "Quitter"},
					{"Credit", "Credit"},
					{"Setting", "Paramètres" }
				} 
			},
			{Languages.ENG, new Dictionary<string, string>()
				{
					{"Pause", "Pause"},
					{"MainMenu" , "Main Menu"},
					{"Resume" , "Resume"},
					{"Exit" , "Exit"},
					{"Credit", "Credit"},
					{"Setting", "Settings" }
				} 
			}
		};

		public static List<string> translationKeys = new List<string>();

		private const string FILE_PATH = "res://Ressources/Settings.cfg";
		
		private const string SECTION_NAME = "Settings";
		private const string KEY_LANGUAGE_NAME = "Language";
		
		static LocalizationManager()
        {
            foreach (string lKey in translator[lang].Keys)
            {
				translationKeys.Add(lKey);
            }
        }

		public static string GetTranslation(string pKey)
        {
			return translator[lang][pKey];
        }

		public static void SaveLanguageChange(Languages pLanguage)
        {
			lang = pLanguage;

			ConfigFile lFile = new ConfigFile();
			lFile.SetValue(SECTION_NAME, KEY_LANGUAGE_NAME, lang);
			lFile.Save(FILE_PATH);
        }

		public static void LoadLanguageSetting()
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if (lError != Error.Ok)
				SaveLanguageChange(lang);
			lang = (Languages)lFile.GetValue(SECTION_NAME, KEY_LANGUAGE_NAME);
        }

	}

}