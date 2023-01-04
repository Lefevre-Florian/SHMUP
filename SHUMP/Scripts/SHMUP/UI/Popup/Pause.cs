using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Pause : Popup
	{

		[Export] private NodePath resumePath = default;

		public override void _Ready()
		{
			base._Ready();

			Button lButton = GetNode<Button>(resumePath);
			lButton.GrabFocus();
			lButton.Connect(EventButton.PRESSED, this, nameof(CloseScreen));
		}

	}

}