using System.Globalization;

namespace ExLocalization
{
	public class Config
	{
		public string Language { get; internal set; } = CultureInfo.CurrentCulture.Name;
	}
}