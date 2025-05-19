namespace DIS.Web.ViewModels
{
    public class FileViewModel
    {
        public int? id { get; set; }
        public string original_filename { get; set; }
        public string filename { get; set; }
        public string file_type { get; set; }
        public byte[] file_bytes  { get; set; }
        public string path { get; set; }
        public string url { get; set; }
        //for viewer
        public int? index { get; set; }
    }
}
