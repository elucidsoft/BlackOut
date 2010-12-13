using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace WP7FileCopyClient
{
    public partial class MainPage : PhoneApplicationPage
    {
        private FileCopyViewModel _viewModel;
        public FileCopyViewModel ViewModel
        {
            get { return _viewModel;  }
            set
            {
                _viewModel = value;
                DataContext = value;
            }
        }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new FileCopyViewModel();
        }

        private void cmdUploadText_Click(object sender, EventArgs e)
        {
            _viewModel.BeginUpload();
        }


    }
}
