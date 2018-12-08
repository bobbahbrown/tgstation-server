using System;
using System.Threading.Tasks;

namespace Tgstation.Server.Host.Components.Telnet
{
	/// <summary>
	/// For managing a telnet connection with a server
	/// </summary>
	public interface ITelnetConnection : IDisposable
	{
		/// <summary>
		/// Checks if the <see cref="ITelnetConnection"/> is connected
		/// </summary>
		/// <returns><see langword="true"/> if it is connected, <see langword="false"/> otherwise</returns>
		bool Connected();

		/// <summary>
		/// Establishes the telnet connection
		/// </summary>
		/// <param name="hostname">A <see langword="string"/> for the server address</param>
		/// <param name="port">The port the game server is hosted on</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		Task Connect(string hostname, int port);

		/// <summary>
		/// Sends a string followed by a newline over the telnet connection
		/// </summary>
		/// <param name="command">The literal string to send</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		Task SendLine(string command);

		/// <summary>
		/// Sends a string over the telnet connection
		/// </summary>
		/// <param name="command">The literal string to send</param>
		/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
		Task Send(string command);

		/// <summary>
		/// Attempts to read a line from the telnet connection
		/// </summary>
		/// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
		Task<string> ReadLine();
	}
}