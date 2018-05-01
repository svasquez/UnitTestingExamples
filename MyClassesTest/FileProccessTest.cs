using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyClasses;
using System.Configuration;
using System.IO;

namespace MyClassesTest
{
    [TestClass]
    public class FileProccessTest
    {
        #region Constants
        private const string BAD_FILE_NAME = @"C:\NotExists.bad";
        #endregion

        #region Properties
        private string _GoodFileName;
        public TestContext TestContext { get; set; }
        #endregion

        #region SetGoodFileName Method
        public void SetGoodFileName()
        {
            _GoodFileName = ConfigurationManager.AppSettings["GoodFileName"];
            if (_GoodFileName.Contains("[AppPath]"))
            {
                _GoodFileName = _GoodFileName.Replace("[AppPath]",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }
        #endregion

        [TestMethod]
        public void FileNameDoesExist()
        {
            bool fromCall;
            FileProccess fp = new FileProccess();

            SetGoodFileName();
            if (!string.IsNullOrEmpty(_GoodFileName))
            {
                TestContext.WriteLine("Creating file:" + _GoodFileName);
                File.AppendAllText(_GoodFileName, "Some Text");
            }

            TestContext.WriteLine("Checking file: " + _GoodFileName);
            fromCall = fp.FileExists(_GoodFileName);

            //Delete file
            if(File.Exists(_GoodFileName))
            {
                TestContext.WriteLine("Deleting file: " + _GoodFileName);
                File.Delete(_GoodFileName);
            }

            Assert.IsTrue(fromCall);
        }

        [TestMethod]
        public void FileNameDoesNotExist()
        {
            bool fromCall;
            FileProccess fp = new FileProccess();
            TestContext.WriteLine("Checking file: " + BAD_FILE_NAME);
            fromCall = fp.FileExists(BAD_FILE_NAME);
            Assert.IsFalse(fromCall);
        }

        [TestMethod]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException()
        {
            FileProccess fp = new FileProccess();

            try
            {
                fp.FileExists("");
            }
            catch (ArgumentNullException)
            {
                // Test was a success
                return;
            }

            // Fail the test
            Assert.Fail("Call to FileExists() did NOT throw an ArgumentNullException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileNameNullOrEmpty_ThrowsArgumentNullException_UsingAttribute()
        {
            FileProccess fp = new FileProccess();

            fp.FileExists("");
        }

        [TestMethod]
        public void FileNameIsWhiteSpace()
        {
            Assert.Inconclusive();
        }
    }
}
