using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TheTvdbDotNet;
using TVSorter.Model;

namespace TVSorter.Data.TvdbV2
{
    public class TvdbV2 : IDataProvider
    {
        private readonly ITvdbSeries series;
        private readonly ITvdbSearch search;
        private readonly ITvdbUpdate update;
        private readonly IStreamWriter streamWriter;

        public TvdbV2(ITvdbSeries series, ITvdbSearch search, ITvdbUpdate update, IStreamWriter streamWriter)
        {
            this.series = series;
            this.search = search;
            this.update = update;
            this.streamWriter = streamWriter;
        }

        public List<TvShow> SearchShow(string name)
        {
            try
            {
                var series = search.SeriesSearchAsync(name: name).GetAwaiter().GetResult();
                return series.Data.Select(x => new TvShow
                {
                    Name = x.SeriesName,
                    TvdbId = x.Id,
                    FolderName = name,
                }).ToList();
            }
            catch (TvdbRequestException)
            {
                return new List<TvShow>();
            }
        }

        public void UpdateShow(TvShow show)
        {
            var newSeries = series.GetSeriesAsync(show.TvdbId).GetAwaiter().GetResult();
            if (show.Banner != newSeries.Data.Banner)
            {
                show.Banner = newSeries.Data.Banner;
                var banner = series.GetBannerAsnyc(newSeries.Data).Result;
                var targetPath = $"Images{Path.DirectorySeparatorChar}{show.TvdbId}.jpg";
                streamWriter.WriteStream(banner, targetPath);
            }

            var newEpisodes = series.GetAllEpisodesAsync(show.TvdbId).GetAwaiter().GetResult()
                .Select(x =>
                    new Episode
                    {
                        TvdbId = x.Id.ToString(),
                        EpisodeNumber = show.UseDvdOrder && x.DvdEpisodeNumber.HasValue ? x.DvdEpisodeNumber.Value : x.AiredEpisodeNumber.Value,
                        SeasonNumber = show.UseDvdOrder && x.DvdSeason.HasValue ? x.DvdSeason.Value : x.AiredSeason.Value,
                        FirstAir = DateTime.Parse(string.IsNullOrEmpty(x.FirstAired) ? "1970-01-01" : x.FirstAired),
                        Name = x.EpisodeName ?? string.Empty,
                        Show = show,
                    })
                .ToList();

            if (show.Episodes != null)
            {
                foreach (Episode episode in newEpisodes)
                {
                    Episode currentEpisode = show.Episodes.FirstOrDefault(x => x.Equals(episode));
                    if (currentEpisode != null)
                    {
                        episode.FileCount = currentEpisode.FileCount;
                    }
                }
            }

            show.Episodes = newEpisodes;
            show.LastUpdated = DateTime.UtcNow;
        }

        public IEnumerable<TvShow> UpdateShows(IList<TvShow> shows)
        {
            DateTime firstUpdate = shows.Min(x => x.LastUpdated);
            List<int> updateIds;

            // Only get the updates if the date is less than a month ago.
            if (firstUpdate > DateTime.Today.Subtract(TimeSpan.FromDays(30)))
            {
                updateIds = update.GetUpdatesAsync(firstUpdate).Result.Data.Select(x => x.Id).ToList();
            }
            else
            {
                // Update all shows
                updateIds = shows.Select(x => x.TvdbId).ToList();
            }

            foreach (TvShow show in shows)
            {
                // Skip the show if it isn't in the updateIds list.
                if (!updateIds.Contains(show.TvdbId))
                {
                    Logger.OnLogMessage(this, "No updates for {0}", LogType.Info, show.Name);

                    // Update the last updated time anyway, it is still up to date at this time.
                    show.LastUpdated = DateTime.UtcNow;
                }
                else
                {
                    UpdateShow(show);
                }

                yield return show;
            }
        }
    }
}
