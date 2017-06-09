using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace MultiNotes.Windows.Util
{
    public static class MinimizeToTray
    {
        public static void Enable(Window window)
        {
            new MinimizeToTrayInstance(window);
        }

        private class MinimizeToTrayInstance
        {
            private readonly Window _window;
            private NotifyIcon _notifyIcon;
            private bool _balloonShown;

            public MinimizeToTrayInstance(Window window)
            {
                Debug.Assert(window != null, "window parameter is null.");
                _window = window;
                _window.StateChanged += HandleStateChanged;
            }

            private void HandleStateChanged(object sender, EventArgs e)
            {
                if (_notifyIcon == null)
                {
                    // Initialize NotifyIcon instance "on demand"
                    _notifyIcon = new NotifyIcon
                    {
                        Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location)
                    };
                    _notifyIcon.MouseClick += HandleNotifyIconOrBalloonClicked;
                    _notifyIcon.BalloonTipClicked += HandleNotifyIconOrBalloonClicked;
                }
                // Update copy of Window Title in case it has changed
                _notifyIcon.Text = _window.Title;

                // Show/hide Window and NotifyIcon
                var minimized = _window.WindowState == WindowState.Minimized;
                _window.ShowInTaskbar = !minimized;
                _notifyIcon.Visible = minimized;
                if (minimized && !_balloonShown)
                {
                    // If this is the first time minimizing to the tray, show the user what happened
                    _notifyIcon.ShowBalloonTip(500, null, _window.Title, ToolTipIcon.None);
                    _balloonShown = true;
                }
            }

            private void HandleNotifyIconOrBalloonClicked(object sender, EventArgs e)
            {
                // Restore the Window
                _window.WindowState = WindowState.Normal;
            }
        }
    }
}
