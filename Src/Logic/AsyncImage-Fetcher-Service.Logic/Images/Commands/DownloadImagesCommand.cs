using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;
using System.Collections.Generic;

namespace AsyncImage_Fetcher_Service.Logic.Images.Commands
{
    public class DownloadImagesCommand : ICommand<Dictionary<string, string>>
    {
        public IEnumerable<string> ImageUrls { get; }
        public int MaxDownloadAtOnce { get; }

        public DownloadImagesCommand(IEnumerable<string> imageUrls, int maxDownloadAtOnce)
        {
            ImageUrls = imageUrls;
            MaxDownloadAtOnce = maxDownloadAtOnce;
        }
    }
}