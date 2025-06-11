using Microsoft.AspNetCore.Http;
using PetFamily.Core.Dtos;

namespace PetFamily.Framework.Abstractions;

public class UploadFileProcess : IAsyncDisposable
{
    private static readonly List<UploadFileDto> _filesDtos = [];

    public IEnumerable<UploadFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var stream = file.OpenReadStream();
            var fileInfoDto = new FileInfoDto(fileName, extension, file.Length);
            var uploadPetFileDto = new UploadFileDto(stream, fileInfoDto);
            _filesDtos.Add(uploadPetFileDto);
        }

        return _filesDtos;
    }

    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDtos)
        {
            await file.FileStream.DisposeAsync();
        }
    }
}