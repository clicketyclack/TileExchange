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
using TileExchange.TileSet;
using TileExchange.TesselatedImages;

namespace TileExchange
{
	public interface IExchangeEngine
	{

	}

	public class BasicExchangeEngine : IExchangeEngine
	{
		private ITileSet ts;
		private ITesselatedImage input_image;
		public BasicExchangeEngine(ITileSet ts, ITesselatedImage input_image)
		{
			this.ts = ts;
			this.input_image = input_image;
		}

		public void run()
		{
			var fragments = input_image.GetImageFragments();

			foreach (var fragment in fragments)
			{
				var average_color = fragment.GetOriginalFragment().AverageColor();
				var average_hue = ImageProcessor.Imaging.Colors.HslaColor.FromColor(average_color).H;
				var tolerance = 0.01f;
				var candidates = ts.TilesByHue(average_hue, tolerance);

				while (tolerance < 0.5 && candidates.Count == 0)
				{
					tolerance += 0.05f;
					candidates = ts.TilesByHue(average_hue, tolerance);

				}

				Console.WriteLine("For fragment with average hue {0}, tolerance {1} gave {2} candidates.", average_hue, tolerance, candidates.Count);

				if (candidates.Count > 0)
				{
					fragment.SetReplacementFragment(candidates[0]);
				}
			}
		}
	}
}
