using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class TitleCard : Screen
	{
		[Export] private NodePath startPath = default;
		[Export] private NodePath exitPath = default;

		[Export] private NodePath creditPath = default;
		[Export] private NodePath creditPanelPath = default;

		[Export] private NodePath settingPath = default;
		[Export] private NodePath settingPopup = default;

		private const string PATH_GAME_SCENE = "res://Scenes/GameScene/Game.tscn";

		private Button btnStart = null;
		private Button btnCredit = null;
		private Button btnExit = null;
		private Button btnSetting = null;

		private Popup settings = null;

		public override void _Ready()
		{
			base._Ready();
			GetTree().Paused = false;

			btnStart = GetNode<Button>(startPath);
			btnStart.GrabFocus();
			btnStart.Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			
			btnExit = GetNode<Button>(exitPath);
			btnExit.Connect(EventButton.PRESSED, this, nameof(ExitGame));

			btnCredit = GetNode<Button>(creditPath);
			btnCredit.Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(creditPanelPath), this));

			settings = GetNode<Popup>(settingPopup);

			btnSetting = GetNode<Button>(settingPath);
			btnSetting.Connect(EventButton.PRESSED, settings, nameof(settings.OpenScreen));
		}

        private void ChangeScene()
        {
			GetTree().ChangeScene(PATH_GAME_SCENE);
        }

		private void ExitGame()
        {
			GetTree().Quit();
        }

        protected override void Destructor()
        {
			btnStart.Disconnect(EventButton.PRESSED, this, nameof(ChangeScene));
			btnExit.Disconnect(EventButton.PRESSED, this, nameof(ExitGame));
			btnCredit.Disconnect(EventButton.PRESSED, this, nameof(SwitchPanel));
			btnSetting.Disconnect(EventButton.PRESSED, settings, nameof(settings.OpenScreen));
           
			base.Destructor();
        }

    }

}