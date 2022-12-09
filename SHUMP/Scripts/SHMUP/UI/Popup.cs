using Godot;
using System;
using System.Collections.Generic;
using Com.IsartDigital.Utils.Events;
using Com.IsartDigital.SHMUP.Structure;
using Com.IsartDigital.SHMUP.Structure.Architecture;

namespace Com.IsartDigital.SHMUP.UI {

	public class Popup : Screen
	{

		[Export] private NodePath mainMenuPath = default;
		[Export] private NodePath languagePath = default;
		[Export] private NodePath resumePath = default;
		[Export] private NodePath reloadPath = default;

		[Export] private List<NodePath> exitPaths = new List<NodePath>();

		private const string PATH_MAIN_MENU_SCENE = "res://Scenes/GameScene/MainMenu.tscn";

		private CheckButton language;

		public override void _Ready()
		{
            foreach (NodePath lPath in exitPaths)
            {
				GetNode<Button>(lPath).Connect(EventButton.PRESSED, this, nameof(CloseScreen));
            }

			GetNode<Button>(mainMenuPath).Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			GetNode<Button>(resumePath).Connect(EventButton.PRESSED, this, nameof(Resume));
			GetNode<Button>(reloadPath).Connect(EventButton.PRESSED, this, nameof(Reload));

			language = GetNode<CheckButton>(languagePath);
			language.Connect(EventButton.PRESSED, this, nameof(ChangeLanguage));

		}

		private void ChangeScene()
        {
			GetTree().ChangeScene(PATH_MAIN_MENU_SCENE);
        }

		private void ChangeLanguage()
        {
			if (language.Pressed)
				LocalizationManager.SaveLanguageChange(Languages.FR);
			else
				LocalizationManager.SaveLanguageChange(Languages.ENG);
		}

		private void Reload()
        {
			GetTree().ReloadCurrentScene();
        }

		private void Resume()
        {
			GetTree().Paused = false;
			CloseScreen();
        }

	}

}