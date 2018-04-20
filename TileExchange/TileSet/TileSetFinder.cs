/* 
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
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using TileExchange.Fragment;
using TileExchange.ExchangeEngine;
using TileExchange.TileSetTypes;

using Newtonsoft.Json;
using System;
namespace TileExchange.TileSetFinder
{
	public class TileSetFinder
	{
		public TileSetFinder()
		{
		}

		/// <summary>
		/// Finds tilesets in a directory.
		/// </summary>
		/// <returns>Found tilesets.</returns>
		/// <param name="recursive">If set to <c>true</c> recursive.</param>
		public static List<String> FindTilesets(String search_path, Boolean recursive)
		{

			var toreturn = new List<String>();
			FindInto(search_path, recursive, toreturn);
			return toreturn;
		}

		private static void FindInto(String search_path, Boolean recursive, List<String> found) {
			
			string[] files = Directory.GetFiles(search_path, "*.tset");


			foreach (var file in files)
			{

				var full_filepath = System.IO.Path.Combine(search_path, file);
				found.Add(full_filepath);

			}

			if (recursive)
			{
				string[] directories = Directory.GetDirectories(search_path);

				foreach (var dir in directories)
				{
					var full_filepath = System.IO.Path.Combine(search_path, dir);
					FindInto(full_filepath, recursive, found);
				}
			}


		}

	}
}
