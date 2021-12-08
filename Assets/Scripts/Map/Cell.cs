using Unity.Mathematics;

namespace MMH
{
	public class Cell
	{
		public int Id;
		public bool Solid;
		public int2 Position;

		public OverlayType OverlayType;
		public StructureType StructureType;
		public GroundType GroundType;
	}
}
