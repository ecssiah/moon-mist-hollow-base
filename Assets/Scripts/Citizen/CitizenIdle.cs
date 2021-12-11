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
				_citizen.Cooldown = Utils.RandomRange(4, 16);
				_citizen.Direction = Utils.RandomEnumValue<Direction>();

				_citizen.UpdateRenderDirection();
			}
		}
	}
}
