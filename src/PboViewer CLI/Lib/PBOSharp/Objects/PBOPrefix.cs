namespace PBOSharp.Objects
{
    public class PBOPrefix
    {
        public string PrefixName { get; set; }
        public string PrefixValue { get; set; }

        public PBOPrefix(string name, string value)
        {
            PrefixName = name;
            PrefixValue = value;
        }
    }
}
