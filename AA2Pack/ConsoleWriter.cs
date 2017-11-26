using System;

namespace AA2Pack
{
    public static class ConsoleWriter
    {
        private static object _MessageLock = new object();

        public static void ClearLine()
        {
            lock (_MessageLock)
            {
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, currentLineCursor);
            }
        }

        private static ConsoleColor _baseColor => ConsoleColor.Gray;
        private static ConsoleColor _accentColor => ConsoleColor.DarkMagenta;
        private static ConsoleColor _accent2Color => ConsoleColor.Yellow;

        private static void writePartialLine(string message, ConsoleColor? color = null)
        {
            if (color == null)
                color = _baseColor;

            lock (_MessageLock)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write(message);
            }
        }

        private static void writeLine(string message, ConsoleColor? color = null)
        {
            if (color == null)
                color = _baseColor;

            lock (_MessageLock)
            {
                Console.ForegroundColor = (ConsoleColor)color;
                Console.Write(message);
            }
        }

        public static void Write(string text = "")
        {
            Console.ForegroundColor = _baseColor;
            Console.Write(text);
        }

        public static void WriteLine(string text = "")
        {
            Write(text + Environment.NewLine);
        }

        public static void WriteAlternating(params string[] args)
        {
            bool isColored = false;
            foreach (string text in args)
            {
                if (isColored)
                    Console.ForegroundColor = _accent2Color;
                else
                    Console.ForegroundColor = _baseColor;

                isColored = !isColored;

                Console.Write(text);
            }
        }

        public static void WriteLineAlternating(params string[] args)
        {
            bool isColored = false;
            foreach (string text in args)
            {
                if (isColored)
                    Console.ForegroundColor = _accent2Color;
                else
                    Console.ForegroundColor = _baseColor;

                isColored = !isColored;

                Console.Write(text);
            }

            Console.WriteLine();
        }

        public static void WriteSwitch(string _switch, string description)
        {
            WriteLineAlternating(_switch.PadRight(3), " | " + description);
        }
    }
}
