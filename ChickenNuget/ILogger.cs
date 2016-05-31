namespace ChickenNuget
{
    public interface ILogger
    {
        void Indent();
        void Log(string message);
        void Unindent();
    }
}