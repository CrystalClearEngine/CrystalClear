using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using CrystalClear.EventSystem;
using System.Reflection;
using System.Threading;

namespace CrystalClear.ScriptUtilities
{
	public static class StepRoutine
	{
		public static void Start(IEnumerable stepRoutine)
		{
			Thread thread = new Thread(() => _Start(stepRoutine));
			thread.Start();
		}

		private static async void _Start(IEnumerable stepRoutine)
		{
			foreach (WaitFor waitFor in stepRoutine)
			{
				await Task.Run(waitFor.Wait);
			}
			Thread.CurrentThread.Abort();
		}
	}

	public class WaitFor
	{
		public WaitFor(Type scriptEventType)
		{
			ScriptEvent scriptEvent =
				(ScriptEvent)scriptEventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null);

			scriptEvent.Subscribe(new Action(() => ShouldWait = false).Method, this);
		}

		private bool ShouldWait;

		public async Task Wait()
		{
			while (ShouldWait == false) ;
		}
	}
}
