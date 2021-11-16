using System;
using Unity.Mathematics;

namespace MMH
{
	public class OnUpdateCitizenPositionEventArgs : EventArgs
	{
		public int2 PreviousPosition;
		public Citizen Citizen;
	}
}
