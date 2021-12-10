namespace MMH
{
	public class TimeSystem : GameSystem
	{
		private int _ticks;

		public override void Init()
		{
			_ticks = 0;
		}

		public override void Tick(object sender, OnTickArgs eventArgs)
		{
			_ticks++;
		}
	}
}
