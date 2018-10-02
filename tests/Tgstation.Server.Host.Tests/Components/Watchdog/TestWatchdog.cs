﻿using Byond.TopicSender;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Tgstation.Server.Api.Models.Internal;
using Tgstation.Server.Host.Components.Chat;
using Tgstation.Server.Host.Components.Compiler;
using Tgstation.Server.Host.Core;

namespace Tgstation.Server.Host.Components.Watchdog.Tests
{
	[TestClass]
	public sealed class TestWatchdog
	{
		[TestMethod]
		public void TestConstruction()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(null, null, null, null, null, null, null, null, null, null, null, null, default));

			var mockChat = new Mock<IChat>();
			mockChat.Setup(x => x.RegisterCommandHandler(It.IsNotNull<ICustomCommandHandler>())).Verifiable();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, null, null, null, null, null, null, null, null, null, null, null, default));

			var mockSessionControllerFactory = new Mock<ISessionControllerFactory>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, null, null, null, null, null, null, null, null, null, null, default));

			var mockDmbFactory = new Mock<IDmbFactory>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, null, null, null, null, null, null, null, null, null, null, default));

			var mockLogger = new Mock<ILogger<Watchdog>>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, null, null, null, null, null, null, null, null, default));

			var mockReattachInfoHandler = new Mock<IReattachInfoHandler>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, null, null, null, null, null, null, null, default));

			var mockDatabaseContextFactory = new Mock<IDatabaseContextFactory>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, null, null, null, null, null, null, default));

			var mockByondTopicSender = new Mock<IByondTopicSender>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, null, null, null, null, null, default));

			var mockEventConsumer = new Mock<IEventConsumer>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, mockEventConsumer.Object, null, null, null, null, default));

			var mockJobManager = new Mock<IJobManager>();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, mockEventConsumer.Object, mockJobManager.Object, null, null, null, default));

			var mockRestartRegistration = new Mock<IRestartRegistration>();
			mockRestartRegistration.Setup(x => x.Dispose()).Verifiable();
			var mockServerControl = new Mock<IServerControl>();
			mockServerControl.Setup(x => x.RegisterForRestart(It.IsNotNull<IRestartHandler>())).Returns(mockRestartRegistration.Object).Verifiable();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, mockEventConsumer.Object, mockJobManager.Object, mockServerControl.Object, null, null, default));

			var mockLaunchParameters = new DreamDaemonLaunchParameters();
			Assert.ThrowsException<ArgumentNullException>(() => new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, mockEventConsumer.Object, mockJobManager.Object, mockServerControl.Object, mockLaunchParameters, null, default));

			var mockInstance = new Models.Instance();
			new Watchdog(mockChat.Object, mockSessionControllerFactory.Object, mockDmbFactory.Object, mockLogger.Object, mockReattachInfoHandler.Object, mockDatabaseContextFactory.Object, mockByondTopicSender.Object, mockEventConsumer.Object, mockJobManager.Object, mockServerControl.Object, mockLaunchParameters, mockInstance, default).Dispose();

			mockRestartRegistration.VerifyAll();
			mockServerControl.VerifyAll();
			mockChat.VerifyAll();
		}
	}
}
