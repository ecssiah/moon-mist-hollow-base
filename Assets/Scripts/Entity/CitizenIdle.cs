using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenState
	{
		public override void OnTick()
		{
			Debug.Log("Idle ticking");
		}
	}
}
