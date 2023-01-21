using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Pause : GamePopup
	{

		[Export] private NodePath resumeBtnPath = default;
		[Export] private NodePath settingBtnPath = default;

		[Export] private PackedScene settingScene = default;

		Popup setting = null;

		public override void _Ready()
		{
			base._Ready();

			setting = settingScene.Instance<Popup>();

			Button lButton = GetNode<Button>(resumeBtnPath);
			lButton.GrabFocus();
			lButton.Connect(EventButton.PRESSED, this, nameof(UnPause));

			AddChild(setting);
			setting.CloseScreen();
			GetNode<Button>(settingBtnPath).Connect(EventButton.PRESSED, setting, nameof(setting.OpenScreen));
		}

		private void UnPause()
        {
			GetTree().Paused = false;
			CloseScreen();
        }

	}

}