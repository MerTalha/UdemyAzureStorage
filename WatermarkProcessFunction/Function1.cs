using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using AzureStorageLibrary;
using AzureStorageLibrary.Interfaces;
using AzureStorageLibrary.Models;
using AzureStorageLibrary.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace WatermarkProcessFunction
{
    public class Function1
    {
        [FunctionName("Function1")]
        public async static Task Run([QueueTrigger("watermarkqueue", Connection = "QueueConnection")]PictureWatermarkQueue myQueueItem, ILogger log)
        {
            ConnectionStrings.AzureStorageConnectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";

            IBlobStorage blobStorage = new BlobStorage();

            INoSqlStorage<UserPicture> noSqlStorage = new TableStorage<UserPicture>();

            foreach (var item in myQueueItem.Picture)
            {
                using var stream = await blobStorage.DownloadAsync(item, EContainerName.pictures);

                using var memoryStream = AddWatermark(myQueueItem.WatermarkText,stream);

                await blobStorage.UploadAsync(memoryStream, item, EContainerName.watermarkpictures);

                log.LogInformation($"{item} resmine watermark ekleme iþlemi yapýlmýþtýr.");
            }

            var userpPicture = await noSqlStorage.Get(myQueueItem.UserId, myQueueItem.City);

            if(userpPicture.WatermarkRawPaths != null)
            {
                myQueueItem.Picture.AddRange(userpPicture.WatermarkPaths);
            }
            userpPicture.WatermarkPaths = myQueueItem.Picture;

            await noSqlStorage.Add(userpPicture);



        }

        public static MemoryStream AddWatermark(string watermarkText, Stream PictureStream)
        {
            MemoryStream ms = new MemoryStream();

            using (Image image = Bitmap.FromStream(PictureStream))
            {
                using (Bitmap tempBitmap = new Bitmap(image.Width, image.Height))
                {
                    using (Graphics gph = Graphics.FromImage(tempBitmap))
                    {
                        gph.DrawImage(image, 0, 0);

                        var font = new Font(FontFamily.GenericSansSerif, 25, FontStyle.Bold);

                        var color = Color.FromArgb(255, 0, 0);

                        var brush = new SolidBrush(color);

                        var point = new Point(20, image.Height - 50);

                        gph.DrawString(watermarkText, font, brush, point);

                        tempBitmap.Save(ms, ImageFormat.Png);
                    }
                } 
            }
            ms.Position = 0;

            return ms;
        }


    }
}
