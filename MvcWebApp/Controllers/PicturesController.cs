using AzureStorageLibrary.Interfaces;
using AzureStorageLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using MvcWebApp.Models;

namespace MvcWebApp.Controllers
{
    public class PicturesController : Controller
    {
        public string UserId { get; set; } = "12345";
        public string City { get; set; } = "istanbul";

        private readonly INoSqlStorage<UserPicture> _noSqlStorage;
        private readonly IBlobStorage _blobStorage;
        public PicturesController(INoSqlStorage<UserPicture> noSqlStorage, IBlobStorage blobStorage)
        {
            _noSqlStorage = noSqlStorage;
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult>Index()
        {
            List<FileBlob> fileBlobs = new List<FileBlob>();

            var user = await _noSqlStorage.Get(UserId, City);

            if (user != null)
            {
                user.Paths.ForEach(x =>
                {
                    fileBlobs.Add(new FileBlob { Name = x, Url = $"{_blobStorage.BlobUrl}/{EContainerName.pictures}/{x}" });
                });
            }
            ViewBag.fileBlobs = fileBlobs;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IEnumerable<IFormFile> pictures)
        {
            List<string> picturesList = new List<string>();
            foreach (var item in pictures)
            {
                var newPictureName = $"{Guid.NewGuid()}{Path.GetExtension(item.FileName)}";

                await _blobStorage.UploadAsync(item.OpenReadStream(), newPictureName, EContainerName.pictures);

                picturesList.Add(newPictureName);
            }

            var isUser = await _noSqlStorage.Get(UserId, City);

            if(isUser != null)
            {
                picturesList.AddRange(isUser.Paths);
            }
            else
            {
                isUser = new UserPicture();

                isUser.RowKey = UserId;
                isUser.PartitionKey = City;
                isUser.Paths = picturesList;
            }

            await _noSqlStorage.Add(isUser);

            return RedirectToAction("Index");
        }

    }
}
