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
            _blobStorage = blobStorage ?? throw new ArgumentNullException(nameof(blobStorage), "Blob storage is not initialized");

        }


        public async Task<IActionResult> Index()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/{x}" }).ToList();

            ViewBag.logs = await _blobStorage.GetLogAsync("controller.txt");
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] IFormFile picture)
        {
            await _blobStorage.SetLogAsync("Upload methoduna giriş yapıldı", "controller.txt");

            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName, EContainerName.pictures);

            await _blobStorage.SetLogAsync("Upload methodundan çıkış yapıldı", "controller.txt");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var stream = await _blobStorage.DownloadAsync(fileName, EContainerName.pictures);

            await _blobStorage.SetLogAsync("Dosya indirildi: " + fileName, "controller.txt");

            return File(stream, "application/octet-stream", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _blobStorage.DeleteAsync(fileName, EContainerName.pictures);

            await _blobStorage.SetLogAsync("Dosya silindi: " + fileName, "controller.txt");

            return RedirectToAction("Index");
        }

    }
}
