﻿using System.Threading.Tasks;
using Tgstation.Server.Host.Watchdog;

namespace Tgstation.Server.Host.Console
{
	/// <summary>
	/// Contains the entrypoint for the application
	/// </summary>
	static class Program
	{
		/// <summary>
		/// The <see cref="IServerFactory"/> for the <see cref="Program"/>
		/// </summary>
		internal static IWatchdogFactory WatchdogFactory { get; set; } = new WatchdogFactory();

        /// <summary>
        /// Entrypoint for the application
        /// </summary>
        /// <param name="args">The arguments for the <see cref="Program"/></param>
        /// <returns>A <see cref="Task"/> representing the running operation</returns>
        internal static Task Main(string[] args) => WatchdogFactory.CreateWatchdog().RunAsync(args, default);
	}
}
