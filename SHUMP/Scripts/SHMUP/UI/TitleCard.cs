using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Structure;

namespace Com.IsartDigital.SHMUP.UI {

	public class TitleCard : Screen
	{
		[Export] private NodePath startPath = default;
		[Export] private NodePath exitPath = default;

		[Export] private NodePath creditPath = default;
		[Export] private NodePath creditPanelPath = default;

		[Export] private NodePath settingPath = default;
		[Export] private NodePath settingPopup = default;

		[Export] private NodePath titlePath = default;
		[Export] private NodePath settingTitlePath = default;

		private const string PATH_GAME_SCENE = "res://Scenes/GameScene/Game.tscn";

		Button btnStart = null;
		Button btnCredit = null;
		Button btnExit = null;
		Button btnSetting = null;

		Label title = null;
		Label settingTitle = null;

		Popup settings = null;

		public override void _Ready()
		{
			base._Ready();

			GetTree().Paused = false;

			title = GetNode<Label>(titlePath);
			settingTitle = GetNode<Label>(settingTitlePath);

			btnStart = GetNode<Button>(startPath);
			btnStart.GrabFocus();
			btnStart.Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			
			btnExit = GetNode<Button>(exitPath);
			btnExit.Connect(EventButton.PRESSED, this, nameof(ExitGame));

			btnCredit = GetNode<Button>(creditPath);
			btnCredit.Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(creditPanelPath), this));

			btnSetting = GetNode<Button>(settingPath);
			btnSetting.Connect(EventButton.PRESSED, this, nameof(settings.OpenScreen));
		}

        protected override void UpdateAllText()
        {
			title.Text = localizationManager.GetTranslation(title.Text);

			btnStart.Text = localizationManager.GetTranslation(btnStart.Text);
			btnExit.Text = localizationManager.GetTranslation(btnExit.Text);
			btnCredit.Text = localizationManager.GetTranslation(btnCredit.Text);
			btnSetting.Text = localizationManager.GetTranslation(btnSetting.Text);
        }

        private void ChangeScene()
        {
			GetTree().ChangeScene(PATH_GAME_SCENE);
        }

		private void ExitGame()
        {
			GetTree().Quit();
        }

	}

}