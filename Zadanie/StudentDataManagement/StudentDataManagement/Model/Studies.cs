
namespace StudentDataManagement.Model
{
    public class Studies
    {
        public string Name { get; }
        public string Mode { get; }

        public Studies(string name, string mode)
        {
            Name = name;
            Mode = mode;
        }
    }
}
