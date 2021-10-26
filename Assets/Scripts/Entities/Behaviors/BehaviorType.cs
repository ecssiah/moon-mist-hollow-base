using UnityEngine;

namespace MMH
{
	public abstract class BehaviorType : ScriptableObject
	{
		public abstract void Act(Entity entity);
	}
}
