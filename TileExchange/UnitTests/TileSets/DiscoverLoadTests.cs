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
using NUnit.Framework;
using TileExchange.TileSetTypes;
using TileExchange.TileSetRepo;
using TileExchange.ExchangeEngine;

namespace TileExchange
{

	public class DiscoverLoadTests
	{

		private TileSetRepo.TileSetRepo five_mixed_tsets { get; set; }

		[OneTimeSetUp]
		public void OneTime() 
		{
			var tileset_path = UserSettings.GetDefaultPath("tileset_path");
			tileset_path = System.IO.Path.Combine(tileset_path, "test");
			tileset_path = System.IO.Path.Combine(tileset_path, "5_mixed_tsets");

			five_mixed_tsets = new TileSetRepo.TileSetRepo();
			five_mixed_tsets.Discover(tileset_path, false);
		}


		/// <summary>
		/// Check number of tilesets in a directory.
		/// </summary>
		[Test]
		public void DiscoverTileSets()
		{

			var tileset_path = UserSettings.GetDefaultPath("tileset_path");
			tileset_path = System.IO.Path.Combine(tileset_path, "test");

			var tsr_root = new TileSetRepo.TileSetRepo();
			tsr_root.Discover(tileset_path, false);
			Assert.AreEqual(2, tsr_root.NumberOfTilesets());

			var tsr_root_recur = new TileSetRepo.TileSetRepo();
			tsr_root_recur.Discover(tileset_path, true);
			Assert.AreEqual(10, tsr_root_recur.NumberOfTilesets());
		}

		/// <summary>
		/// Verify that tileset discovery correctly identifies tileset types.
		/// </summary>
		[Test]
		public void DiscoverTileSetsHandleType()
		{
			var found = five_mixed_tsets.ByName("Stars (8x16)");
			Assert.AreEqual("ChoppedBitmapTileSet", found[0].TileSetType);

			found = five_mixed_tsets.ByName("16 pastels (32x32)");
			Assert.AreEqual("ProceduralHSVTileSet", found[0].TileSetType);
		}


		/// <summary>
		/// Verify that tileset discovery correctly identifies tileset types.
		/// </summary>
		[Test]
		public void TestGetTilesetNames()
		{
			var found = five_mixed_tsets.ListTilesetNames();
			Assert.AreEqual(5, found.Count);
			Assert.AreEqual("16 pastels (32x32)", found[0]);
			Assert.AreEqual("Stars (8x16)", found[4]);
		}
	}
}