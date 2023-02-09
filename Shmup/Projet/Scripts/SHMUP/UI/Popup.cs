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
            Button lButton;
            foreach (NodePath lPath in exitPaths)
            {
                lButton = GetNode<Button>(lPath);
                exitButtons.Add(lButton);
				lButton.Connect(EventButton.PRESSED, this, nameof(CloseScreen));
            }
		}

        protected void ChangeScene()
        {
			GetTree().ChangeScene(PATH_MAIN_MENU_SCENE);
        }


	}

}