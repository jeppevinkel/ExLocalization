using System.Globalization;

namespace ExLocalization
{
	internal class Config
	{
		public string Language { get; internal set; } = CultureInfo.CurrentCulture.Name;
	}
}