using Mediator;
using Microsoft.AspNetCore.WebUtilities;

namespace CityDiIV.Application.Features.SampleDatas.Commands;
public record UploadLargFileCommand(MultipartReader reader) : IRequest<bool>;

