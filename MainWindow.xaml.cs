namespace CopyPaste
{
    using System;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Interop;
    using ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private Fields

        private NotifyIcon _ni = new NotifyIcon();
        private readonly CopyPasteItemViewModel _vm;
        #endregion

        #region Constructor
        public MainWindow()
        {
            WindowInteropHelper wih = new WindowInteropHelper(this);
            this._vm = new CopyPasteItemViewModel(wih);
            this.DataContext = _vm;
            InitializeComponent();
        }
        #endregion

        #region Control event handlers

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
            this.notify_tray.Icon = new Icon(@"Icons/test2.ico");
        }

        private void notify_tray_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _vm.CloseCbViewer();
        }

        private void OnLoad(object sender, EventArgs e)
        {
            _vm.InitCbViewer();
        }

        #endregion
    }
}
