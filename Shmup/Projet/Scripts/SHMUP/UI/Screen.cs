using Com.IsartDigital.Utils.Events;
using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class Screen : Control
	{

        public override void _Ready()
        {
            Connect(EventNode.TREE_EXITING, this, nameof(Destructor));
        }

        public virtual void OpenScreen()
        {
			Visible = true;
        }

		public virtual void CloseScreen()
        {
			Visible = false;
        }

        protected void SwitchPanel(Screen pOpeningPanel, Screen pClosingPanel)
        {
            pOpeningPanel.OpenScreen();
            pClosingPanel.CloseScreen();
        }

        protected virtual void Destructor()
        {
            Disconnect(EventNode.TREE_EXITING, this, nameof(Destructor));
        }

    }
}