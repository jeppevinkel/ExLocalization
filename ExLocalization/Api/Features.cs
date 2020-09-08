using System.Globalization;
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

		public static T LoadTranslation<T>(this IPlugin<IConfig> plugin)
		{
			return FileManager.LoadTranslation<T>(plugin);
		}

		public static CultureInfo GetLanguage()
		{
			return CultureInfo.CurrentCulture;
		}
	}
}