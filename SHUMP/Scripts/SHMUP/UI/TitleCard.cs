using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class TitleCard : Screen
	{
		[Export] private NodePath startPath = default;
		[Export] private NodePath exitPath = default;
		[Export] private NodePath creditPath = default;
		[Export] private NodePath settingPath = default;

		private const string PATH_GAME_SCENE = "res://Scenes/GameScene/Game.tscn";
		private const string PATH_CREDIT = "../Credit";

		public override void _Ready()
		{
			GetTree().Paused = false;

			GetNode<Button>(startPath).Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			GetNode<Button>(exitPath).Connect(EventButton.PRESSED, this, nameof(ExitGame));
			GetNode<Button>(creditPath).Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Control>(PATH_CREDIT), this));
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