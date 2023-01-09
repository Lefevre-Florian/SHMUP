using Godot;
using System;
using Com.IsartDigital.Utils.Events;

namespace Com.IsartDigital.SHMUP.UI {

	public class Credit : Screen
	{

		[Export] private NodePath closeButtonPath = default;
        [Export] private NodePath titleCardPath = default;

        Button btnClose = null;

        public override void _Ready()
        {
            base._Ready();

            btnClose = GetNode<Button>(closeButtonPath);

            btnClose.Connect(EventButton.PRESSED, this, nameof(SwitchPanel), new Godot.Collections.Array(GetNode<Screen>(titleCardPath), this));
        }

        protected override void Destructor()
        {
            btnClose.Disconnect(EventButton.PRESSED, this, nameof(SwitchPanel));
            base.Destructor();
        }

    }

}