namespace MMH
{
	public class GameSystem
	{
		public virtual void Init() { }
		public virtual void Tick(object sender, OnTickArgs eventArgs) { }
		public virtual void Quit() { }
	}
}
