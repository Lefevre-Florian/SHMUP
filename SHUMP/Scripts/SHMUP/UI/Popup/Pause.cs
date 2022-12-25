using Godot;
using System;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.Structure.Architecture;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Pause : Popup
	{

		[Export] private NodePath localisationPath = default;
		[Export] private NodePath resumePath = default;

		private CheckButton language;

		public override void _Ready()
		{
			base._Ready();

			language = GetNode<CheckButton>(localisationPath);

			GetNode<Button>(resumePath).Connect(EventButton.PRESSED, this, nameof(CloseScreen));

			language.Connect(EventButton.PRESSED, this, nameof(ChangeLanguage));

		}

		private void ChangeLanguage()
		{
			if (language.Pressed)
				LocalizationManager.SaveLanguageChange(Languages.FR);
			else
				LocalizationManager.SaveLanguageChange(Languages.ENG);
		}

	}

}