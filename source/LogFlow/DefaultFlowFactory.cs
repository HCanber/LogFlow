using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NLog;

namespace LogFlow
{
	/// <summary>
	/// The default <see cref="IFlowFactory"/> that creates an instance of 
	/// every class that inherits from <see cref="Flow"/> and has a parameterless
	/// public constructor. To prohibit a class from being automatically instantiated
	/// mark it with <see cref="DoNotAutoCreateAttribute"/>
	/// </summary>
	public class DefaultFlowFactory : IFlowFactory
	{
		private readonly Logger _log = LogManager.GetCurrentClassLogger();

		public IEnumerable<Flow> CreateFlows()
		{
			var flowTypes = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => !assembly.FullName.StartsWith("System.") && !assembly.FullName.StartsWith("Microsoft."))
				.SelectMany(assembly => assembly.GetTypes())
				.Where(type =>
					!type.IsAbstract &&                                               //Class may not be abstract
					type.IsSubclassOf(typeof(Flow)) &&                                //Class must inherit from Flow
					type.GetCustomAttribute<DoNotAutoCreateAttribute>() == null &&    //Class may not be marked with [DoNotAutoCreate]
					type.GetConstructor(Type.EmptyTypes) != null                      //Must have parameterless public constructor
				)
				.ToList();

			_log.Trace("Number of flows found by DefaultFlowFactory: {0}", flowTypes.Count);

			var flows = new List<Flow>();
			foreach (var flowType in flowTypes)
			{
				try
				{
					var flow = (Flow)Activator.CreateInstance(flowType);
					flows.Add(flow);
				}
				catch (Exception exception)
				{
					_log.Error(exception);
				}
			}
			return flows;
		}
	}
}