using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Windows.Storage;
using Windows.Storage.Streams;

namespace WhiteboardApp
{
    class HttpServerInterface
    {
        readonly string rootUrl = "http://104.236.134.122:8888/";

        public async void PostInkFile(StorageFile file)
        {
            Byte[] content = await ReadFile(file);
            HttpClient httpClient = new HttpClient();
            HttpContent httpContent = new ByteArrayContent(content);

            await httpClient.PostAsync(rootUrl + "upload", httpContent);
        }

        public async Task<StorageFile> GetInkFile()
        {
            HttpClient httpClient = new HttpClient();
            Byte[] content = await httpClient.GetByteArrayAsync(rootUrl+"download");

            StorageFile sampleFile = await ApplicationData.Current.RoamingFolder.CreateFileAsync("ink.isf", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteBytesAsync(sampleFile, content);

            return sampleFile;
        }

        private async Task<byte[]> ReadFile(StorageFile file)
        {
            byte[] fileBytes = null;
            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())
            {
                fileBytes = new byte[stream.Size];
                using (DataReader reader = new DataReader(stream))
                {
                    await reader.LoadAsync((uint)stream.Size);
                    reader.ReadBytes(fileBytes);
                }
            }
            return fileBytes;
        }
    }
}
