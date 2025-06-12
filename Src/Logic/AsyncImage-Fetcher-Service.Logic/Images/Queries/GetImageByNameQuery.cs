using AsyncImage_Fetcher_Service.Logic.Abstractions.Interfaces;

namespace AsyncImage_Fetcher_Service.Logic.Images.Queries
{
    public class GetImageByNameQuery : IQuery<string>
    {
        public string ImageName { get; }

        public GetImageByNameQuery(string imageName)
        {
            ImageName = imageName;
        }
    }
}