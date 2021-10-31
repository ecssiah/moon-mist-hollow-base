using UnityEngine;

namespace MMH
{
	public abstract class Behavior : ScriptableObject
	{
		public abstract void Act(Entity entity);
	}
}
