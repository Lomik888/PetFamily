using PetFamily.Application.Contracts.DTO.PetDtos;

namespace PetFamily.API.Contracts;

public class UploadPetFileProcess : IAsyncDisposable
{
    private static readonly List<UploadPetFileDto> _filesDtos = [];

    public List<UploadPetFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var stream = file.OpenReadStream();
            var fileInfoDto = new FileInfoDto(fileName, extension, file.Length);
            var uploadPetFileDto = new UploadPetFileDto(stream, fileInfoDto);
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