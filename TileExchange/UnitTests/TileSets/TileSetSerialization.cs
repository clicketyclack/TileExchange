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
using System.IO;
using System.Reflection;
using System.Drawing;

using TileExchange.TileSetTypes;
namespace TileExchange.UnitTests.TileSets

{

	/// <summary>
	/// This fixture only verifies that project imports and dependencies work.
	/// Created due to some issues with dependencies not matching my .net target profile.
	/// </summary>
	[TestFixture]
	public class TileSetSerialization
	{
		public TileSetSerialization()
		{
		}


		[SetUp]
		public void Initialize() { 
		
		
		}

		/// <summary>
		/// Verify that user settings can be constructed and serialized.
		/// </summary>
		[Test]
		public void ProceduralHSVFromScratch()
		{
			var packname = "scratch";
			var twidth = 100;
			var theight = 300;
			var hues1 = new float[] { 0.1f, 0.84f };
			var hues2 = new float[] { 0.35f, 0.65f };

			var saturation = 0.4f;
			var luminosity = 0.8f;

			var phsv1 = new ProceduralHSVTileSet(packname + "1", twidth, theight, hues1, saturation, luminosity);
			var phsv2 = new ProceduralHSVTileSet(packname + "2", twidth+1, theight+2, hues2, saturation + 0.11f, luminosity + 0.12f);


			var serialized1 = phsv1.Serialize();
			var serialized2 = phsv2.Serialize();

			System.Console.Write(serialized1);
			Assert.IsTrue(serialized1.Contains(@"packname"":""scratch1"));
			Assert.IsTrue(serialized2.Contains(@"packname"":""scratch2"));

			Assert.IsTrue(serialized1.Contains(@"twidth"":100"));
			Assert.IsTrue(serialized2.Contains(@"twidth"":101"));

			Assert.IsTrue(serialized2.Contains(@"theight"":302"));

			Assert.IsTrue(serialized1.Contains(@"saturation"":0.4"));
			Assert.IsTrue(serialized2.Contains(@"luminosity"":0.92"));

			Assert.IsTrue(serialized1.Contains(@"""hues"":[0."));


		}

		/// <summary>
		/// Verify that user settings can be constructed and serialized.
		/// </summary>
		[Test]
		public void ProceduralHSVDeserialization()
		{
			String hsv_string = @"{""twidth"":333,""theight"":300,""hues"":[0.9, 0.99],""saturation"":0.55,""luminosity"":0.123,""packname"":""from_hsv_string""}";
			var hsv = ProceduralHSVTileSet.DeSerialize(hsv_string);
			Assert.AreEqual(hsv.twidth, 333);
			Assert.AreEqual(hsv.theight, 300);
			Assert.AreEqual(hsv.hues[1], 0.99f);

			Assert.AreEqual(hsv.saturation, 0.55f);
			Assert.AreEqual(hsv.luminosity, 0.123f);
			Assert.AreEqual(hsv.packname, "from_hsv_string");

		}




		/// <summary>
		/// Verify that a chopped bitmap can be constructed and serialized.
		/// </summary>
		[Test]
		public void ChoppedBitmapTileSetFromScratch()
		{
			var bmts1 = new ChoppedBitmapTileSet("stars.png", "stars_pack", 250, 550);
			var bmts2 = ChoppedBitmapTileSet.Default();

			var serialized1 = bmts1.Serialize();
			var serialized2 = bmts2.Serialize();


			Console.Write(serialized1);
			Console.Write(serialized2);

			Assert.IsTrue(serialized1.Contains(@"""twidth"":250"));
			Assert.IsTrue(serialized1.Contains(@"""bitmap_fname"":""stars.png"""));
			Assert.IsTrue(serialized1.Contains(@"""packname"":""stars_pack"""));
			Assert.IsTrue(serialized1.Contains(@"""theight"":550"));

			Assert.IsTrue(serialized2.Contains(@"""twidth"":"));
			Assert.IsTrue(serialized2.Contains(@"""bitmap_fname"":"""));
			Assert.IsTrue(serialized2.Contains(@"""packname"":"""));
			Assert.IsTrue(serialized2.Contains(@"""theight"":"));

		}




		/// <summary>
		/// Verify that a ChoppedBitmapTileSet can be de-serialized.
		/// </summary>
		[Test]
		public void ChoppedBitmapTileSetDeserialization()
		{
			var bmp_string1 = @"{ ""bitmap_fname"":""a.png"",""twidth"":251,""theight"":551,""packname"":""somepack""}";
			var bmp_string2 = @"{ ""bitmap_fname"":""b.png"",""twidth"":17,""theight"":18,""packname"":""hello_some_pack""}";

			var bmts1 = ChoppedBitmapTileSet.DeSerialize(bmp_string1);
			var bmts2 = ChoppedBitmapTileSet.DeSerialize(bmp_string2);

			Assert.AreEqual(bmts1.bitmap_fname, "a.png");
			Assert.AreEqual(bmts1.twidth, 251);
			Assert.AreEqual(bmts2.theight, 18);
			Assert.AreEqual(bmts2.packname, "hello_some_pack");

		}
	}
}


