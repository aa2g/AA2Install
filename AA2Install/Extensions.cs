using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AA2Install
{
    public static class Extensions
    {
        public static DialogResult InvokeMessageBox(this Form form, string text, string caption = null, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
        {
            if (form.InvokeRequired)
            {
                return (DialogResult)form.Invoke(new Func<DialogResult>(() =>
                {
                    return MessageBox.Show(form, text, caption, buttons, icon);
                }));
            }
            else
                return MessageBox.Show(form, text, caption, buttons, icon);
        }
    }
}
