// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileManagerTests.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Tests for the <see cref="FileManager" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Test
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NSubstitute;

    using NUnit.Framework;

    using TVSorter.Files;
    using TVSorter.Wrappers;

    /// <summary>
    /// Tests for the <see cref="FileManager"/>
    /// </summary>
    [TestFixture]
    public class FileManagerTests : ManagerTestBase
    {
        #region Fields

        /// <summary>
        /// The file manager the tests are performed on.
        /// </summary>
        private FileManager fileManager;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Tests the creation of the output directory.
        /// </summary>
        [Test]
        public void DirectoryCreation()
        {
            this.CreateTestFile(this.Root, "alpha.s01e01.avi");
            var result = new FileResult
                {
                   Checked = true, Show = this.TestShows.First(), InputFile = this.GetFileInfo("alpha.s01e01.avi"), 
                };
            result.Episode = result.Show.Episodes.First();

            this.fileManager.CopyFile(new List<FileResult> { result });

            Assert.True(
                Directory.Exists(
                    "TV" + Path.DirectorySeparatorChar + "Alpha Folder" + Path.DirectorySeparatorChar + "Season 1"), 
                "The show's directory does not exist");
        }

        /// <summary>
        /// Sets up the tests in the fixture.
        /// </summary>
        public override void Setup()
        {
            base.Setup();

            this.fileManager = new FileManager(this.StorageProvider);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the file info for the specified file and path.
        /// </summary>
        /// <param name="names">
        /// The names of the file and it's directories.
        /// </param>
        /// <returns>
        /// The file info object.
        /// </returns>
        private IFileInfo GetFileInfo(params string[] names)
        {
            var fileInfo = Substitute.For<IFileInfo>();
            string path = "TV";
            path = names.Aggregate(path, (x, y) => x + Path.DirectorySeparatorChar + y);
            fileInfo.Name.Returns(path);
            return fileInfo;
        }

        #endregion
    }
}