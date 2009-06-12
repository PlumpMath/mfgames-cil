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

using MfGames.Utility;

using NUnit.Framework;

#endregion

namespace UnitTests
{
	/// <summary>
	/// Test the time zones.
	/// </summary>
	[TestFixture]
	public class TestTimeZones
	{
		[Test]
		public void TestSlashUSCentral()
		{
			TimeZone tz = TimeZones.ToTimeZone("US/Central");
			Assert.AreEqual("-06:00:00",
			                tz.GetUtcOffset(new DateTime(2005, 1, 1)).ToString());
		}

		[Test]
		public void TestSlashUSCentralCDT()
		{
			TimeZone tz = TimeZones.ToTimeZone("US/Central");
			Assert.AreEqual("CDT", tz.DaylightName);
		}

		[Test]
		public void TestSlashUSCentralCST()
		{
			TimeZone tz = TimeZones.ToTimeZone("US/Central");
			Assert.AreEqual("CST", tz.StandardName);
		}
	}
}