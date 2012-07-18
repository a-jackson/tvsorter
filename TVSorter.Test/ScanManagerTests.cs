// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScanManagerTests.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NSubstitute;

    using NUnit.Framework;

    using TVSorter.Files;
    using TVSorter.Storage;

    /// <summary>
    /// Tests for the <see cref="ScanManager"/> class.
    /// </summary>
    [TestFixture]
    public class ScanManagerTests
    {
        #region Fields

        /// <summary>
        /// The scan manager that the tests will be performed on.
        /// </summary>
        private ScanManager scanManager;

        /// <summary>
        /// The scan manager's settings.
        /// </summary>
        private Settings settings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the TVShow objects to use in the tests.
        /// </summary>
        private IEnumerable<TvShow> TestShows
        {
            get
            {
                yield return
                    new TvShow
                        {
                            Name = "Alpha",
                            FolderName = "Alpha Folder",
                            TvdbId = "1",
                            Episodes =
                                new List<Episode>
                                    {
                                        new Episode
                                            {
                                                EpisodeNumber = 1,
                                                SeasonNumber = 1,
                                                FirstAir = new DateTime(2012, 1, 1),
                                                Name = "Episode One",
                                                TvdbId = "111"
                                            },
                                        new Episode
                                            {
                                                EpisodeNumber = 2,
                                                SeasonNumber = 1,
                                                FirstAir = new DateTime(2012, 1, 2),
                                                Name = "Episode Two",
                                                TvdbId = "112"
                                            }
                                    },
                            AlternateNames = new List<string> { "alt name" }
                        };
                yield return
                    new TvShow
                        {
                            Name = "Beta",
                            FolderName = "Beta Folder",
                            TvdbId = "2",
                            Episodes =
                                new List<Episode>
                                    {
                                        new Episode
                                            {
                                                EpisodeNumber = 1,
                                                SeasonNumber = 1,
                                                FirstAir = new DateTime(2012, 2, 2),
                                                Name = "Episode One",
                                                TvdbId = "211",
                                            }
                                    }
                        };
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests the Refresh method can find a show with lots of different formats.
        /// </summary>
        /// <param name="seasonEpisodeNumber">
        /// The season and episode number string format.
        /// </param>
        [Test]
        public void RefreshFindShow(
            [Values("S01E01", "S1E1", "1 - 1", "1x1", "01x01", "0101", "s1.e1", "2012.01.01", "Jan.01.2012")]
            string seasonEpisodeNumber)
        {
            this.CreateTestFile("Alpha." + seasonEpisodeNumber + ".avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result");
            this.MatchesShow1(results[0]);
        }

        /// <summary>
        /// Tests the precendece of the the regular expressions to ensure that the higher listed 
        /// match is used in the case of multiple matches.
        /// </summary>
        /// <param name="seasonEpisodeNumber">
        /// The season and episode number string format.
        /// </param>
        [Test]
        public void EpisodeFormatPrecedence(
            [Values("S01E01.2x2", "S1E1.2012-02-02", "1 - 1.2x2", "1x1.0202", "01x01.0202", 
                "0101.s2.e2", "s1.e1.s2.e2", "2012.01.01.Feb.02.2012", "Jan.01.2012.2x2")]
            string seasonEpisodeNumber)
        {
            this.CreateTestFile("Alpha." + seasonEpisodeNumber + ".avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.MatchesShow1(results[0]);
        }

        /// <summary>
        /// Tests that the program doesn't return matches for files 
        /// that don't have a season/episode number or date in them.
        /// </summary>
        [Test]
        public void NotAnEpisode()
        {
            this.CreateTestFile("Alpha.avi");
            this.CreateTestFile("Alpha.S01E01.noext");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(0, results.Count, "There should be no results.");
        }

        /// <summary>
        /// Tests that the system is able to match to the correct show.
        /// Allows for variations on the name and alternate names.
        /// </summary>
        /// <param name="showName">
        /// The show's name in the file name.
        /// </param>
        [Test]
        public void MatchesShow(
            [Values("Alpha", "Alpha.Folder", "ALPHA_FOLDER", "ALPHA-FOLDER", 
                "Alt Name", "AlT.Name", "Alt_Name", "alt-name")]string showName)
        {
            this.CreateTestFile(showName + ".S01E01.avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.MatchesShow1(results[0]);
        }

        /// <summary>
        /// Tests the functionality to reset the show on a result. The
        /// show should rematch the episode to an episode in the new show.
        /// </summary>
        [Test]
        public void ResetShow()
        {
            this.CreateTestFile("Alpha.S01E01.avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.MatchesShow1(results[0]);

            this.scanManager.ResetShow(results[0], this.TestShows.First(x => x.Name == "Beta"));
            Assert.AreEqual("Beta", results[0].Show.Name, "The show hasn't changed.");
            Assert.AreEqual("211", results[0].Episode.TvdbId, "The episode hasn't changed.");
        }

        /// <summary>
        /// Tests the functionality to reset the show.
        /// In this case, the destination show doesn't have the same episode.
        /// </summary>
        [Test]
        public void ResetShowNoEpisode()
        {
            this.CreateTestFile("Alpha.S02E02.avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.scanManager.ResetShow(results[0], this.TestShows.First(x => x.Name == "Beta"));
            Assert.AreEqual("Beta", results[0].Show.Name, "The show hasn't changed.");
            Assert.AreEqual(null, results[0].Episode, "The episode shouldn't have been matched.");
        }

        /// <summary>
        /// Tests the functionality to reset the show.
        /// In this case, the destination show doesn't have the same episode.
        /// </summary>
        [Test]
        public void ResetShowToNull()
        {
            this.CreateTestFile("Alpha.S02E02.avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.scanManager.ResetShow(results[0], null);
            Assert.AreEqual(null, results[0].Show, "The show hasn't changed.");
            Assert.AreEqual(null, results[0].Episode, "The episode shouldn't have been matched.");
        }

        /// <summary>
        /// Tests the scanner is not scanning recursively when it shouldn't be.
        /// </summary>
        [Test]
        public void NotRecursiveScanning()
        {
            this.CreateTestFile("Sub" + Path.DirectorySeparatorChar + "Alpha.S01E01.avi");
            List<FileResult> results = this.scanManager.Refresh(string.Empty);
            Assert.AreEqual(0, results.Count, "There should be no results.");
        }

        /// <summary>
        /// Tests that the scanner works recursively.
        /// </summary>
        [Test]
        public void RecursiveScanner()
        {
            this.settings.RecurseSubdirectories = true;
            this.CreateTestFile("Sub" + Path.DirectorySeparatorChar + "Alpha.S01E01.avi");

            List<FileResult> results = this.scanManager.Refresh(string.Empty);
            Assert.AreEqual(1, results.Count, "There should be 1 result.");
            this.MatchesShow1(results[0]);
        }
        
        /// <summary>
        /// Cleans up after the tests in the fixture.
        /// </summary>
        [TearDown]
        public void Teardown()
        {
            Directory.Delete("TV", true);
        }

        /// <summary>
        /// Sets up the tests in the fixture.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Create a storage provider.
            var storage = Substitute.For<IStorageProvider>();

            // Set Load TvShows to return the TestShows.
            storage.LoadTvShows().Returns(this.TestShows);

            // When LoadSettings is called, set the settings.
            storage.When(x => x.LoadSettings(Arg.Any<Settings>())).Do(
                x =>
                {
                    x.Arg<Settings>().SourceDirectory = "TV";
                    x.Arg<Settings>().RecurseSubdirectories = false;
                    x.Arg<Settings>().FileExtensions = new List<string> { ".avi" };
                    this.settings = x.Arg<Settings>();
                });

            // Create the test environment.
            Directory.CreateDirectory("TV" + Path.DirectorySeparatorChar + "Sub");

            this.scanManager = new ScanManager(storage);
        }

        /// <summary>
        /// Asserts the result matches the first show and its first episode.
        /// </summary>
        /// <param name="result">The result to check.</param>
        private void MatchesShow1(FileResult result)
        {
            Assert.AreEqual(this.TestShows.First(), result.Show, "The result should be the first test show.");
            Assert.AreEqual(
                this.TestShows.First().Episodes.First(),
                result.Episode,
                "The result should match the show's first episode.");
        }

        /// <summary>
        /// Creates a test file with the specified name under the TV directory.
        /// </summary>
        /// <param name="name">The name of the file to create.</param>
        private void CreateTestFile(string name)
        {
            File.Create("TV" + Path.DirectorySeparatorChar + name).Close();
        }

        #endregion
    }
}