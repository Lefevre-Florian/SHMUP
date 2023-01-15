using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class TranslableButton : Button
	{

		[Export] private AudioStreamOGGVorbis sound = null;

		private LocalisationManager localisationManager = null;

		private string translationKey;

		public override void _Ready()
		{
			translationKey = Text.Trim();

			localisationManager = LocalisationManager.GetInstance();

			Translate();

			localisationManager.Connect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
			Connect(EventButton.PRESSED, this, nameof(Press));
		}

		private void Translate()
		{
			Text = localisationManager.GetTranslation(translationKey);
		}

		private void Press()
        {
			if (sound == null)
				return;

			SoundManager.GetInstance().GetAudioPlayer(sound, this);
        }

		private void Destructor()
		{
			if (localisationManager != null)
			{
				localisationManager.Disconnect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
				localisationManager = null;
			}

			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
		}

	}

}