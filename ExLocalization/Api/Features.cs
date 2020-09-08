using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Interfaces;

namespace ExLocalization.Api
{
	public static class Features
	{
		public static bool RegisterTranslation<T>(this IPlugin<IConfig> plugin, T translation, string language)
		{
			if (!ExLocalization.Loaded)
			{
				FileManager.InitialLoad();
				ExLocalization.Loaded = true;
			}

			if (!LanguageManager.LanguageExists(language)) return false;

			FileManager.SaveTranslation(plugin, translation, language);
			return true;
		}
	}
}