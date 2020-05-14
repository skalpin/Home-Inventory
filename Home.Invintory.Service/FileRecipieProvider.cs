using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CsvHelper;
using Home.Invintory.Service.Model;

namespace Home.Invintory.Service
{
    public class FileRecipieProvider
    {
        public IEnumerable<Ingrident> GetIngridentsFor(IEnumerable<string> recipieNames)
        {
            foreach (var file in Directory.EnumerateFiles("RecipieFiles")
                .Select(f => new { RecipieName = Path.GetFileNameWithoutExtension(f), FileName = f })
                .Where(f => recipieNames.Any(r => Regex.IsMatch(f.RecipieName, $@"^{r}$"))))
            {
                using (var reader = new StreamReader(file.FileName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (var ingrident in csv.GetRecords<Ingrident>())
                        yield return ingrident;
                }
            }
        }

        public IEnumerable<Ingrident> GetIngridentsFor(Func<string, bool> action)
        {
            var recipiesToAdd = new List<string>();

            foreach (var recipie in Directory.EnumerateFiles("RecipieFiles")
                .Select(f => new { RecipieName = Path.GetFileNameWithoutExtension(f), FileName = f }))
            {
                if (action(recipie.RecipieName))
                {
                    recipiesToAdd.Add(recipie.FileName);
                }
            }

            foreach (var file in recipiesToAdd)
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    foreach (var ingrident in csv.GetRecords<Ingrident>())
                        yield return ingrident;
                }
            }
        }
    }
}
