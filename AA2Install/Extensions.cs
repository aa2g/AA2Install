using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static void SemiAsyncWait(this Action action)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                action();
            };
            bw.SemiAsyncWait();
        }

        public static void SemiAsyncWait(this BackgroundWorker bw)
        {
            bw.RunWorkerAsync();

            while (bw.IsBusy)
                Application.DoEvents();
        }

        public static void SemiAsyncWait(this Task task)
        {
            if (task.Status == TaskStatus.Created)
                task.Start();

            while (!(task.IsCompleted || task.IsCanceled || task.IsFaulted))
                Application.DoEvents();
        }
    }
}
