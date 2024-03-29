﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvastApi.Utility
{
	public interface ICommandUtility
	{
		double GetTimeNowInMilliseconds();
		byte[] GetNBytes(string fileName, int Size);
		string GetContent(string uri);
	}
}
