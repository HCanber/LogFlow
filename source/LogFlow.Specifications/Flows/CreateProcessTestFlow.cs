using LogFlow.Specifications.Helpers;

namespace LogFlow.Specifications.Flows
{
	public class CreateProcessTestFlow : Flow
	{
		public CreateProcessTestFlow()
		{
			CreateProcess()
				.FromInput(new EmptyInput())
				.Then(new TestProcessor())
				.ToOutput(new ReportToCurrentResultOutput());
		}

		public CreateProcessTestFlow(string logType)
		{
			CreateProcess(logType)
				.FromInput(new EmptyInput())
				.Then(new TestProcessor())
				.ToOutput(new ReportToCurrentResultOutput());
		}

		public CreateProcessTestFlow(string logType, string name)
		{
			CreateProcess(logType, name)
				.FromInput(new EmptyInput())
				.Then(new TestProcessor())
				.ToOutput(new ReportToCurrentResultOutput());
		}
	}
}