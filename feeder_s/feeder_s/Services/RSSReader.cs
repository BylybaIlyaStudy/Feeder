using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Syndication;
using ReadSharp;
using System.ComponentModel.DataAnnotations.Schema;

namespace feeder_s.Services
{
    public class RSSReader
    {
        private List<XmlReader> RSSFeeds = new List<XmlReader>();
        public List<string> Links = new List<string>();

        public RSSReader() 
        {
            if (Program.s.Feeds != null)
            {
                foreach (RSS_Feed feed in Program.s.Feeds)
                {
                    if (feed.Status)
                        this.Links.Add(feed.Link);
                }
            }

            this.Update();
        }

        public void Update()
        {
            foreach (string link in this.Links)
            {
                try
                {
                    RSSFeeds.Add(XmlReader.Create(link));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(link + " feed not found");
                }
            }
        }

        public List<Items> GetElements()
        {
            List<Items> items = new List<Items>();

            foreach (XmlReader reader in this.RSSFeeds)
            {
                SyndicationFeed channel = SyndicationFeed.Load(reader);

                foreach (SyndicationItem RSI in channel.Items)
                {
                    items.Add(new Items());

                    items[items.Count - 1].title = RSI.Title.Text;
                    items[items.Count - 1].pubDate = RSI.PublishDate.DateTime.ToString();
                    //items[items.Count - 1].description = Regex.Replace(RSI.Summary.Text, @"<[^>]*>", String.Empty);
                    
                    if (Program.s.Description_format) 
                        items[items.Count - 1].description = HtmlUtilities.ConvertToPlainText(RSI.Summary.Text);
                    else
                        items[items.Count - 1].description = RSI.Summary.Text;

                    items[items.Count - 1].link += RSI.Links[0].Uri.ToString() + "\n";
                }
            }

            return items;
        }
    }
}
