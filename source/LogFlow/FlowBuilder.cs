using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LogFlow
{
	public class FlowBuilder
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public List<Flow> Flows = new List<Flow>();

		public void BuildAndRegisterFlow(Flow flow)
		{
			if(flow.FlowStructure.Context == null)
			{
				Log.Error("No Flow has been registered for {0}", flow.GetType().FullName);
				return;
			}

			var flowStructure = flow.FlowStructure;
			var logType = flowStructure.Context.LogType;
			var name = flowStructure.Context.Name;

			if(string.IsNullOrWhiteSpace(name))
			{
				Log.Error("No name for Flow has been registered for {0}. A name must be entered for each Flow.", flow.GetType().FullName);
				return;
			}

			if(Flows.Any(f => ReferenceEquals(f, flow)))
			{
				Log.Error("The Flow with the name {0} has already been added. The same flow may not be added twice.", name);
				return;
			}

			if(Flows.Any(f => f.FlowStructure.Context.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
			{
				Log.Error("There is already a Flow registered with the name " + name + ". Flow names must be unique.");
				return;
			}

			if(string.IsNullOrWhiteSpace(logType))
			{
				Log.Error("No logType for Flow has been registered for {0}. A logType must be entered for each Flow.", name);
				return;
			}

			if (flowStructure.Input == null)
			{
				Log.Error("Flow {0} doesn't have an input.", logType);
				return;
			}

			if (flowStructure.Output == null)
			{
				Log.Error("Flow {0} doesn't have an output.", logType);
				return;
			}

			Flows.Add(flow);
		}
	}
}
