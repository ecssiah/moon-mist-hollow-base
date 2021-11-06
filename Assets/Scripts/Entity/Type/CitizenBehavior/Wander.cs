using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Wander Behavior", menuName = "MMH/Entity/Type/CitizenBehavior/Wander")]
	public class Wander : CitizenBehavior
	{
		public int Cooldown;

		public override void Act(Citizen citizen)
		{
			Debug.Log($"Citizen {citizen.Id} is acting...");
		}
	}
}