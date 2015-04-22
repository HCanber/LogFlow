using LogFlow.Storage;
using NLog.Config;

namespace LogFlow
{
	public class LogContext
	{
		public LogContext(string logType, string name=null) : this(logType, null, name) { }

		public LogContext(string logType, IStateStorage storage, string name=null)
		{
			name = name ?? logType;
			LogType = logType;
			Storage = storage ?? new BinaryRangeStateStorage(name);
			Name = name;
		}

		public string LogType { get; private set; }
		public string Name { get; private set; }
		public IStateStorage Storage { get; private set; }
	}
}