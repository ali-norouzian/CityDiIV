using CityDiIV.Application.Common.Extensions;
using CityDiIV.Domain.Contracts.Persistence;
using CityDiIV.Domain.Entities;
using Mediator;
using Microsoft.Net.Http.Headers;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace CityDiIV.Application.Features.SampleDatas.Commands;
public class UploadLargFileCommandHandler : IRequestHandler<UploadLargFileCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public UploadLargFileCommandHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async ValueTask<bool> Handle(UploadLargFileCommand request, CancellationToken cancellationToken)
    {
        var section = await request.reader.ReadNextSectionAsync();

        const int maxLinesPerBatch = 1_000;

        while (section != null)
        {
            if (ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition))
            {
                if (MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
                {
                    // Use StreamReader to read lines from the section.Body stream
                    using var readerStream = new StreamReader(section.Body);

                    int linesRead = 0;
                    string line;
                    var lineBatch = new List<string>(1000);

                    // Read lines in batches of 1,000
                    while ((line = await readerStream.ReadLineAsync()) != null)
                    {
                        lineBatch.Add(line);
                        linesRead++;

                        // Once 1,000 lines are read, write them to the target file
                        if (linesRead >= maxLinesPerBatch)
                        {
                            await SaveData(lineBatch);
                            lineBatch.Clear();
                            linesRead = 0;
                        }
                    }

                    // Write any remaining lines
                    if (lineBatch.Count > 0)
                    {
                        await SaveData(lineBatch);
                    }
                }
            }

            // Move to the next section
            section = await request.reader.ReadNextSectionAsync();
        }

        return true;
    }


    private async Task SaveData(List<string> csvData)
    {
        var sw = new Stopwatch();
        sw.Start();

        var sampleDataList = new ConcurrentBag<SampleData>();
        Parallel.ForEach(csvData,
            new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
            row =>
            {
                var data = row.Split(",");

                try
                {
                    sampleDataList.Add(new()
                    {
                        SampleInt = Convert.ToInt32(data[1]),
                        SampleDecimal = Convert.ToDecimal(data[2]),
                        SampleString = data[3],
                        SampleBool = data[4] == "1" ? true : false,
                    });
                }
                catch (FormatException ex)
                {
                }

            });

        var repo = _uow.GetRepository<SampleData>();
        await repo.Add(sampleDataList.ToList());
        await _uow.SaveChanges();

        sw.Stop();
        Console.WriteLine($"Time taken: {sw.Elapsed}");
    }

}