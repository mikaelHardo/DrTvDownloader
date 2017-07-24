using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrTvDownloader.Library;

namespace DrTvDownloader.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var downloader = new Downloader();
            downloader.FindNewEpisodes();
        }
    }
}
