using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class Screen : Control
	{

		public void OpenScreen()
        {
			Visible = true;
        }

		public void CloseScreen()
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