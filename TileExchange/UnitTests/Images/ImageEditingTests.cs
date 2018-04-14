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
using TileExchange.TileSetFinders;
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

			var tsfinder = new TileSetFinder();
			var tileset = (IHueMatchingTileset)tsfinder.ByName("stars")[0];
			var loader = new TesselatedImageLoader();
			var tesser = new Basic16Tesselator();
			var loaded_image = loader.LoadFromImagelibrary("green_leaf.jpg", tesser);

			var assembled_bitmap_pre = loaded_image.AssembleFragments();

			new BasicExchangeEngine(tileset, loaded_image).run();

			var assembled_bitmap = loaded_image.AssembleFragments();

			var writer = new ImageWriter();
			writer.WriteBitmap(assembled_bitmap_pre, "basic_output_pre.jpg");
			writer.WriteBitmap(assembled_bitmap, "basic_output.jpg");

		}
	}
}