using Godot;
using System;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class SaveManager
	{

		private const string FILE_PATH = "res://Ressources/Settings.cfg";

		public static void SaveData(string pSectionName, string pKeyName, object pData)
        {
			ConfigFile lFile = new ConfigFile();
			lFile.SetValue(pSectionName, pKeyName, pData);
			lFile.Save(FILE_PATH);
        }

		public static object LoadData(string pSectionName, string pKeyName)
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if(lError == Error.Ok)
            {
				if (lFile.HasSectionKey(pSectionName, pKeyName)) 
					return lFile.GetValue(pSectionName, pKeyName);
            }
			return null;
        }

	}

}