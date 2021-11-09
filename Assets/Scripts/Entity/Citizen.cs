namespace MMH
{
    public class Citizen : Entity
    {
        private CitizenStateManager citizenStateManager;

        public Nation Nation;
        public WorldMap WorldMap;

        public Citizen()
		{
            citizenStateManager = new CitizenStateManager(this, WorldMap);
		}

    }
}
