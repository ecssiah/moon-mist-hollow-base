using Unity.Mathematics;

namespace MMH
{
    public class Citizen
    {
		private static int currentId = 0;

		private CitizenStateManager citizenStateManager;

		public int Id;
		public int2 Position;
		public Direction Direction;

		public Nation Nation;
        public WorldMap WorldMap;

        public Citizen()
		{
			Id = currentId++;

			citizenStateManager = new CitizenStateManager(this, WorldMap);
		}
    }
}
