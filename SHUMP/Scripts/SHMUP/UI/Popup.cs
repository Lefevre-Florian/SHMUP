using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Popup : Screen
	{
		[Export] private List<NodePath> exitPaths = new List<NodePath>();

		private const string PATH_MAIN_MENU_SCENE = "res://Scenes/GameScene/MainMenu.tscn";
        private const string PATH_RELOAD_SCENE = "res://Scenes/GameScene/Game.tscn";

        private List<Button> exitButtons = new List<Button>();

		public override void _Ready()
		{
            base._Ready();

            Button lButton;
            foreach (NodePath lPath in exitPaths)
            {
                lButton = GetNode<Button>(lPath);
                exitButtons.Add(lButton);
				lButton.Connect(EventButton.PRESSED, this, nameof(CloseScreen));
                
            }
		}

        protected override void UpdateAllText()
        {
            foreach (Button lButton in exitButtons)
            {
                lButton.Text = localizationManager.GetTranslation(lButton.Text);
            }
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


	}

}