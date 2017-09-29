// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileManagerTests.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Tests for the
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using TVSorter.Data;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Wrappers;

namespace TVSorter.Test
{
    /// <summary>
    ///     Tests for the <see cref="FileManager" />
    /// </summary>
    [TestFixture]
    public class FileManagerTests : ManagerTestBase
    {
        /// <summary>
        ///     Sets up the tests in the fixture.
        /// </summary>
        public override void Setup()
        {
            base.Setup();
            var dataProvider = Substitute.For<IDataProvider>();
            var scanManager = Substitute.For<IScanManager>();
            var fileResultManager = new FileResultManager(StorageProvider);
            fileManager = new FileManager(StorageProvider, dataProvider, scanManager, fileResultManager);
        }

        /// <summary>
        ///     The file manager the tests are performed on.
        /// </summary>
        private FileManager fileManager;

        /// <summary>
        ///     Tests the creation of the output directory.
        /// </summary>
        [Test]
        public void FileCopyMove()
        {
            var file = CreateTestFile(Root, "alpha.s01e01.avi")[0];
            var result = new FileResult { Checked = true, Show = TestShows.First(), InputFile = file };
            result.Episode = result.Show.Episodes.First();
            result.Episodes = new List<Episode> { result.Episode };

            IDirectoryInfo seasonOne = null;

            Root.GetFile(Arg.Any<string>())
                .Returns(
                    x =>
                    {
                        var path = x.Arg<string>();

                        // Check that the path is as expected
                        var expectedPath = string.Format(
                            "Alpha Folder{0}Season 1{0}Alpha.Show.S01E01.Episode.One.(1).avi",
                            Path.DirectorySeparatorChar);
                        Assert.AreEqual(expectedPath, path, "The path is incorrect.");

                        // Return the correct file.
                        var alphaFolder = CreateTestDirectory(Root, "Alpha Folder")[0];
                        seasonOne = CreateTestDirectory(alphaFolder, "Season 1")[0];
                        seasonOne.Exists.Returns(false);
                        var episodeFile = CreateTestFile(seasonOne, "Alpha.Show.S01E01.Episode.One.(1).avi")[0];
                        episodeFile.Exists.Returns(false);
                        return episodeFile;
                    });

            fileManager.ProcessFiles(new List<FileResult> { result }, FileManager.SortType.Copy, Root);

            // Check that seasonOne has been created.
            Assert.NotNull(seasonOne, "The Season One folder should have been created.");

            // Check that there was a call to its create method.
            seasonOne.Received(1).Create();

            // Copy the files.
            file.Received(1)
                .CopyTo(
                    string.Format(
                        "TV{0}Alpha Folder{0}Season 1{0}Alpha.Show.S01E01.Episode.One.(1).avi",
                        Path.DirectorySeparatorChar));

            // Check that the episode was saved.
            StorageProvider.Received(1).SaveEpisode(result.Episode);
            StorageProvider.ClearReceivedCalls();

            // Move the files
            fileManager.ProcessFiles(new List<FileResult> { result }, FileManager.SortType.Move, Root);

            // Should be one call to seasonOne Create since it should have been recreated.
            seasonOne.Received(1).Create();

            // Should be one call to MoveTo with the new directory path.
            file.Received(1)
                .MoveTo(
                    string.Format(
                        "TV{0}Alpha Folder{0}Season 1{0}Alpha.Show.S01E01.Episode.One.(1).avi",
                        Path.DirectorySeparatorChar));

            // Check that the episode was saved.
            StorageProvider.Received(1).SaveEpisode(result.Episode);
        }
    }
}
