using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class VibrationManager
	{
		public static bool vibrationState = true;

		private const string SECTION_NAME = "Settings";
		private const string KEY_VIBRATION_STATE = "Vibration";

		private const string DISCONNECTED_CONTROLLER_INPUT = "joy_connection_changed";

		private const int USED_CONTROLLER = 0;

		private static bool connected = false;

		static VibrationManager()
        {
			object lRawData = SaveManager.LoadData(SECTION_NAME, KEY_VIBRATION_STATE);
			if (lRawData != null)
				vibrationState = (bool)lRawData;


			if (Input.GetConnectedJoypads().Count > 0)
				connected = true;

		}

		public static void LockVibration()
        {
			connected = false;
        }

		public static void SaveVibrationState(bool pState)
		{
			SaveManager.SaveData(SECTION_NAME, KEY_VIBRATION_STATE, pState);
		}

		public static void SetVibration(float pWeakMotorForce, float pStrongMotorForce, float pDuration = 0)
        {
            if (connected)
            {
				Input.StartJoyVibration(USED_CONTROLLER, pWeakMotorForce, pStrongMotorForce, pDuration);
            }
        } 
 

	}

}