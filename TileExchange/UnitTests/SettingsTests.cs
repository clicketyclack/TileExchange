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
using System.IO;
using System.Reflection;
using System.Drawing;

using TileExchange.ExchangeEngine;

namespace TileExchange
{

	/// <summary>
	/// This fixture only verifies that project imports and dependencies work.
	/// Created due to some issues with dependencies not matching my .net target profile.
	/// </summary>
	[TestFixture]
	public class SettingsTests
	{
		public SettingsTests()
		{
		}
		/// <summary>
		/// Verify that user settings can be constructed and serialized.
		/// </summary>
		[Test]
		public void UserSettingsFromScratch()
		{
			var us = new UserSettings();
			var serialized = us.serialize();
			System.Console.Write(serialized);
			Assert.IsTrue(serialized.Contains("\"last_directory\""));
			Assert.IsTrue(serialized.Contains("\"serialized_tilesets\""));
		}

		/// <summary>
		/// Verify that user settings can be constructed and serialized.
		/// </summary>
		[Test]
		public void MinimalDeserialization()
		{
			String minimal = @"{""last_directory"":""/home/clickety""}";
			var us = UserSettings.deserialize(minimal);
			Assert.AreEqual(us.last_directory, "/home/clickety");
		}

	}


}
