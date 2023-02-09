using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class TranslableButton : Button
	{

		[Export] private AudioStreamOGGVorbis sound = null;
		[Export (PropertyHint.Range, "1f, 5f, 0.1f")] private float growthFactor = 1.15f;

		private const string PROPERTY_RECT_SCALE = "rect_scale";
		private LocalisationManager localisationManager = null;

		private string translationKey;

		private Vector2 initialRectScale;

		public override void _Ready()
		{
			translationKey = Text.Trim();

			localisationManager = LocalisationManager.GetInstance();

			initialRectScale = RectScale;

			Translate();

			localisationManager.Connect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
			Connect(EventButton.PRESSED, this, nameof(Press));
			Connect(EventControl.MOUSE_ENTERED, this, nameof(OnFocusEntered));
			Connect(EventControl.MOUSE_EXITED, this, nameof(OnFocusExited));
		}

		private void Translate()
		{
			Text = localisationManager.GetTranslation(translationKey);
		}

		private void Press()
        {
			RectScale = initialRectScale;
			if (sound == null)
				return;

			SoundManager.GetInstance().GetAudioPlayer(sound, this, PauseModeEnum.Process);
        }

		private void OnFocusEntered()
        {
			RectScale *= growthFactor;
        }

		private void OnFocusExited()
        {
			RectScale = initialRectScale;
		}

		private void Destructor()
		{
			if (localisationManager != null)
			{
				localisationManager.Disconnect(nameof(LocalisationManager.LanguageChanged), this, nameof(Translate));
				localisationManager = null;
			}

			Disconnect(EventControl.MOUSE_ENTERED, this, nameof(OnFocusEntered));
			Disconnect(EventControl.MOUSE_EXITED, this, nameof(OnFocusExited));
			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
		}

	}

}