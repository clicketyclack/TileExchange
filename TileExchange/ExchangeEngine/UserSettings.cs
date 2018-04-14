using System;

using Newtonsoft.Json;


namespace TileExchange.ExchangeEngine
{
	public class UserSettings
	{
		public String last_directory { get; }

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


		public static UserSettings LoadFromFile(String filename) {
			UserSettings ss = new UserSettings();
			return ss;

		}
	}
}
