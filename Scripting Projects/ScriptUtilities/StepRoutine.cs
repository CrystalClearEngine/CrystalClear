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
		public static void Start(IEnumerator enumerator)
		{
			ScriptEventHandler scriptEventHandlerAction = new ScriptEventHandler(() => new Stack());
			scriptEventHandlerAction = new ScriptEventHandler(() => Test(enumerator, scriptEventHandlerAction));
			enumerator.MoveNext();
			((WaitFor)enumerator.Current).ScriptEvent.Subscribe(scriptEventHandlerAction);
		}

		public static void Test(IEnumerator enumerator, ScriptEventHandler scriptEventHandlerAction)
		{
			((WaitFor)enumerator.Current).ScriptEvent.Unsubscribe(scriptEventHandlerAction);
			if (enumerator.MoveNext())
			{
				((WaitFor)enumerator.Current).ScriptEvent.Subscribe(scriptEventHandlerAction);
			}
		}
	}

	public class WaitFor // TODO: create separate WaitForScriptEvent and keep this as a base.
	{
		public ScriptEvent ScriptEvent;

		public WaitFor(Type scriptEventType)
		{
			ScriptEvent =
				(ScriptEvent)scriptEventType
				.GetProperty("Instance", bindingAttr: BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
				.GetValue(null); // TODO: add an ISingleton<maybe T> that we can then do GetInstance from instead of this.
		}

		public WaitFor(ScriptEvent scriptEvent)
		{
			ScriptEvent = scriptEvent;
		}
	}
}
