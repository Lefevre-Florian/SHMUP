using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Popup : Screen
	{

		[Export] private NodePath MainMenuPath = default;
		[Export] private List<NodePath> ExitPaths = new List<NodePath>();

		private const string PATH_MAIN_MENU_SCENE = "res://Scenes/UIPrefab/MainMenu.tscn";

		public override void _Ready()
		{
            foreach (NodePath lPath in ExitPaths)
            {
				GetNode<Button>(lPath).Connect(EventButton.PRESSED, this, nameof(CloseScreen));
            }

			GetNode<Button>(MainMenuPath).Connect(EventButton.PRESSED, this, nameof(ChangeScene));
		}

		private void ChangeScene()
        {
			GetTree().ChangeScene(PATH_MAIN_MENU_SCENE);
        }

	}

}