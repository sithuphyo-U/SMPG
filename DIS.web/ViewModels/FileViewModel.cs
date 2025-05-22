namespace DIS.Web.ViewModels
{
    public class FileViewModel
    {
        public int? id { get; set; }
        public string originalfile_name { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }
        public byte[] file_bytes  { get; set; }
        public string path { get; set; }
        public string url { get; set; }
        //for viewer
        public int? index { get; set; }
    }
}
