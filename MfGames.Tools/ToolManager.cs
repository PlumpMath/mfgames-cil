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
using System.Collections.Generic;
using System.Reflection;

using MfGames.Logging;

#endregion

namespace MfGames.Tools
{
	/// <summary>
	/// The primary management class for the entire tool
	/// system. Most tools actually extend this class, calling
	/// Process() from their Main() function. This handles the
	/// processing of the command-line elements, pulling up the tool,
	/// or starting up the GUI interface if no arguments can be found
	/// and options are missing.
	/// </summary>
	public class ToolManager
	{
		/// <summary>
		/// Sets up logging and prepares to use the sysetm.
		/// </summary>
		public bool Process(string[] argsArray)
		{
			// Convert the entire string list into a linked list. We
			// we process the elements, we'll remove them from the
			// list until we only have the positional parameters
			// remaining.
			var args = new List<string>();
			args.AddRange(argsArray);

			// Process for ourselves, this handles the -V and --help
			// stuff. This doesn't handle the actual command elements.
			Process(args, this);

			// If we have no registered tools, we are done since the
			// only thing we can do is populate variables
			if (tools.Count == 0)
			{
				Log.Fatal("Stopped processing with no registered tools");
				return false;
			}

			// Check for the length
			if (args.Count == 0)
			{
				// We need to show the graphical display or a usage
				// screen depending if we can load Gtk#, SWF, or not.
				Log.Fatal("Cannot show usage screen!");
				return false;
			}

			// We have at least one command, so pop it off
			string commandName = args[0];
			args.RemoveAt(0);

			if (!tools.ContainsKey(commandName))
			{
				Log.Fatal("There is no registered tool: {0}", commandName);
				return false;
			}

			// We have the tool, so populate it
			ITool tool = tools[commandName];
			Log.Info("Processing command: {0}", commandName);

			try
			{
				Process(args, tool);
			}
			catch (Exception e)
			{
				Log.Fatal(e.Message);
				Log.Debug(e.StackTrace);

				while (e.InnerException != null)
				{
					e = e.InnerException;
					Log.Fatal("+ {0}", e.Message);
					Log.Debug("+ {0}", e.StackTrace);
				}

				WriteUsage();
				return false;
			}

			// We have all the properties assigned, so then process
			// the tool.
			tool.Process(args.ToArray());

			// We ran properly
			return true;
		}

		#region Arguments

		/// <summary>
		/// Processes through the container and pulls the various
		/// arguments and positional elements and sets them in the
		/// public values.
		/// </summary>
		public void Process(List<string> args, object container)
		{
			// If we have no arguments, then just return
			if (args.Count == 0)
			{
				CheckPositional(-1, container);
				return;
			}

			// Create the arguments which handles most of the work
			var pa = new ParsingArgs(args, container);

			// Build up a list of unprocessed elements
			var unprocessed = new List<string>();

			// Go through the elements and start to process it one at
			// a time
			while (pa.First != null)
			{
				// Try to process it. If this returns false, then it
				// is unparsable and should just be added. If it
				// returns true, then the needed changes have already
				// been performed.
				if (!pa.Process())
				{
					unprocessed.Add(pa.Advance());
				}
			}

			// Set the unprocessed elements as the new one
			args.Clear();
			args.AddRange(unprocessed);

			// Perform the sanity checking for positional properties
			CheckPositional(pa.LastPosition, container);
		}

		#endregion

		#region Checking and Validation

		/// <summary>
		/// Ensures that all the non-optional positional properties
		/// are set from the parsing arguments.
		/// </summary>
		private void CheckPositional(int lastPosition, object container)
		{
			// Go through all the variables and look for positional
			bool isMissing = false;
			Type cType = container.GetType();
			MemberInfo[] mis = cType.FindMembers(
				MemberTypes.Field | MemberTypes.Property,
				BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
				BindingFlags.Static,
				null,
				null);

			foreach (MemberInfo mi in mis)
			{
				// Go through each of the attributes
				foreach (Attribute a in mi.GetCustomAttributes(true))
				{
					// Ignore if we aren't a positional
					if (!(a is PositionalAttribute))
						continue;

					// See if the positional is right
					var ppa = (PositionalAttribute) a;

					// Check the value
					if (lastPosition < ppa.Index && !ppa.IsOptional)
					{
						// This is a non-optional positional and it
						// wasn't set
						Log.Error("Required parameter #{0} was not set", ppa.Index);
						isMissing = true;
					}
				}
			}

			// See if we have problems
			if (isMissing)
				throw new Exception("Required command-line " + "arguments are missing");
		}

		#endregion

		#region Usage

		/// <summary>
		/// This writes out the usage command to the console.
		/// </summary>
		public void WriteUsage()
		{
			Console.WriteLine("USAGE:");
		}

		#endregion

		#region Tools

		private readonly Dictionary<string, ITool> tools =
			new Dictionary<string, ITool>();

		/// <summary>
		/// Registers a single tool and its keys.
		/// </summary>
		protected void RegisterTool(ITool tool)
		{
			Log.Debug("Registering tool: {0}", tool.ToolName);
			tools[tool.ToolName] = tool;
		}

		/// <summary>
		/// Registers the callbacks into the system by allowing the
		/// extending class to RegisterTool(). If this method is not
		/// extended, or it is called from the extending class, it
		/// uses reflection on the defining type to populate all the
		/// tools within the assembly.
		/// </summary>
		public virtual void RegisterTools()
		{
			RegisterTools(GetType().Assembly);
		}

		/// <summary>
		/// Registers all non-abstract classes that extend ITool from
		/// the given type.
		/// </summary>
		public void RegisterTools(Assembly assembly)
		{
			// Get all the types of this class
			foreach (Type type in assembly.GetTypes())
			{
				// We ignore anything not a class or abstract or
				// static as ones we simply can't use.
				if (!type.IsClass || type.IsAbstract)
					continue;

				// Make sure we extend the ITool interface
				if (!typeof(ITool).IsAssignableFrom(type))
					continue;

				// Create a new instance of the tool
				var it = (ITool) Activator.CreateInstance(type);

				// Register it
				RegisterTool(it);
			}
		}

		#endregion Tools

		#region Logging

		private Log log;

		/// <summary>
		/// Contains the logging context for this tool manager.
		/// </summary>
		public Log Log
		{
			get
			{
				if (log == null)
					log = new Log(GetType());

				return log;
			}
		}

		#endregion
	}
}