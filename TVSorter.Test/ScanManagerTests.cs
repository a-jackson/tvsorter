﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ScanManagerTests.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Tests for the  class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TVSorter.Data;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.Wrappers;

namespace TVSorter.Test
{
    /// <summary>
    ///     Tests for the <see cref="ScanManager" /> class.
    /// </summary>
    [TestFixture]
    public class ScanManagerTests : ManagerTestBase
    {
        /// <summary>
        ///     Sets up the tests in the fixture.
        /// </summary>
        [SetUp]
        public override void Setup()
        {
            base.Setup();

            dataProvider = Substitute.For<IDataProvider>();
            tvShowRepository = new TvShowRepository(StorageProvider, dataProvider);
            scanManager = new ScanManager(StorageProvider, dataProvider, tvShowRepository);
        }

        /// <summary>
        ///     Gets or sets the data provider.
        /// </summary>
        private IDataProvider dataProvider;

        /// <summary>
        ///     The scan manager that the tests will be performed on.
        /// </summary>
        private ScanManager scanManager;

        /// <summary>
        ///     The TV show repository.
        /// </summary>
        private ITvShowRepository tvShowRepository;

        /// <summary>
        ///     This test will test several different output formats to ensure
        ///     that the destination paths are correct.
        /// </summary>
        /// <param name="format">
        ///     The format being tested.
        /// </param>
        /// <returns>
        ///     The output path for the file.
        /// </returns>
        [TestCase(
            @"{FName}\Season {SNum(1)}\{SName(.)}.S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}",
            ExpectedResult = @"Alpha Folder\Season 1\Alpha.Show.S01E01.Episode.One.(1).avi")]
        [TestCase(
            @"{FName}\{Date(yyyy)}\{Date(MMM)}\{SName(.)}.S{SNum(2)}E{ENum(2)}.{Date(dd-MMM-yyyy)}.{EName(.)}{Ext}",
            ExpectedResult = @"Alpha Folder\2012\Jan\Alpha.Show.S01E01.01-Jan-2012.Episode.One.(1).avi")]
        [TestCase(@"{FName}", ExpectedResult = "Alpha Folder")]
        [TestCase(@"{SName( )} {SName(.)} {SName(_)}", ExpectedResult = "Alpha Show Alpha.Show Alpha_Show")]
        [TestCase(
            @"{EName( )} {EName(.)} {EName(_)}",
            ExpectedResult = "Episode One (1) Episode.One.(1) Episode_One_(1)")]
        [TestCase(@"{SNum(1)} {SNum(2)} {SNum(3)}", ExpectedResult = "1 01 001")]
        [TestCase(@"{ENum(1)} {ENum(2)} {ENum(3)}", ExpectedResult = "1 01 001")]
        [TestCase(@"{Date(yyyy)} {Date(MM)} {Date(MMM)} {Date(dd)}", ExpectedResult = "2012 01 Jan 01")]
        public string TestOutputFormat(string format)
        {
            // Creat the result.
            var result = new FileResult
            {
                Checked = true,
                Show = TestShows.First(),
                InputFile = Substitute.For<IFileInfo>()
            };
            result.Episode = result.Show.Episodes.First();
            result.Episodes = new List<Episode> { result.Episode };
            result.InputFile.Extension.Returns(".avi");

            var fileResultManager = new FileResultManager(StorageProvider);

            // Format the string.
            return fileResultManager.FormatOutputPath(result, format);
        }

        /// <summary>
        ///     Asserts the result matches the first show and its first episode.
        /// </summary>
        /// <param name="result">
        ///     The result to check.
        /// </param>
        private void MatchesShow1(FileResult result)
        {
            Assert.AreEqual(TestShows.First(), result.Show, "The result should be the first test show.");
            Assert.AreEqual(
                TestShows.First().Episodes.First(),
                result.Episode,
                "The result should match the show's first episode.");
        }

