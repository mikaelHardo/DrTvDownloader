using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                var tags = Directory.Exists(episodeDir) ? Directory.GetFiles(episodeDir)
                    .Select(s =>
                    {
                        try
                        {
                            return TagLib.File.Create(s).Tag.Comment;
                        }
                        catch (Exception e)
                        {
                            return "";
                        }
                    }) : new List<string>();

                var exists = Directory.Exists(episodeDir) &&
                             tags.Any(a => a == slug.Slug);

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

                var cardData = programCard.Data.FirstOrDefault();

                if (!Directory.Exists(episodeDir))
                {
                    Directory.CreateDirectory(episodeDir);
                }

                _logger.Log($"Downloading {slug.Slug}");

                var title = cardData.Title;
                var episode = cardData.EpisodeNumber;
                var episodeTag = $"({episode})";

                if (title.Contains(episodeTag))
                {
                    title = title.Replace($" {episodeTag}", "");
                }
                else
                {
                    var titleSplit = title.Split(new[] { '(', ':', ')' });

                    // format: "name (8:10)"
                    if (titleSplit.Length == 4)
                    {
                        int.TryParse(titleSplit[1], out episode);
                    }

                    if (title.Contains('('))
                    {
                        // lets remove everyting after the last begin parantes
                        title = title.Substring(0, title.LastIndexOf('(') - 1);
                    }

                    // if we get this far, the title is probably fine :-)
                }

                string name;

                if (episode == 0)
                {
                    name = cardData.Title + ".mp4";
                }
                else
                {
                    name = $"{title} S{cardData.SeasonNumber.ToString().PadLeft(2, '0')}E{episode.ToString().PadLeft(2, '0')}.mp4";
                }

                DownloadVideo(videoFeed, episodeDir, name, slug.Slug);
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

        private void DownloadVideo(string url, string dir, string newFilename, string slug)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                WorkingDirectory = dir,
                FileName = "youtube-dl.exe",
                Arguments = $"{url}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.GetEncoding(1252)
            };

            var filename = "";

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    var stderrThread = new Thread(() => { process.StandardError.ReadToEnd(); });
                    stderrThread.Start();

                    // Read stdout synchronously (on this thread)
                    while (true)
                    {
                        var line = process.StandardOutput.ReadLine();
                        if (line == null)
                        {
                            break;
                        }

                        _logger.Log(line);

                        // example: [download] Destination: Kasper og Sofie (11)-kasper-og-sofie-11-2.mp4
                        // other example: [download] Masha og Bj°rnen (1)-masha-og-bjoernen-1.mp4 has already been downloaded
                        if (line.Contains(".mp4") && line.StartsWith("[download]"))
                        {
                            filename = line.Replace("[download] Destination: ", "")
                                           .Replace("[download] ", "")
                                           .Replace(" has already been downloaded", "");
                        }
                    }

                    process.WaitForExit();
                    stderrThread.Join();
                }

                if (filename == "")
                {
                    return;
                }

                _logger.Log($"Found {filename}");

                var fileName = dir + "/" + filename;

                var file = TagLib.File.Create(fileName);

                // add the slug as the comment;
                file.Tag.Comment = slug;
                file.Save();

                File.Move(fileName, dir + "/" + newFilename);
            }
            catch(Exception e)
            {
                _logger.Log($"Could not download: {url}");
            }
        }
    }
}
