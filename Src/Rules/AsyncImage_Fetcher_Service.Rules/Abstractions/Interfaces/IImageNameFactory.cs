using AsyncImage_Fetcher_Service.Rules.Models;

namespace AsyncImage_Fetcher_Service.Rules.Abstractions
{
    public interface IImageNameFactory
    {
        Image Create(string url);
    }
}