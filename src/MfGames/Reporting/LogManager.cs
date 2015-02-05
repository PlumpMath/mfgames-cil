// <copyright file="LogManager.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System;

namespace MfGames.Reporting
{
	/// <summary>
	/// Implements a low-profile logging sender for the MfGames and related libraries. To
	/// connect to the logging interface, the code just needs to attach to the Log event.
	/// </summary>
	public static class LogManager
	{
		#region Public Events

		/// <summary>
		/// Occurs when a message is logged. The sender can be <see langword="null"/>
		/// with events, if the logging interface does not give a context.
		/// </summary>
		public static event EventHandler<SeverityMessageEventArgs> Logged
		{
			add { logged += value; }
			remove { logged -= value; }
		}

		#endregion

		#region Events

		/// <summary>
		/// </summary>
		private static event EventHandler<SeverityMessageEventArgs> logged;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Logs the message to any class listening to the manager.
		/// </summary>
		/// <param name="message">
		/// The message.
		/// </param>
		public static void Log(SeverityMessage message)
		{
			Log(
				null,
				message);
		}

		/// <summary>
		/// Logs the specified sender.
		/// </summary>
		/// <param name="sender">
		/// The sender.
		/// </param>
		/// <param name="message">
		/// The message.
		/// </param>
		public static void Log(
			object sender,
			SeverityMessage message)
		{
			// Check the listeners outside of a lock. If we have anything, then
			// return as fast as we can.
			if (logged == null)
			{
				return;
			}

			// Get the listeners at the point of reading the log.
			EventHandler<SeverityMessageEventArgs> listeners = logged;

			// If we don't have listeners, then just break out.
			if (listeners == null)
			{
				return;
			}

			// Create an event handler and invoke it.
			var args = new SeverityMessageEventArgs(message);
			listeners(
				sender,
				args);
		}

		#endregion
	}
}
