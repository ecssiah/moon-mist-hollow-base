using System;

namespace MMH
{
	public static class Utils
	{
		static Random _R = new Random();

		public static int RandomRange(int minInclusive, int maxInclusive)
		{
			return _R.Next(minInclusive, maxInclusive + 1);
		}

		public static T RandomEnumValue<T>()
		{
			var valuesArray = Enum.GetValues(typeof(T));

			return (T)valuesArray.GetValue(_R.Next(valuesArray.Length));
		}
	}
}
