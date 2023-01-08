using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class GamePopup : Popup
	{

		[Export] private NodePath mainMenuPath = default;
		[Export] private NodePath reloadPath = default;

		private const string PATH_RELOAD_SCENE = "res://Scenes/GameScene/Game.tscn";

		public override void _Ready()
		{
			base._Ready();

			GetNode<Button>(mainMenuPath).Connect(EventButton.PRESSED, this, nameof(ChangeScene));
			GetNode<Button>(reloadPath).Connect(EventButton.PRESSED, this, nameof(Reload));
		}

		private void Reload()
		{
			GetTree().ChangeScene(PATH_RELOAD_SCENE);
		}

	}

}