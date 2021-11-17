using System;
using UnityEngine;

public static class Utils
{
	static System.Random _R = new System.Random();

	public static T RandomEnumValue<T>()
	{
		var valuesArray = Enum.GetValues(typeof(T));

		return (T)valuesArray.GetValue(_R.Next(valuesArray.Length));
	}
}
