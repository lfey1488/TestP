using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using TestP;
using TestP.DB;

class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding win1251 = Encoding.GetEncoding("Windows-1251");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            HasHeaderRecord = false,
            TrimOptions = TrimOptions.Trim
        };

        List<string> categories, subdivisions, processes = [];

        Console.WriteLine("Введите путь к файлу:");
        string? filePath = Console.ReadLine();
        //string filePath = @"C:\Users\Lfey\source\repos\TestP\test_data.CSV";

        using (var reader = new StreamReader(filePath, win1251))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Read();
            var records = csv.GetRecords<Test_data>();

            categories = records.Select(p => p.Category).Distinct().ToList();
        }

        using (var reader = new StreamReader(filePath, win1251))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Read();
            var records = csv.GetRecords<Test_data>();

            subdivisions = records.Select(p => p.Subdivision).Distinct().ToList();
        }

        using (var reader = new StreamReader(filePath, win1251))
        using (var csv = new CsvReader(reader, config))
        {
            csv.Read();
            var records = csv.GetRecords<Test_data>();

            processes = records.Select(p => p.Process_code).Distinct().ToList();
        }

        using (ApplicationContext db = new())
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            foreach (var category in categories)
                db.Categories.Add(new() { Category_name = category });
            db.SaveChanges();

            foreach (var sub_name in subdivisions)
                db.Subdivisions.Add(new() { Subdivision_name = sub_name });
            db.SaveChanges();

            using (var reader = new StreamReader(filePath, win1251))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                var records = csv.GetRecords<Test_data>();

                foreach (var rec in records)
                {
                    int? id = db.Processes.Where(p => FindParent(rec.Process_code) == p.Process_code).Select(x => x.Id).ToList().FirstOrDefault();
                    db.Processes.Add(new()
                    {
                        Process_code = rec.Process_code,
                        Process_name = rec.Process_name,
                        Category_id = db.Categories.Where(p => p.Category_name == rec.Category).Select(x => x.Id).ToList().First(),
                        Subdivision_id = db.Subdivisions.Where(p => p.Subdivision_name == rec.Subdivision).Select(x => x.Id).ToList().First(),
                        Parent_Id = id == 0 ? null : id
                    });
                    db.SaveChanges();
                }
            }
            db.Dispose();
        }

        Console.WriteLine("Exit");
        Console.ReadLine();
    }

    public static string? FindParent(string child)
    {
        int index;
        string? parent = null;

        if (child.Split('.').Length > 1) 
        {
            index = child.LastIndexOf('.');
            parent = child[..index];
        }
        return parent;
    }
}