using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colorful;
using System.Drawing;

namespace AA2Pack
{
    public static class ConsoleWriter
    {
        private static object _MessageLock = new object();

        public static void ClearLine()
        {
            lock (_MessageLock)
            {
                int currentLineCursor = System.Console.CursorTop;
                System.Console.SetCursorPosition(0, System.Console.CursorTop);
                System.Console.Write(new string(' ', System.Console.WindowWidth));
                System.Console.SetCursorPosition(0, currentLineCursor);
            }
        }

        private static Color _baseColor => Color.LightGray;
        private static Color _accentColor => Color.Purple;
        private static Color _accent2Color => Color.Gold;

        private static void writePartialLine(string message, Color? color = null)
        {
            if (color == null)
                color = _baseColor;

            lock (_MessageLock)
            {
                Colorful.Console.Write(message, color);
            }
        }

        private static void writeLine(string message, Color? color = null)
        {
            if (color == null)
                color = _baseColor;

            lock (_MessageLock)
            {
                Colorful.Console.WriteLine(message, color);
            }
        }

        public static void WriteEvent(string eventMessage, string eventArgs)
        {
            lock (_MessageLock)
            {
                ClearLine();
                writePartialLine(eventMessage);
                writeLine(eventArgs, Color.Purple);
            }
        }

        public static void Write(string text = "")
        {
            Colorful.Console.ForegroundColor = _baseColor;
            Colorful.Console.Write(text);
        }

        public static void WriteLine(string text = "")
        {
            Write(text + Environment.NewLine);
        }
        
        public static void WriteFormat(string format, params string[] args)
        {
            Colorful.Console.WriteFormatted(format, args, _accent2Color, _baseColor);
        }

        public static void WriteLineFormat(string format, params string[] args)
        {
            Colorful.Console.WriteLineFormatted(format, args, _accent2Color, _baseColor);
        }

        public static void WriteSwitch(string _switch, string description)
        {
            WriteLineFormat("{0} | " + description, _switch.PadRight(3));
        }
    }
}
