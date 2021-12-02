using System;
using Unity.Mathematics;

namespace MMH
{
	public class OnUpdateCitizenPositionArgs : EventArgs
	{
		public int Ticks;
		public int2 PreviousPosition;
		public Citizen Citizen;
	}
}
