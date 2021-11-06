using UnityEngine;

namespace MMH
{
	public abstract class CitizenBehavior : ScriptableObject
	{
		public abstract void Act(Citizen citizen);
	}
}
