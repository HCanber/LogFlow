using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using NLog;

namespace LogFlow
{
	public class LogFlowEngine
	{
		private static readonly Logger Log = LogManager.GetCurrentClassLogger();
		public static readonly FlowBuilder FlowBuilder = new FlowBuilder();

		public bool Start()
		{
			Log.Trace("Starting");

			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			Log.Trace("Assembly Path:" + path);

			//Find all user created FlowFactory types
			var flowFactoryTypes = LoadAssembliesAndGetFlowFactoryTypes(path);
			Log.Trace("Number of flow factories found: " + flowFactoryTypes.Count);

			//Create all factories, we instantiate DefaultFlowFactory on our own so it ends up being called first
			var flowFactories = new List<IFlowFactory> { new DefaultFlowFactory() };
			var factories = CreateFlowFactories(flowFactories, flowFactoryTypes);

			//Create flows, build and register them
			var flows = LetFactoriesCreateFlows(factories);
			Log.Trace("Number of flows found: " + flows);
			BuildAndRegisterFlows(flows);
		

			Task.WaitAll(FlowBuilder.Flows.Select(x => Task.Run(() => x.Start())).ToArray());

			return true;
		}

		public bool Stop()
		{
			Task.WaitAll(FlowBuilder.Flows.Select(x => Task.Run(() => x.Stop())).ToArray());
			return true;
		}

		private static List<Type> LoadAssembliesAndGetFlowFactoryTypes(string path)
		{
			try
			{
				var allAssemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFile).ToList();

				//Find all factories, except DefaultFlowFactory (we will handle it manually)
				var flowFactoryType = typeof(IFlowFactory);
				var defaultFlowFactoryType = typeof(DefaultFlowFactory);
				var flowFactoryTypes = allAssemblies
					.SelectMany(assembly => assembly.GetTypes())
					.Where(type =>
						!type.IsAbstract                                                //Class may not be abstract
						&& type != defaultFlowFactoryType                               //Class must not be DefaultFlowFactory
						&& flowFactoryType.IsAssignableFrom(type)                       //Class must inherit from IFlowFactory
						&& type.GetCustomAttribute<DoNotAutoCreateAttribute>() == null  //Class may not be marked with [DoNotAutoCreate]
					)
					.ToList();
				return flowFactoryTypes;
			}
			catch (ReflectionTypeLoadException exception)
			{
				Log.Error(exception);
				Log.Error(exception.LoaderExceptions);
			}
			return new List<Type>(0);
		}

		private List<IFlowFactory> CreateFlowFactories(List<IFlowFactory> flowFactories, IEnumerable<Type> flowFactoryTypes)
		{
			foreach (var flowfactoryType in flowFactoryTypes)
			{
				try
				{
					var factory = (IFlowFactory)Activator.CreateInstance(flowfactoryType);
					flowFactories.Add(factory);
				}
				catch (Exception exception)
				{
					Log.Error(exception);
				}
			}
			return flowFactories;
		}

		private static List<Flow> LetFactoriesCreateFlows(List<IFlowFactory> factories)
		{
			var flows = factories.SelectMany(LetFactoryCreateFlows).ToList();
			return flows;
		}

		private static IEnumerable<Flow> LetFactoryCreateFlows(IFlowFactory factory)
		{
			try
			{
				var flows = factory.CreateFlows();
				return flows;
			}
			catch (Exception exception)
			{
				Log.Error(exception);
			}
			return Enumerable.Empty<Flow>();
		}

		private static void BuildAndRegisterFlows(IEnumerable<Flow> flows)
		{
			foreach(var flow in flows)
			{
				try
				{
					FlowBuilder.BuildAndRegisterFlow(flow);
				}
				catch (Exception exception)
				{
					Log.Error(exception);
				}
			}
		}
	}
}
