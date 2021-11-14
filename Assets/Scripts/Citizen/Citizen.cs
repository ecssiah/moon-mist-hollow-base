using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH
{
    public class Citizen
    {
		private static int nextID = 0;

		private Dictionary<CitizenStateType, CitizenState> states;

		public int Id;
		public int2 Position;
		public Direction Direction;
		public Nation Nation;
		public CitizenAttributes Attributes;
		
		private CitizenState currentState;

		public Citizen(CitizenSystem citizenSystem, MapSystem mapSystem)
		{
			TimeSystem.OnTick += OnTick;

			Id = nextID++;

			states = new Dictionary<CitizenStateType, CitizenState>
			{
				[CitizenStateType.CitizenIdle] = new CitizenIdle(citizenSystem, mapSystem),
				[CitizenStateType.CitizenWander] = new CitizenWander(citizenSystem, mapSystem)
			};

			currentState = states[CitizenStateType.CitizenWander];
		}

		public void SetState(CitizenStateType citizenStateType)
		{
			currentState = states[citizenStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickEventArgs eventArgs)
		{
			currentState.Tick(this);
		}
	}
}
