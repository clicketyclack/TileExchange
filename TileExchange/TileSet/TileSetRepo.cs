

/* 
 * Copyright (C) 2017 Erik Mossberg
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

namespace TileExchange.TileSetRepo
{

	/// <summary>
	/// Discovery of tilesets.
	/// </summary>
	public class TileSetRepo
	{

		private List<ITileSet> found;

		public TileSetRepo()
		{
			found = new List<ITileSet>();
		}

		/// <summary>
		/// Populate list of found tilesets.
		/// </summary>
		public void Discover()
		{
			
			var tileset_path = UserSettings.GetDefaultPath("tileset_path");

			string[] files = Directory.GetFiles(tileset_path, "*.png");
			foreach (var file in files)
			{
				string packname = Path.GetFileNameWithoutExtension(file);
				var ts = new ChoppedBitmapTileSet(file, packname, 16, 16);
				found.Add(ts);
			}

			var hues = new List<float>();
			for (float f = 0.0f; f < 0.99; f += 0.1f)
			{
				hues.Add(f);
			}

			var parametric16 = new ProceduralHSVTileSet("parametric16", 16, 16, hues.ToArray(), 0.7f, 0.7f);
			found.Add(parametric16);
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
		/// Serialize this instance.
		/// </summary>
		/// <returns>JSON representation of this instance.</returns>
		public String Serialize()
		{
			return "";
		}

		/// <summary>
		/// Initialize/Construct a TileSetRepo via de-serialization from json.
		/// </summary>
		/// <returns>A de-serialized instance.</returns>
		/// <param name="serialized">JSon representation of Serialized object.</param>
		public static TileSetRepo DeSerialize(String serialized) {
			return new TileSetRepo();
		}
	}
}