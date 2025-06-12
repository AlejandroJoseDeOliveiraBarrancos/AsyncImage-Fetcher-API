using AsyncImage_Fetcher_Service.Logic.Images.Dtos;

namespace AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces
{
    public interface IImageRepository
    {
        Task SaveImageAsync(ImageToSaveDto imageToSave, CancellationToken cancellationToken = default);
        Task<ImageDto> GetImageByNameAsync(string imageName, CancellationToken cancellationToken = default);
    }
}