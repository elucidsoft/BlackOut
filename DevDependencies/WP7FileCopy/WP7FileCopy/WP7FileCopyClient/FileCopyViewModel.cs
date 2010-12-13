using System;
using System.ComponentModel;
using System.Net;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WP7FileCopyClient.MyCopyService;

namespace WP7FileCopyClient
{
    public class FileCopyViewModel : INotifyPropertyChanged
    {


        public FileCopyViewModel()
        {
            
        }

        public void BeginUpload()
        {
            if (IsUploading)
                return;
            string serviceAddress = string.Format("http://{0}:{1}/FileCopy/", this.MachineName, TargetPort);
            ChannelFactory<ICopyService> myChannelFactory = new ChannelFactory<ICopyService>(new BasicHttpBinding(), new EndpointAddress(serviceAddress));

            //_copyServiceClient = myChannelFactory.CreateChannel(new EndpointAddress(serviceAddress));


            var copyServiceClient = new CopyServiceClient(new BasicHttpBinding(), new EndpointAddress(serviceAddress));
              
            if(UseBinary)
            {
                var encoder = System.Text.UnicodeEncoding.Unicode;
                var byteList = encoder.GetBytes(Content);
                copyServiceClient.CopyBytesCompleted += new EventHandler<AsyncCompletedEventArgs>(copyServiceClient_CopyBytesCompleted);
                copyServiceClient.CopyBytesAsync(FileName, byteList);
            }
            else
            {
                IsUploading = true;
                copyServiceClient.CopyStringCompleted += new EventHandler<AsyncCompletedEventArgs>(copyServiceClient_CopyStringCompleted);
                copyServiceClient.CopyStringAsync(FileName,Content);
            }
        }

        void copyServiceClient_CopyBytesCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsUploading=false;
        }

        void copyServiceClient_CopyStringCompleted(object sender, AsyncCompletedEventArgs e)
        {
            IsUploading = false;
        }
        

                
// IsUploading - generated from ObservableField snippet - Joel Ivory Johnson
  private bool _isUploading;
  public bool IsUploading
  {
    get { return _isUploading; }
      set
      {
          if (_isUploading != value)
          {
              _isUploading = value;
              OnPropertyChanged("IsUploading");
          }
      }
  }
 //-----
        // UseBinary - generated from ObservableField snippet - Joel Ivory Johnson
        private bool _useBinary;
        public bool UseBinary
        {
            get { return _useBinary; }
            set
            {
                if (_useBinary != value)
                {
                    _useBinary = value;
                    OnPropertyChanged("UseBinary");
                }
            }
        }
        //-----     

        // IsReadyForUpload - generated from ObservableField snippet - Joel Ivory Johnson
        public bool IsReadyForUpload
        {
            get
            {
                return (
                 (!String.IsNullOrEmpty(Content)) &&
                 (!String.IsNullOrEmpty(FileName)) &&
                 (!String.IsNullOrEmpty(MachineName))
                );
            }

        }
        //-----

        // Content - generated from ObservableField snippet - Joel Ivory Johnson
        private string _content = String.Empty;
        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnPropertyChanged("Content");
                    OnPropertyChanged("IsReadyForUpload");
                }
            }
        }
        //-----
        // MachineNAme - generated from ObservableField snippet - Joel Ivory Johnson
        private string _machineName = "mercury";
        public string MachineName
        {
            get { return _machineName; }
            set
            {
                if (_machineName != value)
                {
                    _machineName = value;
                    OnPropertyChanged("MachineName");
                    OnPropertyChanged("IsReadyForUpload");
                }
            }
        }
        //-----               
        // TargetPort - generated from ObservableField snippet - Joel Ivory Johnson
        private int _targetPort = 8001;
        public int TargetPort
        {
            get { return _targetPort; }
            set
            {
                if (_targetPort != value)
                {
                    _targetPort = value;
                    OnPropertyChanged("TargetPort");
                }
            }
        }
        //-----       
        // FileName - generated from ObservableField snippet - Joel Ivory Johnson
        private string _fileName = "myFile.txt";
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("IsReadyForUpload");
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
