using System.Linq;
using LogFlow.Specifications.Flows;
using Machine.Specifications;

namespace LogFlow.Specifications
{
    [Subject(typeof(Flow))]
    public class when_adding_a_working_flow
    {
        static FlowBuilder builder = new FlowBuilder();

        Establish context = () => builder.BuildAndRegisterFlow(new WorkingFlow());

        private It should_be_saved
            = () => builder.Flows.Count.ShouldEqual(1);
    }

    [Subject(typeof(Flow))]
    public class when_adding_an_empty_flow
    {
        static FlowBuilder builder = new FlowBuilder();

        Establish context = () => builder.BuildAndRegisterFlow(new EmptyFlow());

        private It should_not_be_saved
            = () => builder.Flows.Count.ShouldEqual(0);
    }

    [Subject(typeof(Flow))]
    public class when_adding_a_flow_without_a_name
    {
        static FlowBuilder builder = new FlowBuilder();

        Establish context = () => builder.BuildAndRegisterFlow(new FlowWithoutName());

        private It should_be_saved
            = () => builder.Flows.Count.ShouldEqual(1);

        private It should_be_named_after_the_type
            = () => builder.Flows.Single().LogType.ShouldEqual("FlowWithoutName");
    }

    [Subject(typeof(Flow))]
    public class when_adding_a_flow_without_a_input
    {
        static FlowBuilder builder = new FlowBuilder();

        Establish context = () => builder.BuildAndRegisterFlow(new FlowWithoutInput());

        private It should_not_be_saved
            = () => builder.Flows.Count.ShouldEqual(0);
    }

    [Subject(typeof(Flow))]
    public class when_adding_a_flow_without_a_process
    {
        static FlowBuilder builder = new FlowBuilder();

        Establish context = () => builder.BuildAndRegisterFlow(new FlowWithoutProcess());

        private It should_not_be_saved
            = () => builder.Flows.Count.ShouldEqual(0);
    }

		[Subject(typeof(Flow))]
		public class when_adding_a_the_same_flow_twice
		{
			static FlowBuilder builder = new FlowBuilder();

			Establish context = () =>
			{
				var workingFlow = new WorkingFlow();
				builder.BuildAndRegisterFlow(workingFlow);
				builder.BuildAndRegisterFlow(workingFlow);
			};

			private It should_not_be_allowed
					= () => builder.Flows.Count.ShouldEqual(1);
		}


		[Subject(typeof(Flow))]
		public class when_adding_different_flows_with_the_same_name
		{
			static FlowBuilder builder = new FlowBuilder();

			Establish context = () =>
			{
				builder.BuildAndRegisterFlow(new WorkingFlow("LogType1","FlowName"));
				builder.BuildAndRegisterFlow(new WorkingFlow("LogType2","FlowName"));
			};

			private It should_not_be_allowed
					= () => builder.Flows.Count.ShouldEqual(1);
		}

		[Subject(typeof(Flow))]
		public class when_adding_flows_for_same_logType_but_with_different_names
		{
			static FlowBuilder builder = new FlowBuilder();

			Establish context = () =>
			{
				builder.BuildAndRegisterFlow(new WorkingFlow("LogType1", "FlowName"));
				builder.BuildAndRegisterFlow(new WorkingFlow("LogType2", "DifferentFlowName"));
			};

			private It should_be_allowed
					= () => builder.Flows.Count.ShouldEqual(2);
		}

}
