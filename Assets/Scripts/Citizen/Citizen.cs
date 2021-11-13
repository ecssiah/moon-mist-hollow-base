using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH
{
    public class Citizen
    {
		private static int nextID = 0;

		private CitizenState currentState;

		public WorldMap WorldMap;

		public int Id;
		public int2 Position;
		public Direction Direction;

		public Nation Nation;

        public Citizen()
		{
			TimeSystem.OnTick += OnTick;

			Id = nextID++;

			currentState = CitizenSystem.States[CitizenStateType.CitizenIdle];
		}
		
		public void SetState(CitizenStateType citizenStateType)
		{
			currentState = CitizenSystem.States[citizenStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickEventArgs eventArgs)
		{
			currentState.Tick();
		}
	}
}
