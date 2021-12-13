namespace MMH
{
	public class SimulationSystem
	{
		public virtual void Init() { }
		protected virtual void Tick(object sender, OnTickArgs eventArgs) { }
		public virtual void Quit() { }
	}
}
