using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly FileExtensionContentTypeProvider _extensionContentTypeProvider;

    public FilesController(FileExtensionContentTypeProvider extensionContentTypeProvider)
    {
        _extensionContentTypeProvider = extensionContentTypeProvider;
    }

    [HttpGet("{fileId}")]
    public ActionResult GetFile([FromRoute] string fileId)
    {
        const string pathToFile = "appsettings.json";

        if (!System.IO.File.Exists(pathToFile)) return NotFound();

        if (!_extensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            contentType = "application/octet-stream";

        var bytes = System.IO.File.ReadAllBytes(pathToFile);

        return File(bytes, contentType, Path.GetFileName(pathToFile));
    }
}