using Microsoft.AspNetCore.Mvc;

namespace CityDiIV.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExportApiController : ControllerBase
    {
        public async Task<IActionResult> Export()
        {
            // todo: get data from db
            var memoryStream = new MemoryStream();

            // todo: generate file type. csv or excel
            memoryStream.Position = 0; // Reset stream to beginning

            // Let the framework manage the stream's lifecycle
            return File(memoryStream, "application/octet-stream", $"{DateTime.UtcNow.AddHours(3.5)}.unk");
        }

        [HttpPost("import-data")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            //IEnumerable<WalletLog> data;
            await using (var stream = file.OpenReadStream())
            {
                stream.Position = 0;
                // todo: bring data
                //data = await JsonSerializer.DeserializeAsync<List<WalletLog>>(stream, options: _jsonSerializerOptions);
            }
            //await _dbContext.AddRangeAsync(data);
            //await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
