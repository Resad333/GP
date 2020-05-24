namespace Registration.Core.Abstraction
{
    public interface ILogger
    {
        void Information(string message, params object[] parameters);
        void Warning(string message, params object[] parameters);
        void Debug(string message, params object[] parameters);
        void Error(string message, params object[] parameters);
    }
}
