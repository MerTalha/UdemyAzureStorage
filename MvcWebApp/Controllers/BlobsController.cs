using AzureStorageLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MvcWebApp.Models;

namespace MvcWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;

        }


        public IActionResult Index()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob { Name = "pictures", Url = $"{blobUrl}" }).ToList();

            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] IFormFile formFile)
        {
            var newFileName = Guid.NewGuid().ToString();

            await _blobStorage.UploadAsync(formFile.OpenReadStream(), newFileName, EContainerName.pictures);

            return RedirectToAction("Index");
        } 


    }
}
