using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GridToWorld(int x, int y)
	{
		return new Vector3();
	}

	static System.Random _R = new System.Random();

	public static T RandomEnumValue<T>()
	{
		var valuesArray = Enum.GetValues(typeof(T));

		return (T)valuesArray.GetValue(_R.Next(valuesArray.Length));
	}
}
