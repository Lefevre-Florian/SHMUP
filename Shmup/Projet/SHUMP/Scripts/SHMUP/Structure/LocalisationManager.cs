using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure.Architecture;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.Structure {

	public class LocalisationManager : Node
	{
		private static LocalisationManager instance = null;

		private static Languages lang = Languages.FR;

		private const string SECTION_NAME = "Settings";
		private const string KEY_LANGUAGE_NAME = "Language";

		[Signal]
		public delegate void LanguageChanged();

		private LocalisationManager() : base() { }

		public override void _Ready()
        {
			if(instance != null)
            {
				QueueFree();
				return;
            }
			instance = this;

			Localisation.Init();

			object lRawData = SaveManager.LoadData(SECTION_NAME, KEY_LANGUAGE_NAME);
			if (lRawData != null)
				lang = (Languages)lRawData;

        }

		public static LocalisationManager GetInstance()
        {
			if (instance == null) instance = new LocalisationManager();
			return instance;
        }

        public void SaveLanguageChange(Languages pLanguage)
        {
			lang = pLanguage;

			EmitSignal(nameof(LanguageChanged));

			SaveManager.SaveData(SECTION_NAME, KEY_LANGUAGE_NAME, pLanguage);
        }

		public Languages GetCurrentLanguage()
        {
			return lang; 
        }

		public string GetTranslation(string pkey)
        {
			string lLocal;
			if (Localisation.translations[lang].TryGetValue(pkey, out lLocal))
				return lLocal;
			else
				return null;
        }

        protected override void Dispose(bool pDisposing)
        {
			if (pDisposing && instance != null) instance = null;
            base.Dispose(pDisposing);
        }

    }

}