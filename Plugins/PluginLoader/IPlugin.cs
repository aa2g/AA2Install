using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginLoader
{
    public enum PluginType
    {
        StartupScript,
        MenuStripButton,
        UserControl
    }

    public delegate void Method();

    public class MenuStripMethod
    {
        public string Text;
        public Method Method;

        public MenuStripMethod(string text, Method method)
        {
            Text = text;
            Method = method;
        }
    }

    public class UserControlMethod
    {
        public string Text;
        public UserControl Method;

        public UserControlMethod(string text, UserControl control)
        {
            Text = text;
            Method = control;
        }
    }

    public interface IPlugin
    {
        string Name { get; }
        Version Version { get; }
        PluginType Type { get; }
        object Payload { get; }
    }
}
