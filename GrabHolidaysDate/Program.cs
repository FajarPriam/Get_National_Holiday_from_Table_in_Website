using GrabHolidaysDate.Model;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GrabHolidaysDate
{
    class Program
    {
        static void Main(string[] args)
        {
            string detail;
            var hasil = 0;

            MyContext _context = new MyContext();
            HtmlWeb document = new HtmlWeb();

            Console.Write("Input Year : ");
            //set year by input
            string param = Convert.ToString(Console.ReadLine());
            //set year now 
            //string param = Convert.ToString(DateTime.Now.Year);

            //cek database if the year alredy exists
            var cek = _context.Holiday.Where(a => a.Tahun == param).ToList();

            if (cek.Count == 0)
            {
                // grap table from website
                var document2 = document.Load(@"https://www.officeholidays.com/countries/indonesia/" + param);
                HtmlNodeCollection nodes = document2.DocumentNode.SelectNodes("//table[@class='country-table']/tbody/tr");

                if (nodes != null)
                {
                    HtmlNode[] nodes1 = nodes.ToArray();
                    foreach (HtmlNode item in nodes1)
                    {
                        //splite data
                        TBL_M_HOLIDAYS _holidays = new TBL_M_HOLIDAYS();
                        var date = Regex.Split(item.InnerHtml, "\"");
                        var tgl = date[5];
                        var splitDate = Regex.Split(tgl, "-");

                        var splitWords = Regex.Split(item.InnerText, "\n");
                        var words = splitWords
                                    .Where(x => !x.Contains("&nbsp;") && !string.IsNullOrEmpty(x.Trim()))
                                    .ToList();

                        if (words[2].Contains("&#039;"))
                        {
                            var words1 = words[2];
                            var splitWords1 = Regex.Split(words1, "&#039;");

                            detail = splitWords1[0] + "'" + splitWords1[1];
                        }
                        else
                        {
                            detail = words[2];
                        }

                        var tahun = splitDate[0];

                        //insert to DB
                        _holidays.Date_Holiday = Convert.ToDateTime(tgl);
                        _holidays.Keterangan = detail;
                        _holidays.Tahun = tahun;

                        _context.Holiday.Add(_holidays);
                        hasil = _context.SaveChanges();

                        //show date and detail in console
                        var result = $"{tgl} ; {detail}";
                        Console.WriteLine(result);

                        //show innerHtml and innerText in console
                        //Console.WriteLine(item.InnerHtml);
                        //Console.WriteLine(item.InnerText);
                    }
                    if (hasil > 0)
                    {
                        Console.WriteLine("\nInsert Success");
                    }
                    else
                    {
                        Console.WriteLine("\nInsert Fail");
                    }
                }
                else
                {
                    Console.WriteLine("List holiday in year " + param + " not found!");
                }
            }
            else
            {
                Console.WriteLine("List holiday in year " + param + " already exists");
            }
            Console.ReadKey();
        }
    }
}
