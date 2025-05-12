namespace DIS.Application.Service.Common
{
    public class CommandResultModel
    {
        public bool success { get; set; }
        public List<string> messages { get; set; }
        public int id { get; set; }
        public object model { get; set; }
        public CommandResultModel()
        {
            messages = new List<string>();
        }
    }
}