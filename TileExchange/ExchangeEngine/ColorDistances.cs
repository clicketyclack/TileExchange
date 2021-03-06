﻿/* 
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
using ImageProcessor.Imaging.Colors;

namespace TileExchange.ExchangeEngine
{
	public class ColorDistances
	{

		/// <summary>
		/// Simples color difference, returns higher numbers for higher difference in hue.
		/// </summary>
		/// <returns>Some sort of distance metric. 0.0 = identical, 1.0 = completely different.</returns>
		/// <param name="apple">One color.</param>
		/// <param name="orange">Another color.</param>
		public static float SimpleHue(HslaColor apple, HslaColor orange)
		{
			var toreturn = HueDifference(apple.H, orange.H);
			return toreturn;
		}

		/// <summary>
		/// Calulate the difference in hue.
		/// </summary>
		/// <returns>The calculated difference.</returns>
		/// <param name="apple">One hue.</param>
		/// <param name="orange">The other hue.</param>
		public static float HueDifference(float apple, float orange) {

			var toreturn = apple - orange;

			if (toreturn > 0.5)
			{
				// Too big difference with apple largest, ( 0.0 , orange, .... apple, 1.0)
				// Apple must be in range (0.5, 1.0), so add 0.5 then wrap.
				// Orange must be in range (0.0, 0.5), so add 0.5.
				// Now Orange is larger.
				var new_a = apple + 0.5f - 1.0f;
				var new_o = orange + 0.5f;
				toreturn = new_o - new_a;

			}
			else if (toreturn < -0.5)
			{
				// Too big difference with orange largest, ( 0.0 , apple, .... orange, 1.0)
				// See above.
				var new_o = orange + 0.5f - 1.0f;
				var new_a = apple + 0.5f;
				toreturn = new_a - new_o;
			}

			toreturn *= 2.0f;
			toreturn *= toreturn;

			return toreturn;
		}

		/// <summary>
		/// Calculate a difference measurement between two colors. Weight H, S and L.
		/// </summary>
		/// <returns>The distance.</returns>
		/// <param name="apple">Apple.</param>
		/// <param name="orange">Orange.</param>
		public static float WeightedDistance(HslaColor apple, HslaColor orange) {
			
			var diff_h = HueDifference(apple.H, orange.H);
			var diff_s = apple.S - orange.S;
			diff_s *= diff_s;

			var diff_l = apple.L - orange.L;
			diff_l *= diff_l;

			var distance = 0.5f * diff_h + 0.15f * diff_s + 0.35f * diff_l;

			return distance;
		}
	}
}
