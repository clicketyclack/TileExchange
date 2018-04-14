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

namespace TileExchange.TileSetTypes
{
	
		
	/// <summary>
	/// ITileSet defines the basic properties of a set of tiles.
	/// 
	/// Usually these will be loaded from a file, but can be generated as well.
	/// /// </summary>
	public interface ITileSet
	{
		int NumberOfTiles();
		IFragment Tile(int tilenr);
		String PackName();
	}

	public interface IHueMatchingTileset : ITileSet {
		List<IFragment> TilesByHue(float wanted_hue, float tolerance);
	}

	public class ChoppedBitmapTileSet : ITileSet, IHueMatchingTileset
	{
		private Bitmap bitmap;
		private List<IFragment> tiles;
		private String packname;

		public ChoppedBitmapTileSet(string url, string packname, int twidth, int theight)
		{
			this.bitmap = new Bitmap(url);
			this.tiles = new List<IFragment>();
			this.packname = packname;

			//var twidth = 16;
			//var theight = 16;
			var tileCountX = Math.Floor((double)bitmap.Width / twidth);
			var tileCountY = Math.Floor((double)bitmap.Height / theight);

			for (var tilex = 0; tilex < tileCountX; tilex++)
			{
				for (var tiley = 0; tiley < tileCountY; tiley++)
				{
					var posx = tilex * twidth;
					var posy = tiley * theight;
					Bitmap t = bitmap.Clone(new Rectangle(posx, posy, twidth, theight), bitmap.PixelFormat);
					this.tiles.Add(new BitmapFragment(t));

				}
			}	
		}

		public String PackName()
		{
			return packname;
		}

		public IFragment Tile(int tilenr)
		{
			return tiles[tilenr];
		}

		public int NumberOfTiles()
		{
			return tiles.Count;
		}


		/// <summary>
		/// Return tiles by hue.
		/// </summary>
		/// <returns>The tiles.</returns>
		public List<IFragment> TilesByHue(float wanted_hue, float tolerance)
		{
			var toreturn = new List<IFragment>();

			var wanted_shift = (wanted_hue + 0.5) % 1.0;

			foreach (var tile in tiles)
			{
				var color = tile.AverageColor();
				var hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(color);

				var tile_hue = hsl.H;
				var tile_shifted = (hsl.H + 0.5) % 1.0 ;
				if ((wanted_hue - tolerance <= tile_hue && tile_hue <= wanted_hue + tolerance) ||
				    (wanted_shift - tolerance <= tile_shifted && tile_shifted  <= wanted_shift + tolerance) )
				{
					toreturn.Add(tile);
				}

			}
			return toreturn;
		}
	}
}
