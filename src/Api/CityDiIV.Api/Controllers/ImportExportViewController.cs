using Microsoft.AspNetCore.Mvc;

namespace CityDiIV.Api.Controllers
{
    public class ImportExportViewController : Controller
    {
        [HttpGet("upload")]
        public IActionResult UploadLargeFile()
        {
            return View();
        }
    }
}
