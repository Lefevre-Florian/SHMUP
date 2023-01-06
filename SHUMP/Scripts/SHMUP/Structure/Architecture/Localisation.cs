using Godot;
using System;
using System.Collections.Generic;

namespace Com.IsartDigital.SHMUP.Structure.Architecture {

	public static class Localisation
	{

		public static Dictionary<Languages, Dictionary<string, string>> translations = new Dictionary<Languages, Dictionary<string, string>>();

		private const string FILE_PATH = "res://Ressources/Translation.csv";

		private const string COMMA_SEPARATOR = ";";

		private const int KEY_INDEX = 0;

		public static void Init()
		{
			File lFile = new File();
            if (lFile.FileExists(FILE_PATH))
            {
				lFile.Open(FILE_PATH, File.ModeFlags.Read);

				// Skip first line
				lFile.GetLine();

				Dictionary<string, string> lFr = new Dictionary<string, string>();
				Dictionary<string, string> lEn = new Dictionary<string, string>();

				int lFRIndex = (int)Languages.FR + 1;
				int lENIndex = (int)Languages.EN + 1;

				int lLineSize = Enum.GetNames(typeof(Languages)).Length + 1;

				string[] lLine;
                while (!lFile.EofReached())
                {
					lLine = lFile.GetCsvLine(COMMA_SEPARATOR);

					if (lLine.Length < lLineSize)
						continue;

					lFr.Add(lLine[KEY_INDEX], lLine[lFRIndex]);
					lEn.Add(lLine[KEY_INDEX], lLine[lENIndex]);
                }

				translations.Add(Languages.FR, lFr);
				translations.Add(Languages.EN, lEn);

				lFile.Close();
            }
		}

	}

}