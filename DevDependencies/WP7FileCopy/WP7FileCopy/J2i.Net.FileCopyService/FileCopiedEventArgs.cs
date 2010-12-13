using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace J2i.Net.FileCopyService
{
    public class FileCopiedEventArgs :EventArgs,  INotifyPropertyChanged
    {


        // FileSize - generated from ObservableField snippet - Joel Ivory Johnson
        private int _fileSize;
        public int FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    OnPropertyChanged("FileSize");
                }
            }
        }
        //-----     
        // FullPath - generated from ObservableField snippet - Joel Ivory Johnson
        private string _fullPath;
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                if (_fullPath != value)
                {
                    _fullPath = value;
                    OnPropertyChanged("FullPath");
                }
            }
        }
        //-----             
        // FileName - generated from ObservableField snippet - Joel Ivory Johnson
        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                }
            }
        }
        //-----
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
