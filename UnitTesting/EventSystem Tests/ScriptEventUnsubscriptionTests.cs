﻿using System.Collections.Generic;
using CrystalClear.EventSystem;
using CrystalClear.EventSystem.StandardEvents;
using CrystalClear.HierarchySystem;
using CrystalClear.HierarchySystem.Scripting;
using CrystalClear.ScriptUtilities;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using CrystalClear.Standard.HierarchyObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class ScriptEventUnsubscriptionTests
	{
		[TestMethod]
		public void MyTestMethod()
		{
			var scriptObject = new ScriptObject
			{
				LocalHierarchy =
				{
					new KeyValuePair<string, HierarchyObject>("Child", new ScriptObject
						{
							AttachedScripts =
							{
								{"Script", new Script(new ImaginaryConstructableObject(typeof(TestScript)))},
							},
						}
					),
				},
			};
			Assert.IsNotNull(TestEvent.Instance.GetSubscribers(), "The Script wasn't subscribed to the TestEvent.");
			scriptObject.DestroyChild("Child");
			Assert.IsNull(TestEvent.Instance.GetSubscribers(),
				"The Script wasn't unsubscribed from the TestEvent after it's HierarchyObject got removed.");
		}

		private sealed class OnTestEvent : SubscribeToAttribute
		{
			public OnTestEvent() : base(typeof(StartEvent))
			{
			}
		}

		private class TestEvent : ScriptEvent<StartEvent>
		{
		}

		[IsScript]
		private class TestScript
		{
			[OnTestEvent]
			public void OnStart()
			{
			}
		}
	}
}