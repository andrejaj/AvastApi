using AvastApi.Utility;
using NUnit.Framework;
using System;

namespace Tests
{
	/// <summary>
	/// Integration tests
	/// </summary>
	public class CommandUtilityTests
	{
		private ICommandUtility commandUtility;
		[SetUp]
		public void Setup()
		{
			commandUtility = new CommandUtility();
		}

		[Test]
		public void EpochTime_Not_Zero()
		{
			//Act
			var epochTime = commandUtility.GetEpochTimeMilliseconds();

			//Assert
			Assert.NotZero(epochTime);
		}

		[Test]
		public void Content_From_Uri_Received()
		{
			//Act
			var content = commandUtility.GetContent("http://www.google.com");

			//Assert
			Assert.IsNotNull(content);
			Assert.IsNotEmpty(content);
			Assert.That(content.Contains("<title>Google</title>"));
		}

		[Test]
		public void Content_Throws_Exception_When_Url_Is_Null()
		{
			//Act
			var ex = Assert.Throws<ArgumentNullException>(() => commandUtility.GetContent(null));

			//Assert
			StringAssert.StartsWith("Value cannot be null.", ex.Message);
		}

		[Test]
		public void Content_Throws_Exception_When_Url_Is_Empty()
		{
			//Act
			var ex = Assert.Throws<ArgumentException>(() => commandUtility.GetContent(String.Empty));

			//Assert
			Assert.That(ex.Message, Is.EqualTo("uri"));
		}

		[Test]
		public void Content_Throws_Exception_When_Url_Incorrect()
		{
			//Act
			var ex = Assert.Throws<System.Net.WebException>(() => commandUtility.GetContent("ww.google.com"));

			//Assert
			StringAssert.Contains("Could not find file", ex.Message);
		}

		[TestCase(6)]
		public void Bytes_Received(int byteLength)
		{
			//Act
			var bytes = commandUtility.GetNBytes("DemoFile.txt", byteLength);

			//Assert
			Assert.That(bytes.Length == byteLength);
		}

		[Test]
		public void Bytes_Throws_Exception_When_Zero()
		{
			//Act
			var ex = Assert.Throws<ArgumentOutOfRangeException>(() => commandUtility.GetNBytes("DemoFile.txt", 0));

			//Assert
			StringAssert.StartsWith("Specified argument was out of the range of valid values.", ex.Message);
		}

		[Test]
		public void Bytes_Throws_Exception_When_Filename_Is_NULL()
		{
			//Act
			var ex = Assert.Throws<ArgumentNullException>(() => commandUtility.GetNBytes(null, 0));

			//Assert
			StringAssert.StartsWith("Value cannot be null.", ex.Message);
		}

		[Test]
		public void Bytes_Throws_Exception_When_Filename_Is_Empty()
		{
			//Act
			var ex = Assert.Throws<ArgumentException>(() => commandUtility.GetNBytes(String.Empty, 0));

			//Assert
			Assert.That(ex.Message, Is.EqualTo("fileName"));
		}

		[Test]
		public void Bytes_Throws_Exception_When_Filename_Does_Not_Exist()
		{
			//Act
			var ex = Assert.Throws<InvalidOperationException>(() => commandUtility.GetNBytes(new Guid().ToString(), 6));

			//Assert
			Assert.That(ex.Message, Is.EqualTo("Sequence contains no matching element"));
		}
	}
}