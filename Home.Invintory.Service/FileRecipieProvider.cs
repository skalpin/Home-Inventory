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
    }
}
