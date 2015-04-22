using Machine.Specifications;

namespace LogFlow.Specifications.Flows
{
	public class FlowSpecs
	{
		[Subject(typeof(Flow))]
		public class when_calling_CreateProcess_with_no_parameters
		{
			static Flow flow = new CreateProcessTestFlow();

			private It should_have_type_as_logType
					= () => flow.LogType.ShouldEqual(flow.GetType().Name);

			private It should_have_type_as_name
					= () => flow.Name.ShouldEqual(flow.GetType().Name);
		}

		[Subject(typeof(Flow))]
		public class when_calling_CreateProcess_with_logType
		{
			private const string TheLogType = "TheLogType";
			static Flow flow = new CreateProcessTestFlow(TheLogType);

			private It should_have_logType_as_logType
					= () => flow.LogType.ShouldEqual(TheLogType);

			private It should_have_logType_as_name
					= () => flow.Name.ShouldEqual(TheLogType);
		}


		[Subject(typeof(Flow))]
		public class when_calling_CreateProcess_with_logType_and_null_name
		{
			private const string TheLogType = "TheLogType";
			static Flow flow = new CreateProcessTestFlow(TheLogType, null);

			private It should_have_logType_as_logType
					= () => flow.LogType.ShouldEqual(TheLogType);

			private It should_have_logType_as_name
					= () => flow.Name.ShouldEqual(TheLogType);
		}

		[Subject(typeof(Flow))]
		public class when_calling_CreateProcess_with_logType_and_name
		{
			private const string TheLogType = "TheLogType";
			private const string TheName = "TheName";
			static Flow flow = new CreateProcessTestFlow(TheLogType, TheName);

			private It should_have_logType_as_logType
					= () => flow.LogType.ShouldEqual(TheLogType);

			private It should_have_name_as_name
					= () => flow.Name.ShouldEqual(TheName);
		}
	}
}