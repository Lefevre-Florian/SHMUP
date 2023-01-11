using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class VibrationSetting : CheckButton
	{

		[Export] private string storingKey = "";

		private const string SECTION_NAME = "Setting";

		public override void _Ready()
		{
			Connect(EventButton.PRESSED, this, nameof(OnBtnPressed));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));

			object lRawData = SaveManager.LoadData(SECTION_NAME, storingKey);
			if (lRawData != null)
				Pressed = (bool)lRawData;
		}

		private void OnBtnPressed()
        {
			SaveManager.SaveData(SECTION_NAME, storingKey, Pressed);
        }

		private void Destructor()
        {
			Disconnect(EventButton.PRESSED, this, nameof(OnBtnPressed));
			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
		}

	}

}