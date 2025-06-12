namespace AsyncImage_Fetcher_Service.Logic.Images.Dtos
{
    public class ImageToSaveDto
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}