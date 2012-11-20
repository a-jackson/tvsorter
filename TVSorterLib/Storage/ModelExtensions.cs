// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Contains extension methods to model object for storage in XML.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    using TVSorter.Model;

    /// <summary>
    /// Contains extension methods to model object for storage in XML.
    /// </summary>
    internal static class ModelExtensions
    {
        #region Methods

        /// <summary>
        /// Populates the specified settings object from the XElement.
        /// </summary>
        /// <param name="settings">
        /// The settings object.
        /// </param>
        /// <param name="settingsNode">
        /// The settings element.
        /// </param>
        internal static void FromXml(this Settings settings, XElement settingsNode)
        {
            settings.SourceDirectory = settingsNode.GetAttribute("sourcedirectory", string.Empty);
            settings.DefaultOutputFormat = settingsNode.GetAttribute("defaultformat", string.Empty);
            settings.RecurseSubdirectories = bool.Parse(settingsNode.GetAttribute("recursesubdirectories", "false"));
            settings.DeleteEmptySubdirectories =
                bool.Parse(settingsNode.GetAttribute("deleteemptysubdirectories", "false"));
            settings.RenameIfExists = bool.Parse(settingsNode.GetAttribute("renameifexists", "false"));
            settings.DestinationDirectories = new List<string>();
            settings.DefaultDestinationDirectory = string.Empty;
            settings.UnlockMatchedShows = bool.Parse(settingsNode.GetAttribute("unlockmatchedshows", "false"));
            settings.AddUnmatchedShows = bool.Parse(settingsNode.GetAttribute("addunmatchedshows", "false"));
            settings.LockShowsWithNoEpisodes = bool.Parse(settingsNode.GetAttribute("lockshowsnonewepisodes", "false"));
            settings.DefaultDestinationDirectory = settingsNode.GetAttribute("defaultdestinationdir", string.Empty);

            XElement destinationDirectories = settingsNode.Element(Xml.GetName("DestinationDirectories"));

            if (destinationDirectories != null)
            {
                settings.DestinationDirectories =
                    destinationDirectories.GetElementsText(Xml.GetName("Destination")).ToList();
            }

            settings.FileExtensions =
                settingsNode.Element(Xml.GetName("FileExtensions")).GetElementsText(Xml.GetName("Extension")).ToList();
            settings.RegularExpressions =
                settingsNode.Element(Xml.GetName("RegularExpression")).GetElementsText(Xml.GetName("RegEx")).ToList();
            settings.OverwriteKeywords =
                settingsNode.Element(Xml.GetName("OverwriteKeywords")).GetElementsText(Xml.GetName("Keyword")).ToList();
        }

        /// <summary>
        /// Loads the specified XElement into the show.
        /// </summary>
        /// <param name="show">
        /// The show to read.
        /// </param>
        /// <param name="showNode">
        /// The element to read from.
        /// </param>
        internal static void FromXml(this TvShow show, XElement showNode)
        {
            show.Name = showNode.GetAttribute("name", string.Empty);
            show.FolderName = showNode.GetAttribute("foldername", string.Empty);
            show.TvdbId = showNode.GetAttribute("tvdbid", string.Empty);
            show.Banner = showNode.GetAttribute("banner", string.Empty);
            show.CustomFormat = showNode.GetAttribute("customformat", string.Empty);
            show.UseCustomFormat = bool.Parse(showNode.GetAttribute("usecustomformat", "false"));
            show.UseDvdOrder = bool.Parse(showNode.GetAttribute("usedvdorder", "false"));
            show.Locked = bool.Parse(showNode.GetAttribute("locked", "false"));
            show.LastUpdated = DateTime.Parse(showNode.GetAttribute("lastupdated", "1970-01-01 00:00:00"));
            show.UseCustomDestination = bool.Parse(showNode.GetAttribute("usecustomdestination", "false"));
            show.CustomDestinationDir = showNode.GetAttribute("customdestinationdir");
            show.AlternateNames =
                showNode.Descendants(Xml.GetName("AlternateName")).Select(altName => altName.Value).ToList();
            show.Episodes = showNode.Descendants(Xml.GetName("Episode")).Select(
                x =>
                    {
                        var episode = new Episode { Show = show };
                        episode.FromXml(x);
                        return episode;
                    }).ToList();
        }

        /// <summary>
        /// Loads the specified missing episode settings from XML.
        /// </summary>
        /// <param name="settings">
        /// The settings to load into.
        /// </param>
        /// <param name="missingEpsiodeElement">
        /// The XML element to load from.
        /// </param>
        internal static void FromXml(this MissingEpisodeSettings settings, XElement missingEpsiodeElement)
        {
            settings.HideNotYetAired = bool.Parse(missingEpsiodeElement.GetAttribute("hidenotaired", "false"));
            settings.HideLocked = bool.Parse(missingEpsiodeElement.GetAttribute("hidelocked", "false"));
            settings.HidePart2 = bool.Parse(missingEpsiodeElement.GetAttribute("hidepart2", "false"));
            settings.HideSeason0 = bool.Parse(missingEpsiodeElement.GetAttribute("hideseason0", "false"));
            settings.HideMissingSeasons = bool.Parse(missingEpsiodeElement.GetAttribute("hidemissingseasons", "false"));
        }

        /// <summary>
        /// Loads the episode from XML.
        /// </summary>
        /// <param name="episode">
        /// The episode to load into.
        /// </param>
        /// <param name="episodeNode">
        /// The Episode element to load.
        /// </param>
        internal static void FromXml(this Episode episode, XElement episodeNode)
        {
            episode.Name = episodeNode.GetAttribute("name", string.Empty);
            episode.TvdbId = episodeNode.GetAttribute("tvdbid", string.Empty);
            episode.SeasonNumber = int.Parse(episodeNode.GetAttribute("seasonnum", "-1"));
            episode.EpisodeNumber = int.Parse(episodeNode.GetAttribute("episodenum", "-1"));
            episode.FirstAir = DateTime.Parse(episodeNode.GetAttribute("firstair", "1970-01-01 00:00:00"));
            episode.FileCount = int.Parse(episodeNode.GetAttribute("filecount", "0"));
        }

        /// <summary>
        /// Gets the XML for these settings.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// The XML element that represents these settings.
        /// </returns>
        internal static XElement ToXml(this Settings settings)
        {
            var destinationDirectories = new XElement(
                Xml.GetName("DestinationDirectories"), 
                settings.DestinationDirectories.Select(
                    dir =>
                    new XElement(
                        Xml.GetName("Destination"), 
                        dir)));

            var fileExtensions = new XElement(
                Xml.GetName("FileExtensions"), 
                settings.FileExtensions.Select(ext => new XElement(Xml.GetName("Extension"), ext)));

            var regularExpressions = new XElement(
                Xml.GetName("RegularExpression"), 
                settings.RegularExpressions.Select(exp => new XElement(Xml.GetName("RegEx"), exp)));

            var overwriteKeywords = new XElement(
                Xml.GetName("OverwriteKeywords"), 
                settings.OverwriteKeywords.Select(key => new XElement(Xml.GetName("Keyword"), key)));

            return new XElement(
                Xml.GetName("Settings"), 
                new XAttribute("sourcedirectory", settings.SourceDirectory), 
                new XAttribute("defaultformat", settings.DefaultOutputFormat), 
                new XAttribute("recursesubdirectories", settings.RecurseSubdirectories), 
                new XAttribute("deleteemptysubdirectories", settings.DeleteEmptySubdirectories), 
                new XAttribute("renameifexists", settings.RenameIfExists), 
                new XAttribute("addunmatchedshows", settings.AddUnmatchedShows), 
                new XAttribute("unlockmatchedshows", settings.UnlockMatchedShows), 
                new XAttribute("lockshowsnonewepisodes", settings.LockShowsWithNoEpisodes), 
                new XAttribute("defaultdestinationdir", settings.DefaultDestinationDirectory),
                destinationDirectories, 
                fileExtensions, 
                regularExpressions, 
                overwriteKeywords);
        }

        /// <summary>
        /// Converts the TVShow to XML.
        /// </summary>
        /// <param name="show">
        /// The show.
        /// </param>
        /// <returns>
        /// The XML element for the show.
        /// </returns>
        internal static XElement ToXml(this TvShow show)
        {
            var alternateNames = new XElement(Xml.GetName("AlternateNames"));
            if (show.AlternateNames != null)
            {
                alternateNames.Add(
                    show.AlternateNames.Select(
                        alternateName => new XElement(Xml.GetName("AlternateName"), alternateName)));
            }

            var episodes = new XElement(Xml.GetName("Episodes"));

            if (show.Episodes != null)
            {
                episodes.Add(show.Episodes.Select(x => x.ToXml()));
            }

            var element = new XElement(
                Xml.GetName("Show"), 
                new XAttribute("name", show.Name ?? string.Empty), 
                new XAttribute("foldername", show.FolderName ?? string.Empty), 
                new XAttribute("tvdbid", show.TvdbId ?? string.Empty), 
                new XAttribute("banner", show.Banner ?? string.Empty), 
                new XAttribute("customformat", show.CustomFormat ?? string.Empty), 
                new XAttribute("usecustomformat", show.UseCustomFormat), 
                new XAttribute("usedvdorder", show.UseDvdOrder), 
                new XAttribute("locked", show.Locked), 
                new XAttribute("lastupdated", show.LastUpdated), 
                new XAttribute("usecustomdestination", show.UseCustomDestination),
                new XAttribute("customdestinationdir", show.CustomDestinationDir ?? string.Empty),
                alternateNames, 
                episodes);

            return element;
        }

        /// <summary>
        /// Converts the settings to XML.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        /// <returns>
        /// The XML for the settings.
        /// </returns>
        internal static XElement ToXml(this MissingEpisodeSettings settings)
        {
            return new XElement(
                Xml.GetName("MissingEpisodeSettings"), 
                new XAttribute("hidenotaired", settings.HideNotYetAired), 
                new XAttribute("hidelocked", settings.HideLocked), 
                new XAttribute("hidepart2", settings.HidePart2), 
                new XAttribute("hideseason0", settings.HideSeason0), 
                new XAttribute("hidemissingseasons", settings.HideMissingSeasons));
        }

        /// <summary>
        /// Converts the episode to XML.
        /// </summary>
        /// <param name="episode">
        /// The episode.
        /// </param>
        /// <returns>
        /// The XElement.
        /// </returns>
        internal static XElement ToXml(this Episode episode)
        {
            var episodeElement = new XElement(
                Xml.GetName("Episode"), 
                new XAttribute("name", episode.Name), 
                new XAttribute("tvdbid", episode.TvdbId), 
                new XAttribute("seasonnum", episode.SeasonNumber), 
                new XAttribute("episodenum", episode.EpisodeNumber), 
                new XAttribute("firstair", episode.FirstAir), 
                new XAttribute("filecount", episode.FileCount));
            return episodeElement;
        }

        #endregion
    }
}