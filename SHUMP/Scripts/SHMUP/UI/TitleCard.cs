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
		[Export] private NodePath settingPanelPath = default;

		private const string PATH_GAME_SCENE = "res://Scenes/GameScene/Game.tscn";

		public override void _Ready()
		{
			GetTree().Paused = false;

			LocalizationManager.InitLocalizationManager();

			Button lStartBtn = GetNode<Button>(startPath);
			lStartBtn.GrabFocus();

			lStartBtn.Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			GetNode<Button>(exitPath).Connect(EventButton.PRESSED, this, nameof(ExitGame));
			GetNode<Button>(creditPath).Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(creditPanelPath), this));
			GetNode<Button>(settingPath).Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(settingPanelPath), this));
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