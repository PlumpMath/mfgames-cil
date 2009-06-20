﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MfGames.Utility;

namespace MfGames.Extensions
{
	/// <summary>
	/// Extension methods for System.Object.
	/// </summary>
	public static class SystemObjectExtensions
	{
		/// <summary>
		/// Changes the type of the given object to another type, using the ExtendedConvert.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static object ChangeType(this object value, Type type)
		{
			return ExtendedConvert.ChangeType(value, type);
		}

		/// <summary>
		/// Changes the type of the given object to another type, using the ExtendedConvert.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ChangeType<T>(this object value)
		{
			return (T) ExtendedConvert.ChangeType(value, typeof(T));
		}
	}
}
