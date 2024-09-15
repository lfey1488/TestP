namespace TestP.DB
{
    public class Process
    {
        public int Id {  get; set; }
        public string? Process_code { get; set; }
        public string? Process_name { get; set; }
        public int Category_id { get; set; }
        public int Subdivision_id { get; set; }
        public int? Parent_Id { get; set; }
    }
}
