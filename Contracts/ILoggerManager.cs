namespace Contracts;
public interface ILoggerManager
{
	public void LogInfo(string message);
	public void LogWarn(string message);
	public void LogDebug(string message);
	public void LogError(string message);
}
