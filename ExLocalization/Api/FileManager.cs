using System;
using System.Globalization;
using System.IO;
using Exiled.API.Interfaces;
using ExLocalization.Api.CustomYaml;
using YamlDotNet.Serialization;

namespace ExLocalization.Api
{
	public static class FileManager
	{
		private static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		private static readonly string ExiledDir = Path.Combine(AppData, "EXILED");
		private static readonly string TranslationsDir = Path.Combine(ExiledDir, "Translations");

		private static readonly IDeserializer Deserializer = new DeserializerBuilder().Build();
		private static readonly ISerializer Serializer = new SerializerBuilder().Build();

		public static void LoadTranslations()
		{
		}

		internal static void SaveTranslation<T>(IPlugin<IConfig> plugin, T translation, string language)
		{
			var culture = CultureInfo.GetCultureInfo(language);

			string path = Path.Combine(Path.Combine(TranslationsDir, culture.Name), plugin.Name);
			string file = Path.Combine(path, "translation.yml");
			string yaml;
			Directory.CreateDirectory(path);

			if (File.Exists(file))
			{
				var storedTranslation = Deserializer.Deserialize<T>(File.ReadAllText(file));
				yaml = Serializer.Serialize(storedTranslation);

				File.WriteAllText(file, yaml);
				return;
			}

			yaml = Serializer.Serialize(translation);

			File.WriteAllText(file, yaml);
		}

		internal static void InitialLoad()
		{
			GenerateFolders();
			LoadConfig();
		}

		internal static void LoadConfig()
		{
			string configDir = Path.Combine(ExiledDir, "Configs");
			string configFile = Path.Combine(configDir, "ExLocalization.yml");

			if (!File.Exists(configFile) || string.IsNullOrEmpty(File.ReadAllText(configFile)))
			{
				File.WriteAllText(configFile, Serializer.Serialize(new Config()));
				ExLocalization.Config = new Config();
				return;
			}

			ExLocalization.Config = Deserializer.Deserialize<Config>(File.ReadAllText(configFile));

			if (LanguageManager.LanguageExists(ExLocalization.Config.Language))
			{
				CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(ExLocalization.Config.Language);
			}
			else
			{
				ExLocalization.Config.Language = CultureInfo.CurrentCulture.Name;
				File.WriteAllText(configFile, Serializer.Serialize(ExLocalization.Config));
			}
		}

		internal static void GenerateFolders()
		{
			foreach (string cultureName in LanguageManager.CreateCultureNames(true))
				Directory.CreateDirectory(Path.Combine(TranslationsDir, cultureName));
		}
	}
}