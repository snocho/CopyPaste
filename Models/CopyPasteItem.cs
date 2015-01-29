namespace CopyPaste.Models
{
    using CopyPaste.Commands;
    using CopyPaste.ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    public class CopyPasteItem : INotifyPropertyChanged
    {
        private string _Date;
        private string _Content;
        private string _ContentToolTip;
        private int _Index;
        private ICommand _copyItemCommand;
        public static CopyPasteItemViewModel _cpivm;

        public CopyPasteItem(string Date, string Content, string ContentToolTip, int Index)
        {
            this._Date = Date;
            this._Content = Content;
            this._ContentToolTip = ContentToolTip;
            this._Index = Index;
        }

        public CopyPasteItem(CopyPasteItemViewModel cpivm)
        {
            _cpivm = cpivm;
        }

        public ICommand CopyItemCommand
        {
            get { return _copyItemCommand = new CopyItemFromListCommand(true, this); }
        }

        public void CopyItemFromList(string content)
        {
            _cpivm.CloseCBViewer();
            System.Windows.Clipboard.SetText(content);
            _cpivm.InitCBViewer();
        }

        public string Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                OnPropertyChanged("Content");
            }
        }

        public string ContentToolTip
        {
            get { return _ContentToolTip; }
            set
            {
                _ContentToolTip = value;
                OnPropertyChanged("ContentToolTip");
            }
        }

        public int Index
        {
            get { return _Index; }
            set
            {
                _Index = value;
                OnPropertyChanged("Index");
            }
        }

        public void AddToCopyPasteItemsList(ObservableCollection<CopyPasteItem> list)
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                this.Date = "#" + (list.Count + 1).ToString() + "@" + DateTime.Now.ToString("HH:mm:ss tt");
                this.Index = list.Count + 1;
                this.Content = System.Windows.Clipboard.GetText();
                this.ContentToolTip = this.Content.Substring(0, this.Content.Length > 255 ? 255 : this.Content.Length);
                list.Insert(0, this);
            }

            if (System.Windows.Clipboard.ContainsAudio() || System.Windows.Clipboard.ContainsFileDropList() || System.Windows.Clipboard.ContainsImage())
            {
                //_itm = new CopyPasteItem();
                this.Date = "#" + (list.Count + 1).ToString() + "@" + DateTime.Now.ToString("HH:mm:ss tt");
                this.Index = list.Count + 1;
                this.Content = "The type of the data in the clipboard is not supported by this sample.";
                list.Insert(0, this);
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
