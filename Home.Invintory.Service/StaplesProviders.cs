using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Home.Invintory.Service.Model;

namespace Home.Invintory.Service
{
    public static class StaplesProvider
    {
        public static IEnumerable<Ingrident> GetStablesFor(int days)
        {
            foreach (var file in Directory.EnumerateFiles("StaplesFiles")
                .Select(f => new { RecipieName = Path.GetFileNameWithoutExtension(f), FileName = f }))
            {
                using (var reader = new StreamReader(file.FileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (var ingrident in csv.GetRecords<Ingrident>())
                    {
                        ingrident.Quantity *= days;
                        yield return ingrident;
                    }
                }
            }
        }
    }
}
