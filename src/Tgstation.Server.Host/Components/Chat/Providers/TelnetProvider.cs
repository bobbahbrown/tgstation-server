using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tgstation.Server.Api.Models;
using Tgstation.Server.Host.Components.Telnet;
using Tgstation.Server.Host.Core;

namespace Tgstation.Server.Host.Components.Chat.Providers
{
	/// <summary>
	/// <see cref="IProvider"/> for the telnet connection to the gameserver
	/// </summary>
	sealed class TelnetProvider : Provider
	{
		/// <summary>
		/// The game verb used to let the gameserver know that the client is a tgs bot
		/// </summary>
		private const string GameserverInitCommand = "TgsTelnetInit";

		/// <summary>
		/// <see cref="TelnetConnection"/> object used for managing the actual connection with the gameserver
		/// </summary>
		readonly TelnetConnection telnetConnection;

		/// <summary>
		/// The <see cref="IAsyncDelayer"/> for the <see cref="TelnetProvider"/>
		/// </summary>
		readonly IAsyncDelayer asyncDelayer;

		/// <summary>
		/// The <see cref="ILogger"/> for the <see cref="TelnetProvider"/>
		/// </summary>
		readonly ILogger<TelnetProvider> logger;

		/// <summary>
		/// Address of the server to connect to
		/// </summary>
		readonly string address;

		/// <summary>
		/// Port of the server to connect to
		/// </summary>
		readonly ushort port;

		/// <summary>
		/// Gameserver nickname
		/// </summary>
		readonly string nickname;

		/// <summary>
		/// Map of <see cref="Channel.RealId"/>s to channel names
		/// </summary>
		readonly Dictionary<ulong, string> channelIdMap;

		/// <summary>
		/// Id counter for <see cref="channelIdMap"/>
		/// </summary>
		private ulong channelIdCounter;

		/// <summary>
		/// Initializes a new instance of the <see cref="TelnetProvider"/> class.
		/// </summary>
		/// <param name="application">The <see cref="IApplication"/> to get the <see cref="IApplication.VersionString"/> from</param>
		/// <param name="asyncDelayer">The value of <see cref="asyncDelayer"/></param>
		/// <param name="logger">The value of logger</param>
		/// <param name="address">The value of <see cref="address"/></param>
		/// <param name="port">The value of <see cref="port"/></param>
		/// <param name="nickname">The value of <see cref="nickname"/></param>
		public TelnetProvider(IApplication application, IAsyncDelayer asyncDelayer, ILogger<TelnetProvider> logger, string address, ushort port, string nickname)
		{
			if (application == null)
				throw new ArgumentNullException(nameof(application));
			this.asyncDelayer = asyncDelayer ?? throw new ArgumentNullException(nameof(asyncDelayer));
			this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

			this.address = address ?? throw new ArgumentNullException(nameof(address));
			this.port = port;
			this.nickname = nickname ?? throw new ArgumentNullException(nameof(nickname));

			telnetConnection = new TelnetConnection();

			channelIdMap = new Dictionary<ulong, string>();
			channelIdCounter = 1;
		}

		/// <inheritdoc/>
		public override void Dispose() => telnetConnection.Dispose();

		/// <inheritdoc/>
		public override async Task<bool> Connect(CancellationToken cancellationToken)
		{
			try
			{
				cancellationToken.ThrowIfCancellationRequested();

				await telnetConnection.Connect(address, port).ConfigureAwait(false);
				await telnetConnection.SendLine($"{GameserverInitCommand} {nickname}").ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception e)
			{
				logger.LogWarning($"Error establishing telnet connection with game server: {e}");
				return false;
			}

			return true;
		}

		/// <inheritdoc/>
		public override bool Connected => telnetConnection.Connected();

		/// <inheritdoc/>
		public override Task Disconnect(CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc/>
		public override string BotMention => throw new System.NotImplementedException();

		/// <inheritdoc/>
		public override Task<IReadOnlyList<Channel>> MapChannels(IEnumerable<ChatChannel> channels, CancellationToken cancellationToken)
		{
			if (channels == null)
				throw new ArgumentNullException(nameof(channels));

			if (!Connected)
				throw new InvalidOperationException("Provider not connected!");

			Channel GetChannelForChatChannel(ChatChannel channel)
			{
				if (channel.TelnetChannel == null)
					throw new InvalidOperationException("ChatChannel missing TelnetChannel");

				ulong id = channelIdCounter++;
				channelIdMap.Add(id, channel.TelnetChannel);

				return new Channel
				{
					RealId = id,
					IsAdminChannel = channel.IsAdminChannel == true,
					ConnectionName = "Gameserver",
					FriendlyName = channel.TelnetChannel,
					IsPrivateChannel = false,
					Tag = channel.Tag
				};
			}

			return Task.FromResult<IReadOnlyList<Channel>>(channels.Select(x => GetChannelForChatChannel(x)).Where(x => x != null).ToList());
		}

		/// <inheritdoc/>
		public override async Task SendMessage(ulong channelId, string message, CancellationToken cancellationToken)
		{
			try
			{
				cancellationToken.ThrowIfCancellationRequested();

				await telnetConnection.SendLine(message).ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception e)
			{
				logger.LogWarning($"Error sending message via telnet connection: {e}");
			}
		}
	}
}