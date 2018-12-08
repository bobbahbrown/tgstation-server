using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tgstation.Server.Host.Components.Telnet
{
	/// <summary>
	/// The object managing a telnet connection to a game server
	/// </summary>
	sealed class TelnetConnection : ITelnetConnection
	{
		private readonly TcpClient tcpClient;

		private readonly Encoding encoding;

		private NetworkStream networkStream;

		private StreamReader streamReader;

		/// <summary>
		/// Initializes a new instance of the <see cref="TelnetConnection"/> class.
		/// </summary>
		public TelnetConnection()
		{
			tcpClient = new TcpClient();

			encoding = Encoding.ASCII;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if(streamReader != null) { streamReader.Close(); }
			tcpClient.Close();
		}

		/// <inheritdoc/>
		public bool Connected()
		{
			return tcpClient.Connected;
		}

		/// <inheritdoc/>
		public async Task Connect(string hostname, int port)
		{
			await tcpClient.ConnectAsync(hostname, port).ConfigureAwait(false);
			networkStream = tcpClient.GetStream();

			if (!networkStream.CanRead || !networkStream.CanWrite)
			{
				networkStream.Close();
				throw new NotSupportedException("The given telnet connection must allow both reading and writing");
			}

			streamReader = new StreamReader(networkStream);
		}

		/// <inheritdoc/>
		public async Task SendLine(string command)
		{
			command += "\n";
			await Send(command).ConfigureAwait(false);
		}

		/// <inheritdoc/>
		public async Task Send(string command)
		{
			if(!Connected())
				throw new InvalidOperationException("The telnet connection must be established before messages can be sent");

			byte[] encodedText = encoding.GetBytes(command);
			await networkStream.WriteAsync(encodedText);
		}

		/// <inheritdoc/>
		public async Task<string> ReadLine()
		{
			return await streamReader.ReadLineAsync().ConfigureAwait(false);
		}
	}
}