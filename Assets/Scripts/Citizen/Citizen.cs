using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class Citizen
    {
		private static int nextID = 1;

		private Dictionary<CitizenStateType, CitizenState> states;

		public int Id;
		public int2 Position;
		public bool Moving;
		public Direction Direction;
		public Nation Nation;
		public CitizenAttributes Attributes;
		
		private CitizenState currentState;

		private int cooldown;
		public int Cooldown => cooldown;

		public Citizen()
		{
			TimeSystem.OnTick += OnTick;

			Id = nextID++;

			states = new Dictionary<CitizenStateType, CitizenState>
			{
				[CitizenStateType.CitizenIdle] = new CitizenIdle(this),
				[CitizenStateType.CitizenWander] = new CitizenWander(this)
			};

			currentState = states[CitizenStateType.CitizenIdle];
		}

		public CitizenStateType GetCitizenStateType()
		{
			return currentState.CitizenStateType;
		}

		public void SetState(CitizenStateType citizenStateType)
		{
			currentState = states[citizenStateType];
		}

		public void SetCooldown(int duration)
		{
			cooldown = duration;
		}

		private void OnTick(object sender, TimeSystem.OnTickArgs eventArgs)
		{
			cooldown--;

			currentState.Tick();
		}
	}
}
