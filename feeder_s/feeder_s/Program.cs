using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace feeder_s
{
    public class Program
    {
        public static Settings s;

        public static void Main(string[] args)
        {
            s = new Settings
            {
                Description_format = true,
                Feeds = new RSS_Feed[]
                {
                    new RSS_Feed()
                    {
                        Link = "https://habr.com/ru/rss/flows/popsci/all/?fl=ru",
                        Status = true
                    },
                    new RSS_Feed()
                    {
                        Link = "https://habr.com/ru/rss/flows/develop/all/?fl=ru",
                        Status = true
                    },
                },
                Time = 60000
            };

            Settings.SaveSettings("settings.xml", s);

            s = Settings.LoadSettings("settings.xml");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
