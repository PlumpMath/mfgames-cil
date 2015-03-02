// <copyright file="VariableMacroExpansionSegment.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;
using System.Text.RegularExpressions;

namespace MfGames.Text
{
	/// <summary>
	/// Contains a variable segment for a macro.
	/// </summary>
	public class VariableMacroExpansionSegment : IMacroExpansionSegment
	{
		#region Constructors and Destructors

		static VariableMacroExpansionSegment()
		{
			StringSpecifierRegex = new Regex(@"^(\d+|\d+,(?:\d+)?|,\d+)$");
		}

		public VariableMacroExpansionSegment(string format, int macroIndex)
		{
			// Save the simple variables.
			MacroIndex = macroIndex;

			// Split out the parts.
			string[] parts = format.Split(new[] { ':' }, 2);

			Field = parts[0];

			if (parts.Length > 1 && !string.IsNullOrWhiteSpace(parts[1]))
			{
				Format = parts[1];
			}
		}

		#endregion

		#region Public Properties

		public static Regex StringSpecifierRegex { get; private set; }
		public string Field { get; private set; }
		public string Format { get; private set; }
		public int MacroIndex { get; private set; }

		#endregion

		#region Public Methods and Operators

		public static string Expand(
			string field,
			string format,
			IMacroExpansionContext context)
		{
			// Pull out the object from the macros.
			object value = context.Values[field];

			// See if we have a format, if we do, try to parse it.
			if (format != null)
			{
				int intValue;

				if (Int32.TryParse(value.ToString(), out intValue))
				{
					value = intValue.ToString(format);
				}
			}

			// Return the results.
			return Convert.ToString(value);
		}

		public static string GetRegex(string format)
		{
			// If we don't have a format, blow up.
			if (format == null)
			{
				throw new InvalidOperationException(
					"Cannot use GetRegex() without a format in all variables.");
			}

			// Look for simple formats.
			string pattern = null;

			if (format.Length > 0)
			{
				char first = format[0];
				string specifier = format.Substring(1);
				int precision;
				bool hasPrecision = Int32.TryParse(specifier, out precision);
				switch (first)
				{
					case 'D':
						if (hasPrecision)
						{
							pattern = "";

							for (var i = 0; i < precision; i++)
							{
								pattern += "\\d";
							}
						}
						break;

					case 'G':
						pattern = "\\d+";
						break;

					case 'S':
						pattern = GetStringPattern(specifier);
						break;
				}
			}

			// If we haven't figured it out, throw up.
			if (pattern == null)
			{
				throw new InvalidOperationException(
					"Cannot create RegeEx from format: " + format + ".");
			}

			// Return the pattern.
			return string.Format("({0})", pattern);
		}

		public string Expand(IMacroExpansionContext context)
		{
			return Expand(Field, Format, context);
		}

		public string GetRegex()
		{
			return GetRegex(Format);
		}

		public void Match(IMacroExpansionContext context, Match match)
		{
			context.Values[Field] = match.Groups[MacroIndex].Value;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Implements a macro extension to the format strings. This can be
		/// any of: S#, S#-#, or S-#. Where "S" has been removed before calling
		/// this and "#" is one or more numbers.
		/// </summary>
		/// <param name="specifier"></param>
		/// <returns></returns>
		private static string GetStringPattern(string specifier)
		{
			// See if the pattern matches.
			Match match = StringSpecifierRegex.Match(specifier);

			if (!match.Success)
			{
				throw new InvalidOperationException(
					"Cannot parse string specifier 'S" + specifier + "'.");
			}

			// If the first value is non-blank, it is a simple value.
			string length = match.Groups[1].Value;
			string singlePattern = "\\w{" + length + "}";
			return singlePattern;
		}

		#endregion
	}
}