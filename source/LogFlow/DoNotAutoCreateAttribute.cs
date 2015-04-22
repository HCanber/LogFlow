using System;

namespace LogFlow
{
	/// <summary>
	/// Mark a <see cref="Flow"/> or an <see cref="IFlowFactory"/> class with this attribute to 
	/// stop the default factories from creating an instance of the class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class DoNotAutoCreateAttribute : Attribute { }
}