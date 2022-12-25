using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Popup : Screen
	{

		[Export] private NodePath mainMenuPath = default;
		[Export] private NodePath reloadPath = default;

		[Export] private List<NodePath> exitPaths = new List<NodePath>();

		private const string PATH_MAIN_MENU_SCENE = "res://Scenes/GameScene/MainMenu.tscn";

		public override void _Ready()
		{
            foreach (NodePath lPath in exitPaths)
            {
				GetNode<Button>(lPath).Connect(EventButton.PRESSED, this, nameof(CloseScreen));
            }

			GetNode<Button>(mainMenuPath).Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			GetNode<Button>(reloadPath).Connect(EventButton.PRESSED, this, nameof(Reload));
		}

		private void ChangeScene()
        {
			GetTree().ChangeScene(PATH_MAIN_MENU_SCENE);
        }

        public override void OpenScreen()
        {
            base.OpenScreen();
            GetTree().Paused = true;
            PauseMode = PauseModeEnum.Process;
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            GetTree().Paused = false;
            PauseMode = PauseModeEnum.Stop;
        }

        private void Reload()
        {
			GetTree().ReloadCurrentScene();
        }

	}

}