using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Structure;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class SoundSetting : HSlider
	{
		[Export] private string storingKey = "";

		private const string FILE_PATH = "res://Ressources/Settings.cfg";
		private const string SECTION_NAME = "Settings";

		private const int MIN_VALUE = -80;
		private const int MAX_VALUE = 24;

		public override void _Ready()
		{
			base._Ready();

			Connect(EventSlider.DRAG_ENDED, this, nameof(Save));

			MinValue = MIN_VALUE;
			MaxValue = MAX_VALUE;

			object lVolume = SaveManager.LoadData(SECTION_NAME, storingKey);
			if (lVolume != null)
				Value = (float)lVolume;
		}

		private void Save(bool pChanged)
        {
			if (pChanged)
				SaveManager.SaveData(SECTION_NAME, storingKey, Value);
        }

	}

}