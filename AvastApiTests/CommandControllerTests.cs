using AvastApi.Controllers;
using AvastApi.Utility;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using AvastApiTests.Helper;

namespace Tests
{
	public class CommandControllerTests
	{
		private CommandController _commandController;
		private Mock<AbstractLogger<CommandController>> _loggerMock;
		private Mock<ICommandUtility> _commandUtilityMock;
		
		[SetUp]
		public void Setup()
		{
			_loggerMock = new Mock<AbstractLogger<CommandController>>();
			_commandUtilityMock = new Mock<ICommandUtility>();
		}

		[Test]
		public void Epoch_Time_Received()
		{
			var value = HelperMethods.GetRandomNumber(1.0, 2.0);
			_commandUtilityMock.Setup(x => x.GetEpochTimeMilliseconds()).Returns(value);
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			var response = _commandController.GetEpochTime();
			var content = (OkObjectResult)response.Result;
			Assert.IsInstanceOf<OkObjectResult>(response.Result);
			Assert.AreEqual(value,  content.Value);
		}

		[Test]
		public void Epoch_Time_Throws_Exception()
		{
			//Arrange
			_commandUtilityMock.Setup(x => x.GetEpochTimeMilliseconds()).Throws(new Exception());
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			//Act
			var response = _commandController.GetEpochTime();
			var content = (ObjectResult)response.Result;

			//Assert
			Assert.IsInstanceOf<ObjectResult>(response.Result);
			_loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
			Assert.AreEqual((int)System.Net.HttpStatusCode.InternalServerError, content.StatusCode);
		}

		[Test]
		public void NBytes_Received()
		{ 
			//Arrange
			var bytes = new byte[] { 30, 60, 40 };
			_commandUtilityMock.Setup(x => x.GetNBytes(It.IsAny<string>(), It.IsAny<int>())).Returns(bytes);
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			//Act
			var response = _commandController.GetNBytes(new Guid().ToString(), bytes.Length);

			//Act
			var content = (OkObjectResult)response.Result;
			Assert.IsInstanceOf<OkObjectResult>(response.Result);
			var expectedValue = content.Value as byte[];
			CollectionAssert.AreEqual(bytes, expectedValue);
		}

		[Test]
		public void NBytes_Throws_Exception()
		{
			//Arrange
			_commandUtilityMock.Setup(x => x.GetNBytes(It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception());
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			//Act
			var response = _commandController.GetNBytes(new Guid().ToString(), 1);

			//Act
			var content = (ObjectResult)response.Result;

			//Assert
			Assert.IsInstanceOf<ObjectResult>(response.Result);
			_loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
			Assert.AreEqual((int)System.Net.HttpStatusCode.InternalServerError, content.StatusCode);
		}

		[Test]
		public void Content_Received_From_URL()
		{
			//Arrange
			var value = new Guid().ToString();
			_commandUtilityMock.Setup(x => x.GetContent(It.IsAny<string>())).Returns(value);
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			//Act
			var response = _commandController.GetContent(new Guid().ToString());
			var content = (OkObjectResult)response.Result;

			//Assert
			Assert.IsInstanceOf<OkObjectResult>(response.Result);
			Assert.AreEqual(value, content.Value);
		}

		[Test]
		public void Content_Throws_Exception()
		{
			//Arrange
			_commandUtilityMock.Setup(x => x.GetContent(It.IsAny<string>())).Throws(new Exception());
			_commandController = new CommandController(_loggerMock.Object, _commandUtilityMock.Object);

			//Act
			var response = _commandController.GetContent(new Guid().ToString());
			var content = (ObjectResult)response.Result;

			//Assert
			Assert.IsInstanceOf<ObjectResult>(response.Result);
			_loggerMock.Verify(x => x.Log(LogLevel.Error, It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
			Assert.AreEqual((int)System.Net.HttpStatusCode.InternalServerError, content.StatusCode);
		}
	}

	public abstract class AbstractLogger<T> : ILogger<T>
	{
		public IDisposable BeginScope<TState>(TState state)
			 => throw new NotImplementedException();

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
			 => Log(logLevel, exception, formatter(state, exception));

		public abstract void Log(LogLevel logLevel, Exception ex, string information);
	}
}
