using AzureStorageLibrary.Interfaces;
using AzureStorageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebApp.Controllers
{
    public class TableStorageController : Controller
    {

        private readonly INoSqlStorage<Product> _noSqlStorage;

        public TableStorageController(INoSqlStorage<Product> noSqlStorage)
        {
            _noSqlStorage = noSqlStorage;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
