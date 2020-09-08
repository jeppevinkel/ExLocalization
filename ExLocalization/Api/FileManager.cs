using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Exiled.API.Interfaces;
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

		internal static void SaveTranslation<T>(IPlugin<IConfig> plugin, T translation, string language)
		{
			var culture = CultureInfo.GetCultureInfo(language);

			string path = Path.Combine(Path.Combine(TranslationsDir, plugin.Name), culture.Name);
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

		internal static T LoadTranslation<T>(IPlugin<IConfig> plugin)
		{
			string pluginPath = Path.Combine(TranslationsDir, plugin.Name);
			string[] translations = Directory.GetDirectories(pluginPath);
			var options = new List<string>();

			foreach (string translation in translations)
			{
				if (translation.EndsWith(CultureInfo.CurrentCulture.Name))
				{
					options.Add(translation);
					break;
				}
				if (translation.EndsWith(CultureInfo.CurrentCulture.TwoLetterISOLanguageName)) options.Add(translation);
				else if (translation.EndsWith("en")) options.Add(translation);
			}

			foreach (string file in options.Select(path => Path.Combine(path, "translation.yml")).Where(File.Exists))
			{
				return Deserializer.Deserialize<T>(File.ReadAllText(file));
			}

			foreach (string translation in translations.Where(t => File.Exists(Path.Combine(t, "translation.yml"))))
			{
				return Deserializer.Deserialize<T>(File.ReadAllText(Path.Combine(translation, "translation.yml")));
			}

			return default;
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
			Directory.CreateDirectory(TranslationsDir);
		}
	}
}