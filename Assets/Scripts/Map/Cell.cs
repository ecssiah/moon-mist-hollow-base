using Unity.Mathematics;

namespace MMH
{
	public class Cell
	{
		public Cell(int id)
		{
			Id = id;
		}

		public int Id { get; private set; }
		public bool Solid { get; set; }
		public int2 Position { get; set; }

		public OverlayType OverlayType { get; set; }
		public StructureType StructureType { get; set; }
		public GroundType GroundType { get; set; }
	}
}
