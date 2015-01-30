using System.Globalization;

namespace CopyPaste.Models
{
    using Commands;
    using ViewModels;
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    public class CopyPasteItem : INotifyPropertyChanged
    {
        private string _date;
        private string _content;
        private string _contentToolTip;
        private int _index;
        private ICommand _copyItemCommand;
        public static CopyPasteItemViewModel Cpivm;

        public CopyPasteItem(string date, string content, string contentToolTip, int index)
        {
            this._date = date;
            this._content = content;
            this._contentToolTip = contentToolTip;
            this._index = index;
        }

        public CopyPasteItem(CopyPasteItemViewModel cpivm)
        {
            Cpivm = cpivm;
        }

        public ICommand CopyItemCommand
        {
            get { return _copyItemCommand = new CopyItemFromListCommand(true, this); }
        }

        public void CopyItemFromList(string content)
        {
            Cpivm.CloseCbViewer();
            System.Windows.Clipboard.SetText(content);
            Cpivm.InitCbViewer();
        }

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }

        public string ContentToolTip
        {
            get { return _contentToolTip; }
            set
            {
                _contentToolTip = value;
                OnPropertyChanged("ContentToolTip");
            }
        }

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
            }
        }

        public void AddToCopyPasteItemsList(ObservableCollection<CopyPasteItem> list)
        {
            if (!System.Windows.Clipboard.ContainsText()) return;
            var content = System.Windows.Clipboard.GetText();
            if (list.Count == 0)
            {
                this.Date = "#" + (list.Count + 1).ToString(CultureInfo.InvariantCulture) + "@" +
                            DateTime.Now.ToString("HH:mm:ss tt");
                this.Index = list.Count + 1;
                this.Content = content;
                this.ContentToolTip = content.Substring(0, this.Content.Length > 255 ? 255 : content.Length);
                list.Insert(0, this);
                return;
            }
            if (list[0].Content.Equals(content)) return;
            this.Date = "#" + (list.Count + 1).ToString(CultureInfo.InvariantCulture) + "@" +
                        DateTime.Now.ToString("HH:mm:ss tt");
            this.Index = list.Count + 1;
            this.Content = content;
            this.ContentToolTip = content.Substring(0, this.Content.Length > 255 ? 255 : content.Length);
            list.Insert(0, this);
            return;
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
