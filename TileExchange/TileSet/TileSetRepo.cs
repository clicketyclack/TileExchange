﻿/* 
 * Copyright (C) 2018 Erik Mossberg
 *
 * This file is part of TileExchanger.
 *
 * TileExchanger is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * TileExchanger is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 * 
 */
using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using TileExchange.Fragment;
using TileExchange.ExchangeEngine;
using TileExchange.TileSetTypes;

using Newtonsoft.Json;

namespace TileExchange.TileSetRepo
{

	/// <summary>
	/// Discovery of tilesets.
	/// </summary>
	public class TileSetRepo
	{

		private List<ITileSet> found;
		public List<Dictionary<String, String>> serialized_tsets { get; set; }

		public TileSetRepo()
		{
			found = new List<ITileSet>();
		}

		/// <summary>
		/// Discover any tilesets in a path.
		/// </summary>
		/// <returns>Nothing.</returns>
		/// <param name="search_path">Search path (optional). By default, the default UserSettings search path will be used.</param>
		/// <param name="recursive">If set to <c>true</c>, perform recursive directory search.</param>
		public void Discover(String search_path = null, Boolean recursive = true)
		{

			var tileset_path = search_path;
			if (tileset_path is null) {
				tileset_path = UserSettings.GetDefaultPath("tileset_path");
			}

			var population = new List<ITileSet>();

			FindTilesets(tileset_path, recursive, population);
			this.found = population;
		}

		/// <summary>
		/// Finds tilesets in a directory.
		/// </summary>
		/// <returns>Found tilesets.</returns>
		/// <param name="recursive">If set to <c>true</c> recursive.</param>
		private static void FindTilesets(String search_path, Boolean recursive, List<ITileSet> population) {

			string[] files = Directory.GetFiles(search_path, "*.tset");

			foreach (var file in files)
			{
				
				var full_filepath = System.IO.Path.Combine(search_path, file);

				LoadTsetFile(full_filepath, population);
			}

			if (recursive)
			{
				string[] directories = Directory.GetDirectories(search_path);

				foreach (var dir in directories)
				{
					var full_filepath = System.IO.Path.Combine(search_path, dir);
					FindTilesets(full_filepath, recursive, population);
				}
			}
		}

		/// <summary>
		/// Loads the tset file.
		/// </summary>
		/// <param name="abspath">Abspath.</param>
		/// <param name="population">Population.</param>
		private static void LoadTsetFile(String abspath, List<ITileSet> population) {
			
			var jsonstr = System.IO.File.ReadAllText(abspath);
			var tileset_type = TileSetTypes.TileSet.DetermineType(jsonstr);

			switch (tileset_type) {
				case "ProceduralHSVTileSet" : population.Add(ProceduralHSVTileSet.DeSerialize(jsonstr)); break;
				case "ChoppedBitmapTileSet": population.Add(ChoppedBitmapTileSet.DeSerialize(jsonstr)); break;
				default: throw new JsonException(String.Format("Could not determine tileset type from file {0}", abspath));
			}
		}

		public ITileSet this[int nr]
		{
			get
			{
				return found[nr];
			}

		}

		/// <summary>
		/// Find TileSets which match a given name pattern.
		/// </summary>
		/// <returns>List of TileSets.</returns>
		/// <param name="packname">Packname.</param>
		public List<ITileSet> ByName(string packname)
		{

			var toreturn = from tset in found
						   where tset.PackName() == packname
						   select tset;

			return toreturn.ToList();

		}


		public ITileSet TileSet(int nr)
		{
			return this[nr];
		}

		/// <summary>
		/// Number of tilesets found.
		/// </summary>
		/// <returns>Number of tilesets found.</returns>
		public int NumberOfTilesets()
		{
			return found.Count;
		}

		/// <summary>
		/// Adds a tile set to the repo.
		/// </summary>
		/// <param name="tset">Tset to add.</param>
		public void AddTileSet(ITileSet tset)
		{
			found.Add(tset);
		}

		/// <summary>
		/// Serialize this instance.
		/// </summary>
		/// <returns>JSON representation of this instance.</returns>
		public String Serialize()
		{

			var tset_strings = new List<Dictionary<String, String>>();
			foreach (ITileSet tset in this.found)
			{
				var tset_str = tset.Serialize();
				var tset_type = tset.GetType().Name;

				var tset_data = new Dictionary<String, String>();
				tset_data["tset_type"] = tset_type;
				tset_data["tset_serialized"] = tset_str;

				tset_strings.Add(tset_data);
			}

			this.serialized_tsets = tset_strings; /// JsonConvert.SerializeObject(tset_strings);
			System.Console.WriteLine("Have {0} tset_strings", this.serialized_tsets);
			var jsonstr = JsonConvert.SerializeObject(this, Formatting.Indented);
			return jsonstr;
		}

		/// <summary>
		/// Initialize/Construct a TileSetRepo via de-serialization from json.
		/// </summary>
		/// <returns>A de-serialized instance.</returns>
		/// <param name="serialized">JSon representation of Serialized object.</param>
		public static TileSetRepo DeSerialize(String serialized)
		{
			var tsr = new TileSetRepo();
			JsonConvert.PopulateObject(serialized, tsr);


			foreach (Dictionary<String, String> tset_data in tsr.serialized_tsets)
			{

				String tset_type = tset_data["tset_type"];
				String tset_serialized = tset_data["tset_serialized"];

				if (tset_type == "ProceduralHSVTileSet")
				{
					tsr.AddTileSet(ProceduralHSVTileSet.DeSerialize(tset_serialized));
				}

				System.Console.WriteLine(String.Format("type {0}, ser {1}", tset_type, tset_serialized));
			}
			return tsr;
		}
	}
}