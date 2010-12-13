using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using WP7FileCopyClient.MyCopyService;

namespace WP7FileCopyClient
{
    public static class Uploader
    {
        public static void Upload(byte[] bytes)
        {
            string serviceAddress = string.Format("http://{0}:{1}/FileCopy/", "eric-pc", "8001");
            ChannelFactory<ICopyService> myChannelFactory = new ChannelFactory<ICopyService>(new BasicHttpBinding(), new EndpointAddress(serviceAddress));
            var copyServiceClient = new CopyServiceClient(new BasicHttpBinding(), new EndpointAddress(serviceAddress));
            copyServiceClient.CopyBytesAsync("levels.dat", bytes);
        }
    }
}
