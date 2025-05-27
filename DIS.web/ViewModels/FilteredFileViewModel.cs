using DIS.DataAccess.Entity;

namespace DIS.Web.ViewModels
{
    public class FilteredFileViewModel
    {
        public File_TB File { get; set; }
        public int WordCount { get; set; }
        public int ParagraphCount { get; set; }
    }
}
