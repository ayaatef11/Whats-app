namespace WhatsApp.Models
{
    public class Group
    {
        private string groupName;

        public Group(string groupName)
        {
            this.groupName = groupName;
        }

        public string Name { get; internal set; }
        public IList <Connection> Connections { get; internal set; }
    }
}
