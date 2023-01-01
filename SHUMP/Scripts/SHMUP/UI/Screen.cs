using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class Screen : Control
	{

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

    }
}