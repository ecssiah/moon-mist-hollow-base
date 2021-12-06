using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class Citizen
	{
		private static int _nextCitizenId = 1;

		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenDirection;
		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenPosition;

		private Dictionary<CitizenMovementStateType, CitizenMovementState> _movementStates;

		private CitizenMovementState _currentMovementState;

		private readonly int _id;
		public int Id { get => _id; }

		private Direction _direction;
		public Direction Direction { get => _direction; set => _direction = value; }

		private int2 _position;
		public int2 Position { get => _position; set => _position = value; }

		private Nation _nation;
		public Nation Nation { get => _nation; set => _nation = value; }

		private CitizenAttributes _attributes;
		public CitizenAttributes Attributes { get => _attributes; }

		private int _cooldown;
		public int Cooldown { get => _cooldown; set => _cooldown = value; }

		public Citizen()
		{
			_id = _nextCitizenId++;

			_movementStates = new Dictionary<CitizenMovementStateType, CitizenMovementState>
			{
				[CitizenMovementStateType.Idle] = new CitizenIdle(this),
				[CitizenMovementStateType.Wander] = new CitizenWander(this)
			};

			_attributes = new CitizenAttributes
			{
				Strength = 1,
				Intelligence = 1,
				Speed = 1,
			};

			_currentMovementState = _movementStates[CitizenMovementStateType.Idle];
		
			TimeSystem.OnTick += OnTick;
		}

		public CitizenMovementStateType GetCitizenMovementStateType()
		{
			return _currentMovementState.CitizenMovementStateType;
		}

		public bool CanAct()
		{
			return _cooldown <= 0;
		}

		public void UpdateRenderDirection()
		{
			OnUpdateCitizenDirection?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void UpdateRenderPosition()
		{
			OnUpdateCitizenPosition?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void SetMovementState(CitizenMovementStateType citizenMovementStateType)
		{
			_currentMovementState = _movementStates[citizenMovementStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickArgs eventArgs)
		{
			_cooldown--;

			_currentMovementState.Tick();
		}
	}
}
