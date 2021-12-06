using Unity.Mathematics;

namespace MMH
{
	public class Cell
	{
		public int Id;
		public int2 Position;
		public bool Solid;

		public OverlayType OverlayType;
		public StructureType StructureType;
		public GroundType GroundType;
	}
}