        /// <summary>
        ///     Tests that an alternate name is added to an existing show.
        /// </summary>
        [Test]
        public void AddAlternateName()
        {
            var alpha = TestShows.First();

            // When the data provider is searched for ShowName, return Alpha.
            dataProvider.SearchShow("ShowName").Returns(new List<TvShow> { alpha });

            // Create the file to be searched.
            CreateTestFile(Root, "ShowName.S01E01.avi");

            // When the show is saved, alpha should contain ShowName as an alternate name.
            StorageProvider.When(x => x.SaveShow(alpha))
                .Do(x => Assert.Contains("ShowName", x.Arg<TvShow>().AlternateNames));

            var results = scanManager.Refresh(Root);

            // There should be one result matching Delta Episode 1.
            Assert.AreEqual(1, results.Count, "There should be one result.");
            Assert.AreEqual(results[0].Show, alpha, "The show shuld be Alpha");

            // Check that the results were called.
            StorageProvider.Received(1).SaveShow(alpha);
        }

        /// <summary>
        ///     Tests the functionality to add unmatched shows.
        /// </summary>
        [Test]
        public void AddUnmatchedShow()
        {
            // Create new show and episodes
            var delta = new TvShow { Name = "Delta", FolderName = "Delta", TvdbId = 4 };
            var episode1 = new Episode { EpisodeNumber = 1, SeasonNumber = 1, Name = "Episode 1", TvdbId = "41" };

            // Create a file for the search to return.
            CreateTestFile(Root, "Delta.S01E01.avi");

            // When the data provider searches for Delta return the delta show.
            dataProvider.SearchShow("Delta").Returns(new List<TvShow> { delta });

            // When the show is saved, get the new show. A new one will be created by
            // TvShow.FromSearchResult
            StorageProvider.When(x => x.SaveShow(Arg.Any<TvShow>())).Do(x => { delta = x.Arg<TvShow>(); });

            // Delta should be updated.When it is, add the episode.
            dataProvider.When(x => x.UpdateShow(delta)).Do(x => { delta.Episodes = new List<Episode> { episode1 }; });

            // Search the folder.
            var results = scanManager.Refresh(Root);

            // There should be one result matching Delta Episode 1.
            Assert.AreEqual(1, results.Count, "There should be one result.");
            Assert.AreEqual(results[0].Show, delta, "The show shuld be Delta");
            Assert.AreEqual(results[0].Episode, episode1, "The episode should be Episode 1");
        }

        /// <summary>
        ///     Tests the output format the dual episodes.
        /// </summary>
        [Test]
        public void DualEpisodeFormatting()
        {
            // Creat the result.
            var result = new FileResult
            {
                Checked = true,
                Show = TestShows.First(),
                InputFile = Substitute.For<IFileInfo>()
            };
            result.Episode = result.Show.Episodes.First();
            result.Episodes = new List<Episode> { result.Episode, result.Show.Episodes[1] };
            result.InputFile.Extension.Returns(".avi");

            var fileResultManager = new FileResultManager(StorageProvider);

            // Format the string.
            var output = fileResultManager.FormatOutputPath(result, "{SName(.)}.S{SNum(2)}E{ENum(2)}.{EName(.)}");
            Assert.AreEqual(
                "Alpha.Show.S01E01-02.Episode.One.(1-2)",
                output,
                "The output format does not match what it should be.");
        }

        /// <summary>
        ///     Tests the scanner ability to detect dual episodes.
        /// </summary>
        [Test]
        public void DualEpisodeScanning()
        {
            CreateTestFile(Root, "Alpha.S01E01-02.avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result");
            MatchesShow1(results[0]);
            Assert.AreEqual(2, results[0].Episodes.Count, "There should be 2 episode in the result.");
            Assert.AreEqual(
                TestShows.First().Episodes[1],
                results[0].Episodes[1],
                "The second episode should be show's second epsiode.");
        }

