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
using NUnit.Framework;
using TileExchange.TileSetTypes;
using TileExchange.TileSetRepo;
using TileExchange.ExchangeEngine;
using TileExchange.TesselatedImages;

namespace TileExchange
{

	/// <summary>
	/// Verifies basics of image loading. We should be able to find and open images from the assets directory.
	/// </summary>
	[TestFixture]
	public class ImageEditingTests
	{

		/// <summary>
		/// Verify that a BasicExchangeEngine can be created with correct arguments.
		/// </summary>
		[Test]
		public void BasicExchangeEngineTest()
		{

			var tsfinder = new TileSetRepo.TileSetRepo();
			tsfinder.Discover();
			var tileset_stars = (IHueMatchingTileset)tsfinder.ByName("stars")[0];
			var tileset_para16 = (IHueMatchingTileset)tsfinder.ByName("parametric16")[0];
			var loader = new TesselatedImageLoader();
			var tesser = new Basic16Tesselator();
			var loaded_image = loader.LoadFromImagelibrary("green_leaf.jpg", tesser);
			var writer = new ImageWriter();

			var output_path = UserSettings.GetDefaultPath("output_path");

			var assembled_bitmap_pre = loaded_image.AssembleFragments();
			writer.WriteBitmap(assembled_bitmap_pre, System.IO.Path.Combine(output_path, "green_leaf_unchanged.jpg"));

			new BasicExchangeEngine(tileset_stars, loaded_image).run();
			var assembled_bitmap_stars = loaded_image.AssembleFragments();
			writer.WriteBitmap(assembled_bitmap_stars, System.IO.Path.Combine(output_path, "stars_leaf_output.jpg"));

			new BasicExchangeEngine(tileset_para16, loaded_image).run();
			var assembled_bitmap_para16 = loaded_image.AssembleFragments();
			writer.WriteBitmap(assembled_bitmap_para16, System.IO.Path.Combine(output_path, "parametric_leaf_output.jpg"));

		}

	}
}