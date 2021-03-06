// Copyright 2005-2012 Moonfire Games
// Released under the MIT license
// http://mfgames.com/mfgames-cil/license

using System.IO;

namespace MfGames.Extensions.System.IO
{
	/// <summary>
	/// Defines various extensions to System.IO.DirectoryInfo.
	/// </summary>
	public static class SystemIODirectoryInfoExtensions
	{
		#region Methods

		/// <summary>
		/// Gets the directory info from the directory.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="dirname">The dirname.</param>
		/// <returns></returns>
		public static DirectoryInfo GetDirectoryInfo(
			this DirectoryInfo directory,
			string dirname)
		{
			return new DirectoryInfo(Path.Combine(directory.FullName, dirname));
		}

		/// <summary>
		/// Gets a child FileInfo object from the directory.
		/// </summary>
		/// <param name="directory">The directory.</param>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
		public static FileInfo GetFileInfo(
			this DirectoryInfo directory,
			string filename)
		{
			return new FileInfo(Path.Combine(directory.FullName, filename));
		}

		#endregion
	}
}
