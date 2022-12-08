using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Credit : Screen
	{

		[Export] private NodePath titleCardPath = default;

        public override void _Ready()
        {
            GetNode<Button>(titleCardPath).Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array());
        }

    }

}