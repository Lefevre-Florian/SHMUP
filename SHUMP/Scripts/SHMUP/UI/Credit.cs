using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Credit : Screen
	{

		[Export] private NodePath closeButtonPath = default;

        private const string PATH_TITLE_CARD = "../TitleCard";

        public override void _Ready()
        {
            GetNode<Button>(closeButtonPath).Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Screen>(PATH_TITLE_CARD), this));
        }

    }

}