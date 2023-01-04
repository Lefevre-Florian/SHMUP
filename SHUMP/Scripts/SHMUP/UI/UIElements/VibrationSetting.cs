using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI.UIElements {

	public class VibrationSetting : CheckBox
	{

		public override void _Ready()
		{
			Connect(EventButton.PRESSED, this, nameof(OnPressed));
			Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
		}

		private void Destructor()
        {
			Disconnect(EventButton.PRESSED, this, nameof(OnPressed));
			Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
        }

		private void OnPressed()
        {
			
        }

	}

}