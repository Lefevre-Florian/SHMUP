using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class SoundSetting : TranslableButton
	{

		[Export] private NodePath sliderPath = default;

		[Export] private string storingKey = "";

		private const string FILE_PATH = "res://Ressources/Settings.cfg";
		private const string SECTION_NAME = "Settings";

		private const int MIN_VALUE = -80;
		private const int MAX_VALUE = 24;

		private const int DEFAULT = -1;

		private Slider slider = null;

		public override void _Ready()
		{
			base._Ready();

			Connect(EventButton.PRESSED, this, nameof(Save));

			slider = GetNode<Slider>(sliderPath);

			slider.MinValue = MIN_VALUE;
			slider.MaxValue = MAX_VALUE;

			Loading();
		}

		private void Save()
        {
			ConfigFile lFile = new ConfigFile();
			lFile.SetValue(SECTION_NAME, storingKey, slider.Value);
			lFile.Save(FILE_PATH);
        }

		private void Loading()
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if(lError == Error.Ok)
            {
				int lVolume = (int)lFile.GetValue(SECTION_NAME, storingKey, DEFAULT);
				if (lVolume != DEFAULT)
					slider.Value = (int)lFile.GetValue(SECTION_NAME, storingKey);
			}
			
        }

	}

}