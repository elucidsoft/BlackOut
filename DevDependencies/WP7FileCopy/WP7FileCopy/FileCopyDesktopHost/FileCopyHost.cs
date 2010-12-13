using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.ServiceModel;

using J2i.Net.FileCopyService;


namespace FileCopyDesktopHost
{
    class FileCopyHost : INotifyPropertyChanged, IDisposable
    {
        private ServiceHost _serviceHost = null;
        private bool _isOpen = false;


        public FileCopyHost()
        {
            _serviceHost = new ServiceHost(typeof(CopyService));
            CopyService.FileCopied += new EventHandler<FileCopiedEventArgs>(CopyService_FileCopied);

        }

        void CopyService_FileCopied(object sender, FileCopiedEventArgs e)
        {
            
            FileList.Add(e);
        }

        // TargetDirectory - generated from ObservableField snippet - Joel Ivory Johnson
        private string _targetDirectory;
        public string TargetDirectory
        {
            get { return _targetDirectory; }
            set
            {
                if (_targetDirectory != value)
                {
                    _targetDirectory = value;
                    OnPropertyChanged("TargetDirectory");
                }
            }
        }
        //-----             
        // FileList - generated from ObservableField snippet - Joel Ivory Johnson
        private ObservableCollection<FileCopiedEventArgs> _fileList = new ObservableCollection<FileCopiedEventArgs>();
        public ObservableCollection<FileCopiedEventArgs> FileList
        {
            get { return _fileList; }
            set
            {
                if (_fileList != value)
                {
                    _fileList = value;
                    OnPropertyChanged("FileList");
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

        void EnsureTargetDirectory()
        {
            if (TargetDirectory == null)
            {
                string homeFolder = Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Wp7Files");
                TargetDirectory = homeFolder;
            }
            if (!Directory.Exists(TargetDirectory))
                Directory.CreateDirectory(TargetDirectory);
            CopyService.TargetDirectory = TargetDirectory;
        }

        public void Start()
        {
            EnsureTargetDirectory();
            _serviceHost.Open();
            _isOpen = true;
        }

        public void Stop()
        {
            if(_isOpen)
            {
                _serviceHost.Close();
                _isOpen = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}
