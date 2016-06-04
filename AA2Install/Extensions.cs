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
        /// <summary>
        /// Shows a message box on the thread of the form.
        /// </summary>
        /// <param name="form">The form to display the message box on.</param>
        /// <param name="text">The text of the message box.</param>
        /// <param name="caption">The caption of the message box.</param>
        /// <param name="buttons">The buttons on the message box.</param>
        /// <param name="icon">The icon on the message box.</param>
        /// <returns>Result of the message box.</returns>
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

        /// <summary>
        /// Invokes a control with a method only if the invoke is required.
        /// </summary>
        /// <param name="control">The control to invoke.</param>
        /// <param name="action">The action to invoke with.</param>
        public static void HybridInvoke(this Control control, Action action)
        {
            if (control.InvokeRequired)
                control.BeginInvoke(action);
            else
                action();
        }

        /// <summary>
        /// Keeps a thread responsive while waiting for a blocking call.
        /// </summary>
        /// <param name="action">The call to unblock.</param>
        public static void SemiAsyncWait(this Action action)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) => {
                action();
            };
            bw.SemiAsyncWait();
        }

        /// <summary>
        /// Keeps a thread responsive while waiting for a blocking call.
        /// </summary>
        /// <param name="bw">The background worker to wait for.</param>
        public static void SemiAsyncWait(this BackgroundWorker bw)
        {
            bw.RunWorkerAsync();

            while (bw.IsBusy)
                Application.DoEvents();
        }

        /// <summary>
        /// Keeps a thread responsive while waiting for a blocking call.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        public static void SemiAsyncWait(this Task task)
        {
            if (task.Status == TaskStatus.Created)
                task.Start();

            while (!(task.IsCompleted || task.IsCanceled || task.IsFaulted))
                Application.DoEvents();
        }

        /// <summary>
        /// Keeps a thread responsive while waiting for a blocking call.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        public static T SemiAsyncWait<T>(this Task<T> task)
        {
            if (task.Status == TaskStatus.Created)
                task.Start();

            while (!(task.IsCompleted || task.IsCanceled || task.IsFaulted))
                Application.DoEvents();

            return task.Result;
        }

        /// <summary>
        /// Keeps a thread responsive while waiting for a blocking call.
        /// </summary>
        /// <param name="task">The task to wait for.</param>
        public static T SemiAsyncWait<T>(this Func<T> func)
        {
            return new Task<T>(func).SemiAsyncWait();
        }
    }
}
