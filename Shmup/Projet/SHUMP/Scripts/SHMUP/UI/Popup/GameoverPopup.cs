using Com.IsartDigital.SHMUP.Structure;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class GameoverPopup : GamePopup
	{

		[Export] private NodePath scoreLabelPath = default;

		Hud hud = null;

		public override void _Ready()
		{
			base._Ready();
			hud = Hud.GetInstance();

			GetNode<Label>(scoreLabelPath).Text = hud.GetScoreValue().ToString();
		}

	}

}