using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("AvastApiTests")]
namespace AvastApi.Utility
{
	internal class CommandUtility : ICommandUtility
	{
		public double GetTimeNowInMilliseconds()
		{
			return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
		}

		public byte[] GetNBytes(string fileName, int Size)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException(nameof(fileName));
			}

			if(fileName == string.Empty)
			{
				throw new ArgumentException(nameof(fileName));
			}

			if(Size <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(Size));
			}

			var assembly = Assembly.GetExecutingAssembly();
			string resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith(fileName));
			byte[] test = new byte[Size];
			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (BinaryReader reader = new BinaryReader(stream))
			{
				reader.BaseStream.Seek(0, SeekOrigin.Begin);
				reader.Read(test, 0, Size);
			}
			return test;
		}

		public string GetContent(string uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException(nameof(uri));
			}

			if(uri == string.Empty)
			{
				throw new ArgumentException(nameof(uri));
			}

			string content = null;
			using (WebClient client = new WebClient())
			{
				content = client.DownloadString(uri);
			}

			return content;
		}
	}
}
