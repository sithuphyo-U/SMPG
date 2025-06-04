namespace DIS.Web.ViewModels
{
    public class ImageViewModel
    {
        public int imageId { get; set; }
        public string? original_image_name { get; set; }
        public string? image_name { get; set; }
        public string? image_type { get; set; }
        public byte[]? file_bytes { get; set; }
        public string? path { get; set; }
        public string? url { get; set; }

        public IFormFile? form_files { get; set; }
        public int? index { get; set; }
    }
}
