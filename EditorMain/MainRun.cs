using CrystalClear;
using CrystalClear.HierarchySystem;
using CrystalClear.RuntimeMain;
using CrystalClear.SerializationSystem.ImaginaryObjects;
using System;
using System.IO.Pipes;
using System.Reflection;
using System.Text;

partial class MainClass
{
	private static void Run(ImaginaryHierarchyObject rootHierarchyObject)
	{
		#region Running
		if (compiledAssembly is null)
		{
			Output.ErrorLog("error: code not compiling");
			return;
		}

		Console.Write("Choose a name for the hierarchy: ");
		string hierarchyName = Console.ReadLine();

		Output.Log();

#if DEBUG
		CrystalClearInformation.UserAssemblies = new[] { compiledAssembly, Assembly.GetAssembly(typeof(HierarchyObject)) };

		RuntimeMain.RunWithImaginaryHierarchyObject(new[] { compiledAssembly }, hierarchyName, rootHierarchyObject);

		while (true) ; // In DEBUG mode you cannot exit the run, since it runs it in the engine.
#endif

#if RELEASE
		Process userProcess = new Process();

		NamedPipeServerStream debugStream = new NamedPipeServerStream("CrystalClearDebugStream", PipeDirection.InOut);

		string args = $"{CurrentProject.BuildPath + @"\UserGeneratedCode.dll"} {"CrystalClearDebugStream"} --debug";

		userProcess.StartInfo = new ProcessStartInfo(@"E:\dev\crystal clear\RuntimeMain\bin\Debug\netcoreapp3.1\RuntimeMain.exe", args);

		userProcess.Start();

		debugStream.WaitForConnection();

		WriteString(debugStream, "Hello!");

		userProcess.WaitForExit();

		#region Exiting
		Console.WriteLine($"The program exited with code {userProcess.ExitCode}.");

		userProcess.Dispose();

		Console.WriteLine("Press any key to continue.");

		Console.ReadKey(true);

		Console.Clear();
		#endregion
#endif
		#endregion
	}

	public static string ReadString(PipeStream ioStream)
	{
		int len = 0;

		len = ioStream.ReadByte() * 256;
		len += ioStream.ReadByte();
		byte[] inBuffer = new byte[len];
		ioStream.Read(inBuffer, 0, len);

		return Encoding.ASCII.GetString(inBuffer);
	}

	public static int WriteString(PipeStream ioStream, string outString)
	{
		byte[] outBuffer = Encoding.ASCII.GetBytes(outString);
		int len = outBuffer.Length;
		if (len > UInt16.MaxValue)
		{
			len = (int)UInt16.MaxValue;
		}
		ioStream.WriteByte((byte)(len / 256));
		ioStream.WriteByte((byte)(len & 255));
		ioStream.Write(outBuffer, 0, len);
		ioStream.Flush();

		return outBuffer.Length + 2;
	}
}