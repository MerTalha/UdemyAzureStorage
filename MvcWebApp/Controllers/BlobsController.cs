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

            ViewBag.Blobs = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/x" }).ToList();

            return View();


        }
    }
}
