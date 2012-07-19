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
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    /// <summary>
    /// Tests for the TVShow class.
    /// </summary>
    [TestFixture]
    public class TvShowTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Tests the CreateNfoFile function.
        /// </summary>
        [Test]
        public void CreateNfoFile()
        {
            var show = new TvShow { FolderName = "Show", TvdbId = "1234" };
            var settings = new Settings
                {
                    DestinationDirectories =
                        new List<string>
                            {
                                "TestWorking" + Path.DirectorySeparatorChar + "TV1", 
                                "TestWorking" + Path.DirectorySeparatorChar + "TV2"
                            }
                };

            Directory.CreateDirectory(
                "TestWorking" + Path.DirectorySeparatorChar + "TV1" + Path.DirectorySeparatorChar + "Show");
            Directory.CreateDirectory("TestWorking" + Path.DirectorySeparatorChar + "TV2");

            show.CreateNfoFile(settings);

            string path = string.Format("TestWorking{0}TV1{0}Show{0}tvshow.nfo", Path.DirectorySeparatorChar);
            string path2 = string.Format("TestWorking{0}TV2{0}Show{0}tvshow.nfo", Path.DirectorySeparatorChar);
            Assert.That(File.Exists(path), "tvshow.nfo doesn't exist.");
            Assert.That(!File.Exists(path2), "tvshow.nfo does exist.");

            string contents = File.ReadAllText(path);
            Assert.AreEqual(contents, "http://thetvdb.com/?tab=series&id=1234&lid=7");

            Directory.Delete("TestWorking", true);
        }

        #endregion
    }
}