using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenMovementState
	{
		public CitizenIdle(Citizen citizen)
		{
			_citizen = citizen;
		}

		public override void Tick()
		{
			if (_citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				_citizen.Cooldown = Random.Range(4, 17);
				_citizen.Direction = newDirection;

				_citizen.UpdateRenderDirection();
			}
		}
	}
}
