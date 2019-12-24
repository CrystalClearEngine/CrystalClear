using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalClear.Standard.Events;
using System.Diagnostics;
using System.Threading;

public static class FrameUpdateLoop
{
	public static void Loop()
	{
		Stopwatch stopwatch = new Stopwatch();
		while(true)
		{
			stopwatch.Start();
			Thread.Sleep(0);
			FrameUpdateEventClass.Instance.RaiseEvent();
			Console.WriteLine(Math.Round(1 /stopwatch.Elapsed.TotalSeconds) + " FPS");
			stopwatch.Stop(); stopwatch.Reset();
		}
	}
}
