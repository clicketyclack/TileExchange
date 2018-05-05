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
using System.Collections.Generic;
using TileExchange.TileSetTypes;
using TileExchange.TesselatedImages;
using TileExchange.Fragment;

namespace TileExchange
{
	public interface IExchangeEngine
	{

	}

	public class BasicExchangeEngine : IExchangeEngine
	{
		private IHueMatchingTileset ts;
		private ITesselatedImage input_image;


		public BasicExchangeEngine(IHueMatchingTileset ts, ITesselatedImage input_image)
		{
			this.ts = ts;
			this.input_image = input_image;
		}

		public void run(int iterations = 20000)
		{
			CheckSetFallback();
			SingleRandomizedIteration(iterations);

		}

		/// <summary>
		/// Traverse all fragments and check if any are missing a replacement fragment. Fallback to a 
		/// valid (but probably wildly non-matching) tile from the new tileset.
		/// </summary>
		private void CheckSetFallback()
		{
			var fallback = ts.Tile(0);
			var fragments = input_image.GetImageFragments();
			foreach (var fragment in fragments)
			{
				if (fragment.GetReplacementFragment() is null ||
					fragment.GetReplacementFragment() == fragment.GetOriginalFragment())
				{
					fragment.SetReplacementFragment(fallback);
				}
			}
		}



		private void ConsiderReplacements(IImageFragment fragment, List<IFragment> candidates)
		{
			var orig_frag = fragment.GetOriginalFragment();
			var orig_avg = orig_frag.AverageColor();
			var orig_hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(orig_avg);

			var curr_frag = fragment.GetReplacementFragment();
			var curr_avg = curr_frag.AverageColor();
			var curr_hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(curr_avg);

			var current_distance = ExchangeEngine.ColorDistances.WeightedDistance(curr_hsl, orig_hsl);

			foreach (var cand in candidates)
			{
				var cand_avg = cand.AverageColor();
				var cand_hsl = ImageProcessor.Imaging.Colors.HslaColor.FromColor(cand_avg);
				var cand_distance = ExchangeEngine.ColorDistances.WeightedDistance(cand_hsl, orig_hsl);

				if (cand_distance < current_distance)
				{
					curr_frag = cand;
					curr_avg = cand_avg;
					curr_hsl = cand_hsl;

					current_distance = cand_distance;
					fragment.SetReplacementFragment(cand);
				}
			}
		}

		/// <summary>
		/// Run one randomized iteration.
		/// </summary>
		private void SingleRandomizedIteration(int SubSelectionCount)
		{
			var fragments = input_image.GetImageFragments();

			var candidates = ts.DrawN(SubSelectionCount);
			Console.WriteLine("Single iteration. Fragments to process : {0}, Candidates to evaluate : {1} .", fragments.Count, candidates.Count);

			foreach (var fragment in fragments)
			{
				ConsiderReplacements(fragment, candidates);
			}
		}
	}
}
