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
		String TileSetType { get; }
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
			var jsonstr = JsonConvert.SerializeObject(this, Formatting.Indented);
			return jsonstr;
		}

		private class MinimalTileset
		{
			public string TileSetType { get; set; }
		}

		/// <summary>
		/// Determines the tileset type.
		/// </summary>
		/// <returns>The type.</returns>
		/// <param name="jsonstr">Jsonstr.</param>
		public static String DetermineType(String jsonstr)
		{

			var results = JsonConvert.DeserializeObject<MinimalTileset>(jsonstr);
			return results.TileSetType;
		}

		public String TileSetType
		{
			get
			{
				return this.GetType().Name;
			}
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

		public String bitmap_fname { get; set; }
		public int twidth { get; set; }
		public int theight { get; set; }

		private Bitmap bitmap { get; set; }

		public ChoppedBitmapTileSet(string bitmap_fname, string packname, int twidth, int theight)
		{
			this.packname = packname;
			this.bitmap_fname = bitmap_fname;
			this.twidth = twidth;
			this.theight = theight;

			ReloadTiles();
		}

		/// <summary>
		/// Reloads the tiles from a bitmap object.
		/// </summary>
		/// <returns><c>true</c>, if tiles were successfully reloaded, <c>false</c> otherwise.</returns>
		public Boolean ReloadTiles()
		{

			try
			{

				Bitmap lbitmap = null;
				var abs_fname = Path.GetFullPath(this.bitmap_fname);

				try
				{

					lbitmap = new Bitmap(abs_fname);
				}
				catch (Exception exc)
				{
					var msg = String.Format("ReloadTiles() could not load bitmap from {0} with abspath {1}. Got exception {2}", this.bitmap_fname, abs_fname, exc.ToString());
					Console.Write(msg);
					return false;
				}

				var ltiles = new List<IFragment>();

				var tileCountX = Math.Floor((double)lbitmap.Width / twidth);
				var tileCountY = Math.Floor((double)lbitmap.Height / theight);

				for (var tilex = 0; tilex < tileCountX; tilex++)
				{
					for (var tiley = 0; tiley < tileCountY; tiley++)
					{
						var posx = tilex * twidth;
						var posy = tiley * theight;
						Bitmap t = lbitmap.Clone(new Rectangle(posx, posy, twidth, theight), lbitmap.PixelFormat);
						ltiles.Add(new BitmapFragment(t));
					}

				}

				this.bitmap = lbitmap;
				this.tiles = ltiles;
				return true;
			}
			catch (Exception exc)
			{
				var msg = String.Format("ReloadTiles() got unspecific exception {}.", exc.ToString());
				throw new Exception(msg);
			}
		}

		/// <summary>
		/// Initialize a default instance.
		/// </summary>
		/// <returns>The default.</returns>
		public static ChoppedBitmapTileSet Default()
		{
			return new ChoppedBitmapTileSet("default.png", "hello", 16, 16);
		}


		/// <summary>
		/// Initialize/Construct a ChoppedBitmapTileSet via de-serialization from json.
		/// </summary>
		/// <returns>A de-serialized instance.</returns>
		/// <param name="serialized">JSon representation of Serialized object.</param>
		public static ChoppedBitmapTileSet DeSerialize(String serialized)
		{
			ChoppedBitmapTileSet ts = ChoppedBitmapTileSet.Default();
			JsonConvert.PopulateObject(serialized, ts);
			return ts;

		}

	}
}



