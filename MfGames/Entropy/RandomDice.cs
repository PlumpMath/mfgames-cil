#region Copyright and License

// Copyright (c) 2005-2009, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;

#endregion

namespace MfGames.Entropy
{
	public class RandomDice : IDice
	{
		private readonly int count;
		private readonly int sides;

		public RandomDice(int count, int sides)
		{
			this.count = count;
			this.sides = sides;
		}

		#region IDice Members

		/// <summary>
		/// This simple property rolls a number of dice equal to the
		/// count, with each die being 1 to sides. The total is returned
		/// as the result.
		/// </summary>
		public int Roll(Random random)
		{
			int total = 0;

			for (int i = 0; i < count; i++)
				total += random.Next(1, sides);

			return total;
		}

		#endregion

		public override string ToString()
		{
			return String.Format("{0}d{1}", count, sides);
		}
	}
}