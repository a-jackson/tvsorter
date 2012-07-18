// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScanManagerTests.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Test
{
    using System.Collections.Generic;
    using System.Globalization;
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
                            Name = "Test Show", 
                            AlternateNames = new List<string>(), 
                            TvdbId = "1234", 
                            Episodes = new List<Episode>()
                        };
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests the Refresh method.
        /// </summary>
        [Test]
        public void RefreshTests()
        {
            File.Create("TV" + Path.DirectorySeparatorChar + "Test.Show.S01E01.avi").Close();

            List<FileResult> results = this.scanManager.Refresh(string.Empty);

            Assert.AreEqual(1, results.Count, "There should be 1 result");
            Assert.AreEqual(results[0].Show, this.TestShows.First(), "The result should match the test show.");

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
                });

            // Create the test environment.
            Directory.CreateDirectory("TV");

            this.scanManager = new ScanManager(storage);
        }

        #endregion
    }
}