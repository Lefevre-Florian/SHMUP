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

		[Export] private NodePath titlePath = default;
		[Export] private NodePath settingTitlePath = default;

		private const string PATH_GAME_SCENE = "res://Scenes/GameScene/Game.tscn";

		private Button btnStart = null;
		private Button btnCredit = null;
		private Button btnExit = null;
		private Button btnSetting = null;

		private Label title = null;
		private Label settingTitle = null;

		private Popup settings = null;

		public override void _Ready()
		{
			GetTree().Paused = false;

			title = GetNode<Label>(titlePath);
			localTranslationKeys.Add(titlePath, title.Text);

			settingTitle = GetNode<Label>(settingTitlePath);
			localTranslationKeys.Add(settingTitlePath, settingTitle.Text);

			btnStart = GetNode<Button>(startPath);
			localTranslationKeys.Add(startPath, btnStart.Text);
			btnStart.GrabFocus();
			btnStart.Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			
			btnExit = GetNode<Button>(exitPath);
			localTranslationKeys.Add(exitPath, btnExit.Text);
			btnExit.Connect(EventButton.PRESSED, this, nameof(ExitGame));

			btnCredit = GetNode<Button>(creditPath);
			localTranslationKeys.Add(creditPath, btnCredit.Text);
			btnCredit.Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(creditPanelPath), this));

			settings = GetNode<Popup>(settingPopup);

			btnSetting = GetNode<Button>(settingPath);
			localTranslationKeys.Add(settingPath, btnSetting.Text);
			btnSetting.Connect(EventButton.PRESSED, settings, nameof(settings.OpenScreen));

			base._Ready();
		}

        protected override void UpdateAllText()
        {
			title.Text = localizationManager.GetTranslation(localTranslationKeys[titlePath]);

			btnStart.Text = localizationManager.GetTranslation(localTranslationKeys[startPath]);
			btnExit.Text = localizationManager.GetTranslation(localTranslationKeys[exitPath]);
			btnCredit.Text = localizationManager.GetTranslation(localTranslationKeys[creditPath]);
			btnSetting.Text = localizationManager.GetTranslation(localTranslationKeys[settingPath]);
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