        /// <summary>
        ///     Tests the precedence of the the regular expressions to ensure that the higher listed match is used in the case of
        ///     multiple matches.
        /// </summary>
        /// <param name="seasonEpisodeNumber">
        ///     The season and episode number string format.
        /// </param>
        [Test]
        public void EpisodeFormatPrecedence(
            [Values(
                "S01E01.2x2",
                "S1E1.2012-02-02",
                "1 - 1.2x2",
                "1x1.0202",
                "01x01.0202",
                "0101.s2.e2",
                "s1.e1.s2.e2",
                "2012.01.01.Feb.02.2012",
                "Jan.01.2012.2x2")] string seasonEpisodeNumber)
        {
            CreateTestFile(Root, "Alpha." + seasonEpisodeNumber + ".avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            MatchesShow1(results[0]);
        }

        /// <summary>
        ///     Tests that the system is able to match to the correct show. Allows for variations on the name and alternate names.
        /// </summary>
        /// <param name="showName">
        ///     The show's name in the file name.
        /// </param>
        [Test]
        public void MatchesShow(
            [Values(
                "Alpha",
                "Alpha.Folder",
                "ALPHA_FOLDER",
                "ALPHA-FOLDER",
                "Alt Name",
                "AlT.Name",
                "Alt_Name",
                "alt-name",
                "Alpha Show",
                "Alpha.Show",
                "Alpha_Show",
                "Alpha-Show")] string showName)
        {
            CreateTestFile(Root, showName + ".S01E01.avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            MatchesShow1(results[0]);
        }

        /// <summary>
        ///     Tests that the program doesn't return matches for files that don't have a season/episode number or date in them.
        /// </summary>
        [Test]
        public void NotAnEpisode()
        {
            CreateTestFile(Root, "Alpha.avi");
            CreateTestFile(Root, "Alpha.S01E01.noext");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(0, results.Count, "There should be no results.");
        }

        /// <summary>
        ///     Tests the scanner is not scanning recursively when it shouldn't be.
        /// </summary>
        [Test]
        public void NotRecursiveScanning()
        {
            var sub = CreateTestDirectory(Root, "Sub");
            CreateTestFile(sub[0], "Alpha.S01E01.avi");
            var results = scanManager.Refresh(Root);
            Assert.AreEqual(0, results.Count, "There should be no results.");
        }

        /// <summary>
        ///     Tests that the scanner works recursively.
        /// </summary>
        [Test]
        public void RecursiveScanner()
        {
            var sub = CreateTestDirectory(Root, "Sub");
            CreateTestFile(sub[0], "Alpha.S01E01.avi");

            StorageProvider.Settings.RecurseSubdirectories = true;

            var results = scanManager.Refresh(Root);
            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            MatchesShow1(results[0]);
        }

        /// <summary>
        ///     Tests the refresh file counts function.
        /// </summary>
        [Test]
        public void RefreshFileCountsTest()
        {
            // Create the folder and files to test.
            var folders = CreateTestDirectory(Root, "Alpha Folder", "Beta Folder");
            CreateTestFile(folders[0], "Alpha.S01E01.avi", "Alpha.S01E01.Name.avi", "Alpha.S01E01-02.avi");
            TestShows.First(x => x.Name == "Beta Show").Episodes[0].FileCount = 1;

            // When save shows is called. Assert that each episode has the correct file count.
            StorageProvider.SaveShows(
                Arg.Do(
                    new Action<IEnumerable<TvShow>>(
                        x =>
                        {
                            var shows = x.ToList();
                            Assert.AreEqual(3, shows[0].Episodes[0].FileCount);
                            Assert.AreEqual(1, shows[0].Episodes[1].FileCount);
                            Assert.AreEqual(0, shows[1].Episodes[0].FileCount);
                        })));

            // Refresh the file counts.
            scanManager.RefreshFileCounts(new[] { Root });

            // Ensure that the call was made. If it was the delegate above will not have been run.
            StorageProvider.Received(1).SaveShows(Arg.Any<IEnumerable<TvShow>>());
        }

        /// <summary>
        ///     Tests the Refresh method can find a show with lots of different formats.
        /// </summary>
        /// <param name="seasonEpisodeNumber">
        ///     The season and episode number string format.
        /// </param>
        [Test]
        public void RefreshFindShow(
            [Values("S01E01", "S1E1", "1 - 1", "1x1", "01x01", "0101", "s1.e1", "2012.01.01", "Jan.01.2012")]
            string seasonEpisodeNumber)
        {
            CreateTestFile(Root, "Alpha." + seasonEpisodeNumber + ".avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result");
            MatchesShow1(results[0]);
        }

        /// <summary>
        ///     Tests the functionality to reset the show on a result. The show should rematch the episode to an episode in the new
        ///     show.
        /// </summary>
        [Test]
        public void ResetShow()
        {
            CreateTestFile(Root, "Alpha.S01E01.avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            MatchesShow1(results[0]);

            scanManager.ResetShow(results[0], TestShows.First(x => x.Name == "Beta Show"));
            Assert.AreEqual("Beta Show", results[0].Show.Name, "The show hasn't changed.");
            Assert.AreEqual("211", results[0].Episode.TvdbId, "The episode hasn't changed.");
        }

        /// <summary>
        ///     Tests the functionality to reset the show. In this case, the destination show doesn't have the same episode.
        /// </summary>
        [Test]
        public void ResetShowNoEpisode()
        {
            CreateTestFile(Root, "Alpha.S02E02.avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            scanManager.ResetShow(results[0], TestShows.First(x => x.Name == "Beta Show"));
            Assert.AreEqual("Beta Show", results[0].Show.Name, "The show hasn't changed.");
            Assert.AreEqual(null, results[0].Episode, "The episode shouldn't have been matched.");
        }

        /// <summary>
        ///     Tests the functionality to reset the show. In this case, the destination show doesn't have the same episode.
        /// </summary>
        [Test]
        public void ResetShowToNull()
        {
            CreateTestFile(Root, "Alpha.S02E02.avi");

            var results = scanManager.Refresh(Root);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            scanManager.ResetShow(results[0], null);
            Assert.AreEqual(null, results[0].Show, "The show hasn't changed.");
            Assert.AreEqual(null, results[0].Episode, "The episode shouldn't have been matched.");
        }

        /// <summary>
        ///     Tests the search new shows function.
        /// </summary>
        [Test]
        public void SearchShowTest()
        {
            var gamma = new TvShow { Name = "Gamma", TvdbId = 3, FolderName = "Gamma Folder" };
            var delta = new TvShow { Name = "Delta", TvdbId = 4, FolderName = "Delta Folder" };
            var delta2 = new TvShow { Name = "Delta2", TvdbId = 5, FolderName = "Delta2 Folder" };

            dataProvider.SearchShow("Gamma Folder").Returns(new List<TvShow> { gamma });
            dataProvider.SearchShow("Delta Folder").Returns(new List<TvShow> { delta, delta2 });

            // Create the directories that will be searched.
            CreateTestDirectory(Root, "Alpha Folder", "Beta Folder", "Gamma Folder", "Delta Folder");

            var results = scanManager.SearchNewShows(new[] { Root });

            // Assert that dataProvider.SearchShow was not called for Alpha and Beta since they already exist in storage.
            dataProvider.DidNotReceive().SearchShow("Alpha Folder");
            dataProvider.DidNotReceive().SearchShow("Beta Folder");

            // Assert that other shows where only searched once.
            dataProvider.Received(1).SearchShow("Gamma Folder");
            dataProvider.Received(1).SearchShow("Delta Folder");

            // Ensure that there were only 2 calls in total.
            dataProvider.ReceivedWithAnyArgs(2).SearchShow(Arg.Any<string>());

            // Ensure that the Gamma show was saved as there was only one result.
            StorageProvider.Received(1).SaveShow(gamma);

            // Should only have the Delta show in the results.
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(2, results["Delta Folder"].Count);

            // Check that the results are the shows that the data provider returned.
            Assert.AreEqual(delta, results["Delta Folder"][0]);
            Assert.AreEqual(delta2, results["Delta Folder"][1]);
        }
    }
}
