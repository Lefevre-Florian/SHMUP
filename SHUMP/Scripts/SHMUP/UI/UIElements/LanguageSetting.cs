using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.Structure.Architecture;

namespace Com.IsartDigital.SHMUP.UI {

	public class LanguageSetting : CheckButton
	{

		public override void _Ready()
		{
			Connect(EventButton.PRESSED, this, nameof(ChangeLanguage));
		}

		private void ChangeLanguage()
		{
			if (Pressed)
				LocalizationManager.SaveLanguageChange(Languages.FR);
			else
				LocalizationManager.SaveLanguageChange(Languages.EN);
		}

	}

}