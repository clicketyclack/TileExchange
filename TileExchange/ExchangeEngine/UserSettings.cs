using System;
using System.Collections.Generic;

using Newtonsoft.Json;


namespace TileExchange.ExchangeEngine
{
	public class UserSettings
	{
		[JsonProperty("last_directory")]
		public String last_directory { get; set; }

		[JsonProperty("serialized_tilesets")]
		public String[] serialized_tilesets { get; set; }


		private static Dictionary<String, String> default_paths;

		/// <summary>
		/// Static constructor for <see cref="T:TileExchange.ExchangeEngine.UserSettings"/> class. Finds default paths to project resources.
		/// </summary>
		static UserSettings() {
			default_paths = new Dictionary<string, string>();
			var user_abspath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			var project_path = System.IO.Path.Combine(user_abspath, "./.cwds/TileExchanger/");
			var images_path = System.IO.Path.Combine(project_path, "./assets/images/");
			var tileset_path = System.IO.Path.Combine(project_path, "./assets/tilesets/");
			var output_path = System.IO.Path.Combine(project_path, "./output/");

			default_paths["user_abspath"] = user_abspath;
			default_paths["project_path"] = project_path;
			default_paths["images_path"] = images_path;
			default_paths["tileset_path"] = tileset_path;
			default_paths["output_path"] = output_path;

		}


		public static String GetDefaultPath(String key) {
			return default_paths[key];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:TileExchange.ExchangeEngine.UserSettings"/> class. 
		/// All settings will get default values.
		/// </summary>
		public UserSettings()
		{
			this.last_directory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		}

		/// <summary>
		/// Load settings from user default location.
		/// </summary>
		/// <returns>The user settings.</returns>
		public static UserSettings LoadUserSettings() {
			String user_dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			String settings_path = "./.tile_exchange.json";
			String abspath = System.IO.Path.Combine(user_dir, settings_path); 

			UserSettings us = new UserSettings();

			return us;
		}


		public String serialize() {
			var jsonstr = JsonConvert.SerializeObject(this);
			return jsonstr;

		}


		/// <summary>
		/// Deserialize (load) a UserSettings object from a serialized string.
		/// </summary>
		/// <returns>The string containing a serialized UserSettings.</returns>
		/// <param name="serialized">Serialized.</param>
		public static UserSettings deserialize(String serialized) {
			UserSettings ss = new UserSettings();
			JsonConvert.PopulateObject(serialized, ss);
			return ss;

		}
	}
}
