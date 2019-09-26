using Scripting.Events;
using Scripting.ScriptAttributes;
using Scripting;
using System;
using System.Collections.Generic;

[Script]
public class HelloWorldExample : IEStart, IEFrameUpdate
{
	public void OnStart()
	{
		Output.Log("Hello, World!", ConsoleColor.Green, ConsoleColor.Black);
		Output.Log("Hello, World!", ConsoleColor.Blue, ConsoleColor.Red);
		UserSettings.SetUp();
		UserSettings.SaveSetting(new UserSettings.UserSetting("Test", "Test 0"));
		UserSettings.SaveSetting(new UserSettings.UserSetting("TestObj", new List<int>() { 100, 200, 1337 }));
		Output.Log((string)UserSettings.GetSetting("Test").value);
		List<int> intList = (List<int>)UserSettings.GetSetting("TestObj").value;
		foreach (int num in intList)
		{
			Output.Log(num.ToString());
		}
	}

	public void OnFrameUpdate(float timeSinceLastFrame)
	{
		Output.Log("We shouldn´t get here...");
	}
}