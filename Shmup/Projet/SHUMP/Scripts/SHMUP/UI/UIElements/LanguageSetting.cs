using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.Structure.Architecture;

namespace Com.IsartDigital.SHMUP.UI {

	public class LanguageSetting : CheckButton
	{
		[Export] private AudioStreamOGGVorbis sound = null;

		private LocalisationManager localisationManager = null;

		public override void _Ready()
		{
			localisationManager = LocalisationManager.GetInstance();
			Connect(EventButton.PRESSED, this, nameof(ChangeLanguage));

			if (localisationManager.GetCurrentLanguage() == Languages.FR)
				Pressed = true;
			else
				Pressed = false;
		}

		private void ChangeLanguage()
		{
			if(sound != null)
            {
				SoundManager.GetInstance().GetAudioPlayer(sound, this);
            }

			if (Pressed)
				localisationManager.SaveLanguageChange(Languages.FR);
			else
				localisationManager.SaveLanguageChange(Languages.EN);
		}

	}

}