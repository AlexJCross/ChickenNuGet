using System;

namespace ChickenNuget
{
    public class Logger : ILogger
    {
        private const int Level = 3;
        private int indentation;
        public void Log(string message)
        {
            var output = new string (' ', this.indentation) + message;
            Console.WriteLine(output);
        }

        public void Unindent()
        {
            this.indentation = Math.Max(0, this.indentation - Level);
        }

        public void Indent()
        {
            this.indentation += Level;
        }
    }
}