namespace TestP
{
    class Test_data
    {
        public string Category {  get; set; }
        public string Process_code { get; set; }
        public string Process_name { get; set; }
        public string Subdivision { get; set; }

        public Test_data(string category, string process_code, string process_name, string subdivision)
        {
            Category = category;
            Process_code = process_code;
            Process_name = process_name;
            Subdivision = subdivision;
        }
    }
}
