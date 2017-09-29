// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagerTestBase.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Base classes for the manager tests to provide common functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using TVSorter.Model;
using TVSorter.Storage;
using TVSorter.Wrappers;

namespace TVSorter.Test
{
    /// <summary>
    ///     Base classes for the manager tests to provide common functionality.
    /// </summary>
    public abstract class ManagerTestBase
    {
        /// <summary>
        ///     Gets the root directory;
        /// </summary>
        protected IDirectoryInfo Root { get; private set; }

        /// <summary>
        ///     Gets the mocked storage provider.
        /// </summary>
        protected IStorageProvider StorageProvider { get; private set; }

        /// <summary>
        ///     Gets the TVShow objects to use in the tests.
        /// </summary>
        protected IEnumerable<TvShow> TestShows
        {
            get
            {
                yield return new TvShow
                {
                    Name = "Alpha Show",
                    FolderName = "Alpha Folder",
                    TvdbId = 1,
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            EpisodeNumber = 1,
                            SeasonNumber = 1,
                            FirstAir = new DateTime(2012, 1, 1),
                            Name = "Episode One (1)",
                            TvdbId = "111"
                        },
                        new Episode
                        {
                            EpisodeNumber = 2,
                            SeasonNumber = 1,
                            FirstAir = new DateTime(2012, 1, 2),
                            Name = "Episode One (2)",
                            TvdbId = "112"
                        }
                    },
                    AlternateNames = new List<string> { "alt name", "alpha" }
                };
                yield return new TvShow
                {
                    Name = "Beta Show",
                    FolderName = "Beta Folder",
                    TvdbId = 2,
                    Episodes = new List<Episode>
                    {
                        new Episode
                        {
                            EpisodeNumber = 1,
                            SeasonNumber = 1,
                            FirstAir = new DateTime(2012, 2, 2),
                            Name = "Episode One",
                            TvdbId = "211"
                        }
                    },
                    AlternateNames = new List<string> { "beta" }
                };
            }
        }

        /// <summary>
        ///     Sets up the tests in the fixture.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            // Create a storage provider.
            StorageProvider = Substitute.For<IStorageProvider>();

            // Set Load TvShows to return the TestShows.
            StorageProvider.LoadTvShows().Returns(TestShows);

            // When LoadSettings is called, set the settings.
            StorageProvider.Settings.Returns(
                new Settings
                {
                    SourceDirectory = "TV",
                    RecurseSubdirectories = false,
                    FileExtensions = new List<string> { ".avi" },
                    DestinationDirectories = new List<string> { "TV" },
                    DefaultDestinationDirectory = "TV",
                    AddUnmatchedShows = true
                });

            Root = CreateTestDirectory(null, "TV")[0];
        }

        /// <summary>
        ///     Creates a test directory with the specified name under the TV directory.
        /// </summary>
        /// <param name="parent">
        ///     The parent directory.
        /// </param>
        /// <param name="names">
        ///     The names of the directories to create.
        /// </param>
        /// <returns>
        ///     The directory info.
        /// </returns>
        protected IDirectoryInfo[] CreateTestDirectory(IDirectoryInfo parent, params string[] names)
        {
            var directories = new IDirectoryInfo[names.Length];

            for (var i = 0; i < names.Length; i++)
            {
                var directory = Substitute.For<IDirectoryInfo>();
                directory.Name.Returns(names[i]);
                var fullName = names[i];
                if (parent != null)
                {
                    fullName = parent.FullName + Path.DirectorySeparatorChar + names[i];
                }

                directory.FullName.Returns(fullName);
                directory.Exists.Returns(true);
                directories[i] = directory;
            }

            if (parent != null)
            {
                parent.GetDirectories().Returns(directories);
            }

            return directories;
        }

        /// <summary>
        ///     Creates a test file with the specified name under the TV directory.
        /// </summary>
        /// <param name="directory">
        ///     The directory that the file is in.
        /// </param>
        /// <param name="names">
        ///     The names of the files to create.
        /// </param>
        /// <returns>
        ///     The files that have been created.
        /// </returns>
        protected IFileInfo[] CreateTestFile(IDirectoryInfo directory, params string[] names)
        {
            var files = new IFileInfo[names.Length];

            for (var i = 0; i < names.Length; i++)
            {
                var file = Substitute.For<IFileInfo>();
                file.Name.Returns(names[i]);
                var fullName = string.Concat(directory.FullName, Path.DirectorySeparatorChar, names[i]);
                file.FullName.Returns(fullName);
                file.Extension.Returns(names[i].Substring(names[i].LastIndexOf('.')));
                file.Exists.Returns(true);
                file.Directory.Returns(directory);
                files[i] = file;
            }

            directory.GetFiles().Returns(files);
            return files;
        }
    }
}
