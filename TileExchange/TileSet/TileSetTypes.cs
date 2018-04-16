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

using Newtonsoft.Json;

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
		String Serialize();

	}

	public abstract class TileSet
	{
		protected List<IFragment> tiles;
		public String packname { get; set; }

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
		/// Serialize this instance.
		/// </summary>
		/// <returns>JSON representation of this instance.</returns>
		public String Serialize()
		{
			var jsonstr = JsonConvert.SerializeObject(this);
			return jsonstr;
		}




	}


	public interface IHueMatchingTileset : ITileSet
	{
		List<IFragment> TilesByHue(float wanted_hue, float tolerance);
	}

	public abstract class HueMatchingTileset : TileSet, IHueMatchingTileset
	{

		/// <summary>
		/// Return tiles by hue.
		/// </summary>
		/// <returns>The tiles.</returns>
		public List<IFragment> TilesByHue(float wanted_hue, float tolerance)
		{
			var toreturn = new List<IFragment>();

			var wanted_shift = (wanted_hue + 0.5) % 1.0;

			foreach (var tile in base.tiles)
			{
				var color = tile.AverageColor();
				var hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(color);

				var tile_hue = hsl.H;
				var tile_shifted = (hsl.H + 0.5) % 1.0;
				if ((wanted_hue - tolerance <= tile_hue && tile_hue <= wanted_hue + tolerance) ||
					(wanted_shift - tolerance <= tile_shifted && tile_shifted <= wanted_shift + tolerance))
				{
					toreturn.Add(tile);
				}

			}
			return toreturn;
		}
	}



	/// <summary>
	/// A procedural tileset constructed by generating tiles in a varying number of hues.
	/// 
	/// The saturation and luminosity of the tiles is fixed, as we only provide simple hue matching.
	/// </summary>
	public class ProceduralHSVTileSet : HueMatchingTileset, ITileSet
	{

		public int twidth { get; set; }
		public int theight { get; set; }
		public float[] hues { get; set; }
		public float saturation { get; set; }
		public float luminosity { get; set; }

		public ProceduralHSVTileSet(string packname, int twidth, int theight, float[] hues, float saturation, float luminosity)
		{
			this.packname = packname;
			this.twidth = twidth;
			this.theight = theight;
			this.hues = hues;
			this.saturation = saturation;
			this.luminosity = luminosity;


			this.tiles = new List<IFragment>();

			foreach (var hue in hues)
			{
				var color = ImageProcessor.Imaging.Colors.HslaColor.FromHslaColor(hue, saturation, luminosity);
				var tile = new ProceduralFragment(new Size { Height = theight, Width = twidth }, color);
				this.tiles.Add(tile);
			}
		}

		/// <summary>
		/// Initialize a default instance.
		/// </summary>
		/// <returns>The default.</returns>
		public static ProceduralHSVTileSet Default()
		{
			var hues = new float[] { 0.0f, (float)(1.0 / 3.0), (float)(2.0 / 3.0) };
			return new ProceduralHSVTileSet("default", 16, 16, hues, 0.7f, 0.7f);
		}

		/// <summary>
		/// Initialize/Construct a ProceduralHSVTileSet via de-serialization from json.
		/// </summary>
		/// <returns>A de-serialized instance.</returns>
		/// <param name="serialized">JSon representation of Serialized object.</param>
		public static ProceduralHSVTileSet DeSerialize(String serialized)
		{
			ProceduralHSVTileSet ts = ProceduralHSVTileSet.Default();
			JsonConvert.PopulateObject(serialized, ts);
			return ts;

		}


	}

	/// <summary>
	/// A tileset that is constructed by chopping up a bitmap image into n*m pixel sub-bitmaps.
	/// </summary>
	public class ChoppedBitmapTileSet : HueMatchingTileset, ITileSet
	{
		private Bitmap bitmap;


		public ChoppedBitmapTileSet(string url, string packname, int twidth, int theight)
		{
			this.bitmap = new Bitmap(url);
			this.tiles = new List<IFragment>();
			this.packname = packname;

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

	}
}



