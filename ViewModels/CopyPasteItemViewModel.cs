using CopyPaste.Commands;

namespace CopyPaste.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Interop;
    using Models;
    using System.Windows.Input;

    public class CopyPasteItemViewModel
    {
        #region Private Fields
        private IntPtr _hWndNextViewer;
        private HwndSource _hWndSource;
        private readonly WindowInteropHelper _wih;
        private readonly ObservableCollection<CopyPasteItem> _list = new ObservableCollection<CopyPasteItem>();
        private ICommand _clearListCommand;
        #endregion

        public CopyPasteItemViewModel(WindowInteropHelper wih)
        {
            this._wih = wih;
        }

        public CopyPasteItem CreateNewCopyPasteItem()
        {
            CopyPasteItem itm = new CopyPasteItem(this);
            return itm;
        }

        public ObservableCollection<CopyPasteItem> List
        {
            get { return _list; }
        }

        public ICommand ClearListCommand
        {
            get { return _clearListCommand = new ClearListCommand(true, this); }
        } 

        #region Clipboard viewer related methods
        public void InitCbViewer()
        {
            _hWndSource = HwndSource.FromHwnd(_wih.Handle);

            if (_hWndSource == null) return;
            _hWndSource.AddHook(this.WinProc);   // start processing window messages 
            _hWndNextViewer = Win32.SetClipboardViewer(_hWndSource.Handle);   // set this window as a viewer 
        }

        public void CloseCbViewer()
        {
            // remove this window from the clipboard viewer chain
            Win32.ChangeClipboardChain(_hWndSource.Handle, _hWndNextViewer);

            _hWndNextViewer = IntPtr.Zero;
            _hWndSource.RemoveHook(this.WinProc);
        }

        private IntPtr WinProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case Win32.WmChangecbchain:
                    if (wParam == _hWndNextViewer)
                    {
                        // clipboard viewer chain changed, need to fix it. 
                        _hWndNextViewer = lParam;
                    }
                    else if (_hWndNextViewer != IntPtr.Zero)
                    {
                        // pass the message to the next viewer. 
                        Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    }
                    break;

                case Win32.WmDrawclipboard:
                    // clipboard content changed
                    CopyPasteItem itm = CreateNewCopyPasteItem();
                    itm.AddToCopyPasteItemsList(_list);
                    // pass the message to the next viewer. 
                    Win32.SendMessage(_hWndNextViewer, msg, wParam, lParam);
                    break;
            }

            return IntPtr.Zero;
        }

        public void ClearList()
        {
            _list.Clear();
        }

        #endregion
    }
}
