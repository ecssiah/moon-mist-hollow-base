using System;
using Unity.Mathematics;

namespace MMH
{
	public class OnUpdateCitizenPositionArgs : EventArgs
	{
		public Citizen Citizen;

		public int2 StartPosition;
	}
}
