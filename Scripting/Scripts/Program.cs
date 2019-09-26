using Scripting.Events;
using Scripting.ScriptAttributes;
using Scripting;
using System;

[Script]
public class HelloWorldExample : IEStart, IEFrameUpdate
{
	public void OnStart()
	{
		Output.Log("Hello, World!", ConsoleColor.Green, ConsoleColor.Black);
		Output.Log("Hello, World!", ConsoleColor.Blue, ConsoleColor.Red);
		Output.Log((string)UserSettings.GetSetting("Test").value);
		Output.Log(UserSettings.GetSetting("TestObj").value.ToString());
		UserSettings.SetUp();
		UserSettings.SaveSetting(new UserSettings.UserSetting("Test", "Test 0"));
		UserSettings.SaveSetting(new UserSettings.UserSetting("TestObj", "1"));
	}

	public void OnFrameUpdate(float timeSinceLastFrame)
	{
		Output.Log("We shouldn´t get here...");
	}
}