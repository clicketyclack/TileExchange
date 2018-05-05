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
using NUnit.Framework;
using ImageProcessor.Imaging.Colors;
using TileExchange.ExchangeEngine;

namespace TileExchange.UnitTests.ExchangeEngine
{

	/// <summary>
	/// Verifies color distances
	/// </summary>
	[TestFixture]
	public class TestColorDistanceCalculations
	{

		/// <summary>
		/// Verify simple HSL calculations.
		/// </summary>
		[Test]
		public void BasicExchangeEngineTest()
		{

			// Various H colors.
			var c0 = HslaColor.FromHslaColor(0.0f, 0.5f, 0.5f);
			var c1 = HslaColor.FromHslaColor(0.1f, 0.5f, 0.5f);
			var c3 = HslaColor.FromHslaColor(0.3f, 0.5f, 0.5f);
			var c4 = HslaColor.FromHslaColor(0.4f, 0.5f, 0.5f);
			var c5 = HslaColor.FromHslaColor(0.5f, 0.5f, 0.5f);
			var c9 = HslaColor.FromHslaColor(0.9f, 0.5f, 0.5f);
			var cA = HslaColor.FromHslaColor(1.0f, 0.5f, 0.5f);

			// Fudged S or L
			var c1a = HslaColor.FromHslaColor(0.1f, 0.4f, 0.5f);
			var c1b = HslaColor.FromHslaColor(0.1f, 0.5f, 0.4f);
			var c1c = HslaColor.FromHslaColor(0.1f, 0.6f, 0.5f);
			var c1d = HslaColor.FromHslaColor(0.1f, 0.5f, 0.6f);


			// Argument order irrelevant
			Assert.AreEqual(ColorDistances.SimpleHue(c1, c3), ColorDistances.SimpleHue(c3, c1), 0.00001);
			Assert.AreEqual(ColorDistances.SimpleHue(c1, c4), ColorDistances.SimpleHue(c4, c1), 0.00001);


			// Color is always 0.0 away from itself.
			Assert.AreEqual(0.0f, ColorDistances.SimpleHue(c1, c1), 0.00001);
			Assert.AreEqual(0.0f, ColorDistances.SimpleHue(c4, c4), 0.00001);
			Assert.AreEqual(0.0f, ColorDistances.SimpleHue(c9, c9), 0.00001);


			// Distance same if wrapping around 1.0/0.0.
			Assert.AreEqual(ColorDistances.SimpleHue(c9, c1), ColorDistances.SimpleHue(c1, c3), 0.00001);

			// Max distance (1.0) at 180 from each other.
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c4, c9), 0.00001);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c9, c4), 0.00001);

			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c5, c0), 0.00001);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c5, cA), 0.00001);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c0, c5), 0.00001);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(cA, c5), 0.00001);

			var c25 = HslaColor.FromHslaColor(0.25f, 0.5f, 0.5f);
			var c75 = HslaColor.FromHslaColor(0.75f, 0.5f, 0.5f);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c25, c75), 0.00001);
			Assert.AreEqual(1.0f, ColorDistances.SimpleHue(c75, c25), 0.00001);

		}
	}
}
