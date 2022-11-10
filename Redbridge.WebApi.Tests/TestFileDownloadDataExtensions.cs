using NUnit.Framework;
using Redbridge.Data;
using Redbridge.WebApi.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redbridge.WebApi.Tests
{
    [TestFixture]
    public class TestFileDownloadDataExtensions
    {
        [Test]
        public void AsFileResult()
        {
            FileDownloadData data = new FileDownloadData();
            data.FileName = "test.zip";
            data.ContentType = "application/zip";
            data.FileStream = new System.IO.MemoryStream();
            var result = data.AsFileResult();
            Assert.NotNull(result);
        }

        [Test]
        public void AsZippedFileResult()
        {
            FileDownloadsData data = new FileDownloadsData();
            data.FileName = "test.zip";
            data.Files = new List<FileDownloadData>().ToArray();
            var result = data.AsZippedFileResult();
            Assert.NotNull(result);
        }

        [Test]
        public void ZipContentResult()
        {
            FileDownloadsData downloadData = new FileDownloadsData();
            downloadData.FileName = "test.zip";
            downloadData.Files = new List<FileDownloadData>().ToArray();
            var result = downloadData.AsZippedFileResult();
            Assert.NotNull(result);
        }
    }
}