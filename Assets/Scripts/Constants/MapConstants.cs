using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH
{
    public enum GroundType
    {
        None,
        Floor1,
        Floor2,
    }

    public enum StructureType
    {
        None,
        Wall1,
        Wall2,
    }

    public enum OverlayType
    {
        None,
        Outline1,
        Outline2,
    }

    public enum Direction
    {
        EE,
        NE,
        NN,
        NW,
        WW,
        SW,
        SS,
        SE,
    }

	public static class MapConstants
	{
        public static Dictionary<Direction, int2> DirectionVectors = new Dictionary<Direction, int2>
        {
            [Direction.EE] = new int2(+1, +0),
            [Direction.NE] = new int2(+1, +1),
            [Direction.NN] = new int2(+0, +1),
            [Direction.NW] = new int2(-1, +1),
            [Direction.WW] = new int2(-1, +0),
            [Direction.SW] = new int2(-1, -1),
            [Direction.SS] = new int2(+0, -1),
            [Direction.SE] = new int2(+1, -1),
        };

        public static Dictionary<Direction, int> DirectionCosts = new Dictionary<Direction, int>
        {
            [Direction.EE] = 10,
            [Direction.NE] = 14,
            [Direction.NN] = 10,
            [Direction.NW] = 14,
            [Direction.WW] = 10,
            [Direction.SW] = 14,
            [Direction.SS] = 10,
            [Direction.SE] = 14,
        };
    }
}