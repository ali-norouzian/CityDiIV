using CityDiIV.Application.Common.Extensions;
using CityDiIV.Application.Features.SampleDatas.Commands;
using CityDiIV.Domain.Contracts.Persistence;
using CityDiIV.Domain.Entities;
using Mediator;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;

namespace CityDiIV.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImportExportApiController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMediator _mediator;
    public ImportExportApiController(IUnitOfWork uow, IMediator mediator)
    {
        _uow = uow;
        _mediator = mediator;
    }

    [HttpGet("generate-sample-data")]
    public async Task<IActionResult> GenSampleData()
    {
        var sampleDataList = new List<SampleData>(); ;
        for (int i = 0; i < 10000; i++)
        {
            sampleDataList.Add(new()
            {
                SampleBool = i % 2 == 0,
                SampleInt = i,
                SampleDecimal = (decimal)i / 1000,
                SampleString = i.ToString()
            });
        }

        var sp = new Stopwatch();
        sp.Start();

        var repo = _uow.GetRepository<SampleData>();
        await repo.Add(sampleDataList);
        await _uow.SaveChanges();

        sp.Stop();

        return Ok(sp.Elapsed);
    }

    [HttpGet("export-data")]
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



    [HttpPost("UploadLargeFile")]
    public async Task<IActionResult> UploadLargeFile()
    {
        var request = HttpContext.Request;

        if (!request.HasFormContentType)
        {
            return BadRequest("Unsupported media type");
        }

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(request.ContentType),
            FormOptions.DefaultMultipartBoundaryLengthLimit);

        var reader = new MultipartReader(boundary, request.Body);

        var command = new UploadLargFileCommand(reader);
        await _mediator.Send(command);

        return Ok("File uploaded successfully.");
    }

}