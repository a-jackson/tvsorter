// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowTests.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Tests for the TVShow class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Test
{
    using NSubstitute;

    using NUnit.Framework;

    using TVSorter.Wrappers;

    /// <summary>
    /// Tests for the TVShow class.
    /// </summary>
    [TestFixture]
    public class TvShowTests : ManagerTestBase
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests the CreateNfoFile function.
        /// </summary>
        [Test]
        public void CreateNfoFile()
        {
            // Create the test directory.
            var directory = Substitute.For<IDirectoryInfo>();

            // Set its name and full path.
            directory.Name.Returns("Alpha Folder");

            // Set the root directory to return alpha folder.
            this.Root.GetDirectories().Returns(new[] { directory });

            // Create the tvshow.nfo
            IFileInfo tvshowFile = this.CreateTestFile(directory, "tvshow.nfo")[0];

            // When WriteAllText is called. Assert that it is the correct text.
            tvshowFile.When(x => x.WriteAllText(Arg.Any<string>())).Do(
                x => Assert.AreEqual("http://thetvdb.com/?tab=series&id=1&lid=7", x.Arg<string>()));

            // When CreateFile is called, return the tvshowFile.
            directory.CreateFile("tvshow.nfo").Returns(x => tvshowFile);

            // Create Nfo files.
            TvShow.CreateNfoFiles(new[] { this.Root }, this.StorageProvider);

            // Assert that no other files were created or written to.
            directory.Received(1).CreateFile(Arg.Any<string>());
            tvshowFile.Received(1).WriteAllText(Arg.Any<string>());
        }

        #endregion
    }
}