using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class TranslableLabel : Label
	{

		private LocalisationManager localisationManager = null;

		private string translationKey;

		public override void _Ready()
		{
			translationKey = Text.Trim();

			localisationManager = LocalisationManager.GetInstance();

			Translate();

			localisationManager.Connect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
		}

		private void Translate()
        {
			Text = localisationManager.GetTranslation(translationKey);
		}

		private void Destructor()
        {
			if(localisationManager != null)
            {
				localisationManager.Disconnect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
				localisationManager = null;
            }

			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
        }

	}

}