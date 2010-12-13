using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace J2i.Net.FileCopyService
{
    public class CopyService:ICopyService
    {

        public static event  EventHandler<FileCopiedEventArgs> FileCopied = null;

        protected void OnFileCopied(FileCopiedEventArgs args)
        {
            if(FileCopied!=null)
            {
                FileCopied(this, args);
            }
        }

// TargetDirectory - generated from ObservableField snippet - Joel Ivory Johnson
  private static string _targetDirectory;
  public static string TargetDirectory
  {
    get { return _targetDirectory; }
      set
      {
          if (_targetDirectory != value)
          {
              _targetDirectory = value;
          }
      }
  }
 //-----

        void EnsureTargetDirectorySet()
        {
            if(TargetDirectory==null)
                throw new ArgumentNullException("TargetDirectory", "Target directory must contain a path in which to save the files");
        }
        #region ICopyService Members

        public void CopyString(string fileName, string contents)
        {
            EnsureTargetDirectorySet();
            string targetPath = Path.Combine(TargetDirectory, fileName);
            using(var sw = new StreamWriter(targetPath))
            {
                sw.Write(contents);    
            }

            //A gold star to who ever can identify the bug in the following statement. 
            //note: to see the bug you would most likely have to have non-latin text.
            var copiedArgs = new FileCopiedEventArgs()
                                 {FileName = fileName, FullPath = targetPath, FileSize = contents.Length};
            OnFileCopied(copiedArgs);
        }



        public void CopyBytes(string fileName, byte[] contents)
        {
             EnsureTargetDirectorySet();
            string targetPath = Path.Combine(TargetDirectory, fileName);

            using (var bw = new BinaryWriter(File.Create(targetPath)))
            {
                bw.Write(contents);
            }
            var copiedArgs = new FileCopiedEventArgs()
                                 {FileName = fileName, FullPath = targetPath, FileSize = contents.Length};
            OnFileCopied(args:copiedArgs);
        }

        #endregion
    }
}
