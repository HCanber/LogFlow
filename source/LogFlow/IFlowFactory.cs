using System.Collections.Generic;

namespace LogFlow
{
	/// <summary>
	/// Responsible for creating Flows. Every class implementing this interface will be automatically
	/// created and called, unless the class is marked with <see cref="DoNotAutoCreateAttribute"/>.
	/// </summary>
	public interface IFlowFactory
	{
		/// <summary>Creates flows given a collection of loaded assemblies.</summary>
		IEnumerable<Flow> CreateFlows();
	}
}