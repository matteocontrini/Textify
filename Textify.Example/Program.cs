using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Textify.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://openfiber.it/mondo-open-fiber/comunicati-stampa/";
            //url = "https://event.unitn.it/cerimonia-laurea/";
            //url = "https://blog.botfactory.it";
            //url = "https://www.trentinoinrete.it/Documentazioni-per-gli-Enti-Locali/Previsione-degli-interventi-per-comune";

            HttpClient http = new HttpClient();
            string html = await http.GetStringAsync(url);

            HtmlToTextConverter converter = new HtmlToTextConverter();
            string output = converter.Convert(html);

            File.WriteAllText("out.txt", output);

            Console.WriteLine(output);
        }
    }
}
