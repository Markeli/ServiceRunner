namespace ServiceRunner.Args
{
    internal class Option
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsRequired { get; set; }

        public bool IsFlag { get; set; }

        public bool IsSetted { get; set; }
    }
}
