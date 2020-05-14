using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Home.Invintory.Service.Model;

namespace Home.Invintory.Service
{
    public class FileRecipieProvider
    {
        public IEnumerable<Ingrident> GetIngridentsFor(IEnumerable<string> recipieNames)
        {
            foreach (var file in Directory.EnumerateFiles("RecipieFiles").Where(f => recipieNames.Contains(f)))
            {
                using (var reader = new StreamReader($"RecipieFiles\\{file}"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (var ingrident in csv.GetRecords<Ingrident>())
                        yield return ingrident;
                }
            }
        }
    }
}
