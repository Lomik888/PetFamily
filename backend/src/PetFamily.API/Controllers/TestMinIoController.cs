using Microsoft.AspNetCore.Mvc;
using PetFamily.Application.Providers;
using PetFamily.Infrastructure.Providers.MinIo;

namespace PetFamily.API.Controllers;

public class TestMinIoController : ApplicationController
{
    private readonly IFilesProvider _minIoProvider;

    public TestMinIoController(IFilesProvider minIoProvider)
    {
        _minIoProvider = minIoProvider;
    }


    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        await _minIoProvider.UploadAsync("volunteer", Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),
            file.OpenReadStream(), cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromQuery] string bucketName,
        [FromQuery] string userId,
        [FromQuery] string objectName,
        CancellationToken cancellationToken)
    {
        await _minIoProvider.RemoveAsync(bucketName, userId, objectName, cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string bucketName,
        [FromQuery] string userId,
        [FromQuery] string objectName,
        CancellationToken cancellationToken)
    {
        var response = await _minIoProvider.PresignedGetAsync(bucketName, userId, objectName, cancellationToken);

        return Ok(response);
    }
}