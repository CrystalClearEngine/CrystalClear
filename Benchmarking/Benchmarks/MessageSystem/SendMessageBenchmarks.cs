﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using CrystalClear.MessageSystem;

namespace Benchmarks.MessageSystemBenchmarks
{
	public class SampleMessage : Message
	{
		public delegate void SampleMessageDelegate(SampleMessage message);

		public Type DelegateType => typeof(SampleMessageDelegate); // For LINQDynamicInvokeSender.
	}

	[MemoryDiagnoser]
	public class SendMessageBenchmarks
	{
		SampleMessage sampleMessage = new SampleMessage();
		
		[OnReceiveMessage(typeof(SampleMessage))]
		public void MessageReceiver(SampleMessage message)
		{
		}

		[Benchmark(Baseline = true)]
		public void CurrentSendMessage()
		{
			sampleMessage.SendTo(this);
		}

		[Benchmark]
		public void LINQInvoke()
		{
			LINQInvokeSender(sampleMessage, this);
		}

		private void LINQInvokeSender(Message message, object recipient)
		{
			if (!message.AllowInstanceMethods)
				return;

			IEnumerable<MethodInfo> messageReceivers =
				from MethodInfo method in recipient.GetType()
					.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType()
				select method;

			foreach (MethodInfo messageReceiver in messageReceivers)
			{
				messageReceiver.Invoke(recipient, messageReceiver.GetParameters().Length == 0 ? null : new[] {message});
			}
		}

		[Benchmark]
		public void LINQDynamicInvoke()
		{
			LINQDynamicInvokeSender(sampleMessage, this);
		}

		private static void LINQDynamicInvokeSender(Message message, object recipient)
		{
			if (!message.AllowInstanceMethods)
				return;

			IEnumerable<MethodInfo> messageReceivers =
				from MethodInfo method in recipient.GetType()
					.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where method.GetCustomAttribute<OnReceiveMessageAttribute>()?.MessageType == message.GetType()
				select method;

			IEnumerable<Delegate> toCall = from MethodInfo method in messageReceivers
				select method.CreateDelegate(((SampleMessage) message).DelegateType, recipient);

			foreach (Delegate item in toCall)
			{
				item.DynamicInvoke(message);
			}
		}
	}
}