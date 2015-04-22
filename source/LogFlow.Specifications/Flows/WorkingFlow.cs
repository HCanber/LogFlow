using LogFlow.Specifications.Helpers;

namespace LogFlow.Specifications.Flows
{
	public class WorkingFlow : Flow
	{
		public WorkingFlow(string logType = "TestProcessor", string name = null)
		{
			CreateProcess(logType, name)
				.FromInput(new EmptyInput())
				.Then(new TestProcessor())
				.ToOutput(new ReportToCurrentResultOutput());
		}
	}
}