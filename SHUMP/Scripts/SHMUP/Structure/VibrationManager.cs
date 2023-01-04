using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class VibrationManager
	{
		public static bool vibrationState = true;

		private const string FILE_PATH = "res://Ressources/Settings.cfg";

		private const string SECTION_NAME = "Settings";
		private const string KEY_VIBRATION_STATE = "Vibration";

		private static bool connected = false;

		public static void InitVibrationManager()
        {
			LoadVibrationState();
			if (Input.GetConnectedJoypads().Count > 0)
				connected = true;
        }

		public static void SaveVibrationState(float pState)
        {
			ConfigFile lFile = new ConfigFile();
			lFile.SetValue(SECTION_NAME, KEY_VIBRATION_STATE, pState);
			lFile.Save(FILE_PATH);
		}

		public static void LoadVibrationState()
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if (lError == Error.Ok)
				vibrationState = (bool)lFile.GetValue(SECTION_NAME, KEY_VIBRATION_STATE);
        }

		public static void SetVibration(float pStrongMotorForce, float pWeakMotorForce, float pDuration = 0)
        {
            if (connected)
            {
				Input.StartJoyVibration(0, pWeakMotorForce, pStrongMotorForce, pDuration);
            }
        } 
 

	}

}