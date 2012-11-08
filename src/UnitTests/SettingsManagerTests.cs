// Copyright 2005-2012 Moonfire Games
// Released under the MIT license
// http://mfgames.com/mfgames-cil/license

#region Namespaces

using System.IO;
using System.Xml.Serialization;
using MfGames.HierarchicalPaths;
using MfGames.Settings;
using MfGames.Settings.Enumerations;
using NUnit.Framework;

#endregion

namespace UnitTests
{
	/// <summary>
	/// Tests various functionality of the <see cref="SettingsManager"/>.
	/// </summary>
	[TestFixture]
	public class SettingsManagerTests
	{
		#region Methods

		/// <summary>
		/// Tests adding one to the manager.
		/// </summary>
		[Test]
		public void AddOneToManager()
		{
			// Setup
			var settingsManager = new SettingsManager();

			// Operation
			settingsManager.Set(
				new HierarchicalPath("/a"),
				new SettingsA1(
					2,
					"two"));

			// Verification
			Assert.AreEqual(
				1,
				settingsManager.Count);
		}

		/// <summary>
		/// Tests adding one to the manager then flushing it.
		/// </summary>
		[Test]
		public void AddOneToManagerAndFlush()
		{
			// Setup
			var settingsManager = new SettingsManager();

			// Operation
			settingsManager.Set(
				new HierarchicalPath("/a"),
				new SettingsA1(
					3,
					"three"));
			settingsManager.Flush();

			// Verification
			Assert.AreEqual(
				1,
				settingsManager.Count);
		}

		/// <summary>
		/// Saves a setting A and loads it as B, but without mapping.
		/// </summary>
		[Test]
		public void ConvertFromAtoB()
		{
			// Setup
			var settingsManager = new SettingsManager();
			settingsManager.Set(
				new HierarchicalPath("/a"),
				new SettingsA1(
					1,
					"one"));

			// Operation
			var b = settingsManager.Get<SettingsA2>(
				"/a",
				SettingSearchOptions.SerializeDeserializeMapping);

			// Verification
			Assert.AreEqual(
				1,
				settingsManager.Count);
			Assert.AreEqual(
				1,
				b.A);
			Assert.AreEqual(
				"one",
				b.B);
		}

		/// <summary>
		/// Tests the initial state of an empty manager.
		/// </summary>
		[Test]
		public void EmptyManager()
		{
			// Setup

			// Operation
			var settingsManager = new SettingsManager();

			// Verification
			Assert.AreEqual(
				0,
				settingsManager.Count);
		}

		/// <summary>
		/// Tests serializing, then deserializing an empty manager.
		/// </summary>
		[Test]
		public void SerializeEmptyManager()
		{
			// Setup
			var settingsManager = new SettingsManager();

			// Operation
			var writer = new StringWriter();
			settingsManager.Save(writer);

			var reader = new StringReader(writer.ToString());
			settingsManager = new SettingsManager();
			settingsManager.Load(reader);

			// Verification
			Assert.AreEqual(
				0,
				settingsManager.Count);
		}

		#endregion

		#region Nested Type: SettingsA1

		[XmlRoot("SettingsA")]
		public class SettingsA1
		{
			#region Constructors

			public SettingsA1()
			{
			}

			public SettingsA1(
				int a,
				string b)
			{
				A = a;
				B = b;
			}

			#endregion

			#region Fields

			public int A;
			public string B;

			#endregion
		}

		#endregion

		#region Nested Type: SettingsA2

		[XmlRoot("SettingsA")]
		public class SettingsA2
		{
			#region Constructors

			public SettingsA2()
			{
				A = -123;
				B = "uninitialized";
			}

			public SettingsA2(
				int a,
				string b)
			{
				A = a;
				B = b;
			}

			#endregion

			#region Fields

			public int A;
			public string B;

			#endregion
		}

		#endregion

		#region Nested Type: SettingsC

		public class SettingsC
		{
		}

		#endregion
	}
}
