using Godot;
using System;
using Com.IsartDigital.Utils.Events;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.UI {

	public class Settings : Screen
	{

		[Export] private NodePath buttonExitPath = default;
		[Export] private NodePath buttonReturnPath = default;

		[Export] private NodePath mainMenuPath = default;

		[Export] private NodePath subMenuSoundPath = default;
		[Export] private NodePath subBtnMenuSoundPath = default;

		[Export] private NodePath subMenuInputPath = default;
		[Export] private NodePath subBtnMenuInputPath = default;

		private List<Button> buttons = new List<Button>();

		public override void _Ready()
		{
			GetNode<Button>(buttonExitPath).Connect(EventButton.PRESSED, this, nameof(ExitGame));
			GetNode<Button>(buttonReturnPath).Connect(EventButton.PRESSED, 
													  this, 
													  nameof(SwitchPanel), 
													  new Godot.Collections.Array(GetNode<Screen>(mainMenuPath), this));
			
			Button lSoundBtn = GetNode<Button>(subBtnMenuSoundPath);
			Button lInputBtn = GetNode<Button>(subBtnMenuInputPath);

			lInputBtn.GrabFocus();

			lInputBtn.Connect(EventButton.PRESSED, this, nameof(SwitchPanel),
							  new Godot.Collections.Array(GetNode<Screen>(subMenuInputPath), GetNode<Screen>(subMenuSoundPath)));

			lSoundBtn.Connect(EventButton.PRESSED, this, nameof(SwitchPanel),
							  new Godot.Collections.Array(GetNode<Screen>(subMenuSoundPath), GetNode<Screen>(subMenuInputPath)));


		}

		private void ExitGame()
		{
			GetTree().Quit();
		}

	}

}