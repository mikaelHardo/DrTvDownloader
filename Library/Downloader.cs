using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DrTvDownloader.Library.Helpers;
using Newtonsoft.Json;

namespace DrTvDownloader.Library
{
    public class Downloader
    {
        private readonly Logger _logger = new Logger();

        public void FindNewEpisodes()
        {
            VerifyYoutubeDl();
            VerifyFfmpeg();


            var keywords = ConfigurationManager.AppSettings["Keywords"].Split(',');

            foreach (var keyword in keywords)
            {
                SearchAndDownload(keyword);
            }
        }

        private void VerifyFfmpeg()
        {
            if (File.Exists("ffmpeg.exe"))
            {
                return;
            }

            _logger.Log("Could not fild ffmpeg, trying to download");

            using (var client = new WebClient())
            {
                client.DownloadFile("http://ffmpeg.zeranoe.com/builds/win64/static/ffmpeg-3.3.2-win64-static.zip",
                    "ffmpeg.zip");
            }

            ZipFile.ExtractToDirectory("ffmpeg.zip", "ffmpeg");

            File.Move("ffmpeg/ffmpeg-3.3.2-win64-static/bin/ffmpeg.exe", "ffmpeg.exe");
            File.Move("ffmpeg/ffmpeg-3.3.2-win64-static/bin/ffplay.exe", "ffplay.exe");
            File.Move("ffmpeg/ffmpeg-3.3.2-win64-static/bin/ffprobe.exe", "ffprobe.exe");
            File.Delete("ffmpeg.zip");
            Directory.Delete("ffmpeg", true);

            _logger.Log("Finished downloading ffmpeg");
        }

        private void VerifyYoutubeDl()
        {
            if (!File.Exists("youtube-dl.exe"))
            {
                _logger.Log("Could not fild Youtube-dl, trying to download");

                using (var client = new WebClient())
                {
                    client.DownloadFile("https://yt-dl.org/downloads/2017.07.15/youtube-dl.exe", "youtube-dl.exe");
                }

                _logger.Log("Finished downloading youtube-dl");
            }
        }

        private void SearchAndDownload(string keyword)
        {
            _logger.Log($"Looking for series {keyword}");

            var videoDir = ConfigurationManager.AppSettings["VideoDir"];

            var seachUrl = $@"http://www.dr.dk/mu/search/bundle?Title=%24like(%27{keyword}%27)";
            var searchResult = Get<BundleResult>(seachUrl);

            var slugs = searchResult.Data.SelectMany(s => s.Relations.Select(r => new { r.Slug, s.Title })).Distinct().ToList();

            foreach (var slug in slugs)
            {
                var episodeDir = videoDir + "/" + slug.Title;

                var exists = Directory.Exists(episodeDir) && 
                             Directory.GetFiles(episodeDir)
                                      .Select(s => s.Split(')').Last())
                                      .Select(s => s.Substring(1))
                                      .Select(s => s.Replace(".mp4", ""))
                                      .Any(a => a.EndsWith(slug.Slug));

                if (exists)
                {
                    _logger.Log($"Already have {slug.Slug}");
                    continue;
                }

                var slugUrl = $@"http://www.dr.dk/mu/programcard/expanded?id={slug.Slug}";

                var programCard = Get<ProgramCardResult>(slugUrl);

                var videoFeed = programCard.Data.Select(s => s.PresentationUri).FirstOrDefault();

                if (videoFeed == null)
                {
                    continue;
                }

                if (!Directory.Exists(episodeDir))
                {
                    Directory.CreateDirectory(episodeDir);
                }

                _logger.Log($"Downloading {slug.Slug}");

                DownloadVideo(videoFeed, episodeDir);
            }
        }

        private static T Get<T>(string url)
        {
            string json;

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                json = client.DownloadString(url);
            }

            var result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        private void DownloadVideo(string url, string dir)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WorkingDirectory = dir,
                FileName = "youtube-dl.exe",
                Arguments = $"{url}",
            };

            try
            {
                using (var exeProcess = Process.Start(startInfo))
                {
                    // TODO: capture output so we can log it somewhere else
                    exeProcess?.WaitForExit();
                }
            }
            catch
            {
                _logger.Log($"Could not download: {url}");
            }
        }
    }
}
