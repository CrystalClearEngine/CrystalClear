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
		UserSettings.SaveSetting(new UserSettings.UserSetting("TestObj", new List<string>() { "Hello, World!", "Whats up world?", "Hey", "adasgasds" }));
		Output.Log((string)UserSettings.GetSetting("Test").value);
		List<string> intList = (List<string>)UserSettings.GetSetting("TestObj").value;
		foreach (string str in intList)
		{
			Output.Log(str);
		}
		Console.WriteLine(UserSettings.savePath);
	}

	public void OnFrameUpdate(float timeSinceLastFrame)
	{
		Output.Log("We shouldn´t get here...");
	}
}