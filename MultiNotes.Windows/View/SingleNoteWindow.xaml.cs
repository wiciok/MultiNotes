using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using MultiNotes.Model;
using MultiNotes.Windows.Util;
using MultiNotes.Windows.ViewModel;

namespace MultiNotes.Windows.View
{
    /// <summary>
    /// Interaction logic for SingleNoteWindow.xaml
    /// </summary>
    public partial class SingleNoteWindow
    {
        //private ShowDesktop showDesktop;
        public SingleNoteWindow(Note note)
        {
            InitializeComponent();
            DataContext = new SingleNoteWindowViewModel(x => Close(), note);
            this.ShowInTaskbar = false;

            // Manually alter window height and width
            SizeToContent = SizeToContent.Manual;

            // Automatically resize height relative to content
            SizeToContent = SizeToContent.Height;

            //dynamically create color menu elements
            foreach (var el in NoteColors.ColorList)
            {
                var menuItemTmp = new MenuItem()
                {
                    Height = 31,
                    Margin = new Thickness(0, 0, -47.8, 0),
                    Header = new Rectangle()
                    {
                        Fill = el,
                        Height = 25,
                        Width = 150,
                        Stroke = System.Windows.Media.Brushes.Black
                    }
                };
                menuItemTmp.Click += MenuItem_OnClick;

                ColorMenu.Items.Add(menuItemTmp);
            }

            //currently not used
            //showDesktop= new ShowDesktop();
            //showDesktop.AddHook(this);
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                var rectangle = menuItem.Header as Rectangle;
                if (rectangle != null)
                    MainGrid.Background = rectangle.Fill;
            }
        }

        //-------------custom grip code--------------

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        //Attach this to the MouseDown event of your drag control to move the window in place of the title bar
        private void WindowDrag(object sender, MouseButtonEventArgs e) // MouseDown
        {
            ReleaseCapture();
            SendMessage(new WindowInteropHelper(this).Handle,
                0xA1, (IntPtr)0x2, (IntPtr)0);
        }

        //Attach this to the PreviewMousLeftButtonDown event of the grip control in the lower right corner of the form to resize the window
        private void WindowResize(object sender, MouseButtonEventArgs e) //PreviewMousLeftButtonDown
        {
            HwndSource hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
            SendMessage(hwndSource.Handle, 0x112, (IntPtr)61448, IntPtr.Zero);
        }
        //--------------------------------------------

        //------set window position to bottom code----
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOACTIVATE = 0x0010;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public void SetBottom()
        {
            Window window = this;
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }

        //-------------------------------------------

        private void SingleNoteWindow_OnLocationChanged(object sender, EventArgs e)
        {
            //todo: przywrocic to
            //SetBottom();
        }

        //currently not used
        /*//----prevent from showing desktop (ctrl+d)--

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, ShowDesktop.WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

            [DllImport("user32.dll")]
            internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

            [DllImport("user32.dll")]
            internal static extern int GetClassName(IntPtr hwnd, StringBuilder name, int count);
        }

        public class ShowDesktop
        {
            private const uint WINEVENT_OUTOFCONTEXT = 0u;
            private const uint EVENT_SYSTEM_FOREGROUND = 3u;
            private const string WORKERW = "WorkerW";

            public void AddHook(Window window)
            {
                if (IsHooked)
                    return;

                IsHooked = true;

                _delegate = new WinEventDelegate(WinEventHook);
                _hookIntPtr = NativeMethods.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WINEVENT_OUTOFCONTEXT);
                _window = window;
            }

            public void RemoveHook()
            {
                if (!IsHooked)
                    return;

                IsHooked = false;

                NativeMethods.UnhookWinEvent(_hookIntPtr.Value);

                _delegate = null;
                _hookIntPtr = null;
                _window = null;
            }

            private string GetWindowClass(IntPtr hwnd)
            {
                StringBuilder _sb = new StringBuilder(32);
                NativeMethods.GetClassName(hwnd, _sb, _sb.Capacity);
                return _sb.ToString();
            }

            internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

            private void WinEventHook(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
            {
                if (eventType == EVENT_SYSTEM_FOREGROUND)
                {
                    string _class = GetWindowClass(hwnd);
                    if (string.Equals(_class, WORKERW, StringComparison.Ordinal))
                        _window.Topmost = true;
                    else
                        _window.Topmost = false;
                }
            }

            public bool IsHooked { get; private set; } = false;
            private IntPtr? _hookIntPtr { get; set; }
            private WinEventDelegate _delegate { get; set; }
            private Window _window { get; set; }
        }
        //--------------------------------------------*/
    }
}
