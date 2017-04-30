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
using System.Reflection;
using System.Collections.Generic;
using System.Drawing;

namespace TileExchange.TileSet
{


	/// <summary>
	/// A single tile.
	/// </summary>
	public interface ITile
	{
		Size GetSize();
	}

	public class Tile : ITile
	{
		private Bitmap image;
		public Tile()
		{
			image = new Bitmap(16, 16);
		}

		public Size GetSize() {
			return new Size { Width = 16, Height = 16 };
		}
	}
		


	/// <summary>
	/// ITileSet defines the basic properties of a set of tiles.
	/// 
	/// Usually these will be loaded from a file, but can be generated as well.
	/// /// </summary>
	public interface ITileSet
	{
		int NumberOfTiles();
		ITile Tile(int tilenr);
	}

	public class TileSet : ITileSet
	{
		public TileSet(string url)
		{
		}

		public ITile Tile(int tilenr)
		{
			return new Tile();
		}

		public int NumberOfTiles()
		{
			return 0;
		}
	}

	/// <summary>
	/// Discovery of tilesets.
	/// </summary>
	public class TileSetFinder
	{

		private List<ITileSet> found;
		
		public TileSetFinder()
		{
			found = new List<ITileSet>();
			Discover();
		}

		/// <summary>
		/// Populate list of found tilesets.
		/// </summary>
		private void Discover()
		{
			var project_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var tileset_path = String.Format("{0}/../../../assets/tilesets/", project_path);
			string[] files = Directory.GetFiles(tileset_path, "*.png");
			foreach (var file in files)
			{
				var ts = new TileSet(file);
				found.Add(ts);
			}
		}

		public ITileSet this[int nr]
		{ 
			get
			{
				return found[nr];
			}

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
	}
}
