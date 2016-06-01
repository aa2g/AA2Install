using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Pack
{
    public static class Extensions
    {
        public static bool ContainsSwitch(this ICollection<string> list, string command)
        {
            return list.Any(x => x.ToLower().TrimStart('-', '/') == command);
        }
    }
}
