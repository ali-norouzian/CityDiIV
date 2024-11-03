using CityDiIV.Application.Common.Extensions;
using CityDiIV.Application.Features.SampleDatas.Commands;
using CityDiIV.Domain.Contracts.Persistence;
using Mediator;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

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