using System;
using System.Collections.Generic;
using System.Text;

namespace AvastApiTests.Helper
{
	internal class HelperMethods
	{
		public static double GetRandomNumber(double minimum, double maximum)
		{
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}
	}
}
