// Copyright 2005-2012 Moonfire Games
// Released under the MIT license
// http://mfgames.com/mfgames-cil/license

using System;

namespace MfGames.Settings.Enumerations
{
	/// <summary>
	/// Defines the various options for searching values in settings.
	/// </summary>
	[Flags]
	public enum SettingSearchOptions
	{
		/// <summary>
		/// Indicates no options which means if the key doesn't exist, then
		/// return the default.
		/// </summary>
		None = 0,

		/// <summary>
		/// Search the parents of the HierarchicalPath if the item can't be found.
		/// </summary>
		SearchHierarchicalParents = 1,

		/// <summary>
		/// Search any parent settings when the item can't be found.
		/// </summary>
		SearchParentSettings = 2,

		/// <summary>
		/// If set, it searches the parent settings first. Otherwise, the hierarchical
		/// parents will be searched first.
		/// </summary>
		ParentSettingsFirst = 4,

		/// <summary>
		/// If set, the various operations will attempt to serialize and
		/// deserialize the objects if they can't be mapped directly.
		/// </summary>
		SerializeDeserializeMapping = 8,
	}
}
