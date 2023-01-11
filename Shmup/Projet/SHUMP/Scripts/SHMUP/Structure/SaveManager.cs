using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure {

	public static class SaveManager
	{
		private const string FILE_PATH = "res://Ressources/Settings.cfg";

		private struct Data
        {
			public string section;
			public string key;
			public object value;
            public Data(string pSection, string pKey, object pValue)
            {
				section = pSection;
				key = pKey;
				value = pValue;
            }
        };

		public static void SaveData(string pSectionName, string pKeyName, object pData)
        {
			ConfigFile lFile = new ConfigFile();
			Error lError = lFile.Load(FILE_PATH);
			if(lError == Error.Ok)
            {
				List<Data> lSectionsKeys = new List<Data>();
                foreach (string lSection in lFile.GetSections())
                {
                    foreach (string lKey in lFile.GetSectionKeys(lSection))
                    {
						lSectionsKeys.Add(new Data(lSection, lKey, lFile.GetValue(lSection, lKey)));
                    }
                }
				int lLength = lSectionsKeys.Count;
                for (int i = 0; i < lLength; i++)
                {
					lFile.SetValue(lSectionsKeys[i].section,
								   lSectionsKeys[i].key,
								   lSectionsKeys[i].value);
                }
            }
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