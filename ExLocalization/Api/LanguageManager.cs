using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExLocalization.Api
{
	internal class LanguageManager
	{
		private static readonly HashSet<string> CultureNames = CreateCultureNames();

		internal static bool LanguageExists(string name)
		{
			return CultureNames.Contains(name);
		}

		internal static HashSet<string> CreateCultureNames(bool onlyIso = false)
		{
			CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures)
				.Where(x => !string.IsNullOrEmpty(x.Name))
				.ToArray();
			var allNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (!onlyIso) allNames.UnionWith(cultureInfos.Select(x => x.TwoLetterISOLanguageName));
			allNames.UnionWith(cultureInfos.Select(x => x.Name));

			return allNames;
		}
	}
}