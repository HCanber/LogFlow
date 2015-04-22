using LogFlow.Storage;
using NLog.Config;

namespace LogFlow
{
	public class LogContext
	{
		public LogContext(string logType, string name=null) : this(logType, new BinaryRangeStateStorage(logType),name) { }

		public LogContext(string logType, IStateStorage storage, string name=null)
		{
			LogType = logType;
			Storage = storage;
			Name = name ?? logType;
		}

		public string LogType { get; private set; }
		public string Name { get; private set; }
		public IStateStorage Storage { get; private set; }
	}
}