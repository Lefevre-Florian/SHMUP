using Godot;
using System;

namespace Com.IsartDigital.SHMUP.UI {

	public class Screen : Control
	{

		public void OpenPopup()
        {
			Visible = true;
        }

		public void ClosePopup()
        {
			Visible = false;
        }

	}
}