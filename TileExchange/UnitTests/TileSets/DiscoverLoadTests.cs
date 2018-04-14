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

namespace TileExchange
{
	[TestFixture]
	public class DiscoverLoadTests
	{
		/// <summary>
		/// Check number of tilesets  in a directory.
		/// </summary>
		[Test]
		public void DiscoverTileSets()
		{
			var tsf = new TileSetFinder();
			Assert.AreEqual(5, tsf.NumberOfTilesets());

			var t1 = tsf.TileSet(0).Tile(0);
			Assert.AreEqual(t1.GetSize().Width, 16);
		}

		/// <summary>
		/// Verify tile count in sets.
		/// </summary>
		[Test]
		public void TileSetContent()
		{
			var tsf = new TileSetFinder();
			var ts0 = tsf[0];
			var ts1 = tsf[1];
			var ts2 = tsf[2];
			var ts3 = tsf[3];

			int[] v = { ts0.NumberOfTiles(), ts1.NumberOfTiles(), ts2.NumberOfTiles(), ts3.NumberOfTiles()};
			Array.Sort(v);

			Assert.AreEqual(4*4, v[0]);
			Assert.AreEqual(256, v[1]);
			Assert.AreEqual(512, v[2]);
			Assert.AreEqual(1024, v[3]);
		}

	
	}
}