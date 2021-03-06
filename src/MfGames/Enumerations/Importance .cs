﻿// Copyright 2005-2012 Moonfire Games
// Released under the MIT license
// http://mfgames.com/mfgames-cil/license

namespace MfGames.Enumerations
{
	/// <summary>
	/// An enum that has common levels of importantance for purposes of weighting. Each
	/// level is roughly twice or half the value below or above it (respectively).
	/// </summary>
	public enum Importance
	{
		/// <summary>
		/// The lowest level of importance.
		/// </summary>
		Lowest = -8,

		/// <summary>
		/// Indicates an importance  that is below average.
		/// </summary>
		Lower = -4,

		/// <summary>
		/// Indicates an importance that is slightly below average.
		/// </summary>
		Low = -2,

		/// <summary>
		/// Indicates an average or normal importance.
		/// </summary>
		Normal = 0,

		/// <summary>
		/// Indicates an importance higher than average.
		/// </summary>
		High = 2,

		/// <summary>
		/// Indicates a higher importance than High.
		/// </summary>
		Higher = 4,

		/// <summary>
		/// Indicates the highest lvel of normal importance.
		/// </summary>
		Highest = 8,
	}
}
