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
	[TestFixture]
	public class DiscoverLoadTests
	{
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
		/// Verify tile count in sets.
		/// </summary>
		[Test]
		public void TileSetContent()
		{
			var tsr = new TileSetRepo.TileSetRepo();
			tsr.DiscoverBitmaps();
			var ts0 = tsr[0];
			var ts1 = tsr[1];
			var ts2 = tsr[2];
			var ts3 = tsr[3];

			int[] v = { ts0.NumberOfTiles(), ts1.NumberOfTiles(), ts2.NumberOfTiles(), ts3.NumberOfTiles() };
			Array.Sort(v);

			Assert.AreEqual(4 * 4, v[0]);
			Assert.AreEqual(256, v[1]);
			Assert.AreEqual(512, v[2]);
			Assert.AreEqual(1024, v[3]);
		}

	}
}