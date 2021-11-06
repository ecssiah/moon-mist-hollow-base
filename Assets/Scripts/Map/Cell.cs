using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class Cell
	{
		public int Id;
		public int2 Position;

		public OverlayType OverlayType;
		public StructureType StructureType;
		public GroundType GroundType;
	}
}
