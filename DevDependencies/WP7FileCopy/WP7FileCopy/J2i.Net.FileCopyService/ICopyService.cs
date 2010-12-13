using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace J2i.Net.FileCopyService
{
    [ServiceContract]
    public interface ICopyService
    {
        [OperationContract]
        void CopyString(string fileName, string contents);

        [OperationContract]
        void CopyBytes(string fileName, byte[] contents);
    }
}
