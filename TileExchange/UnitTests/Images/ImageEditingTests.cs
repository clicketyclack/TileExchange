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

namespace TileExchange
{
	public class ImageEditingTests
	{
		public ImageEditingTests()
		{
		}

		/// <summary>
		/// Run POC method. Will fail if png read/write starts causing issues.
		/// 
		/// If your tests fail here, check that "assets" and "output" paths exist.
		/// </summary>
		[Test]
		public void AlterImagePOC()
		{
			var poc = new MyClass();
			poc.AlterImagePOC();
		}
	}
}