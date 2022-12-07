using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class LocalizationManager
	{

		public static Dictionary<string, Dictionary<string, string>> translator = new Dictionary<string, Dictionary<string, string>>()
		{
			{"fr", new Dictionary<string, string>()
				{
					{"Pause", "Pause"},
					{"MainMenu" , "Menu principal"}
				} 
			},
			{"eng", new Dictionary<string, string>()
				{
					{"Pause", "Pause"},
					{"MainMenu" , "Main Menu"}
				} 
			}
		};

	}

}