// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagerTestBase.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Base classes for the manager tests to provide common functionality.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NSubstitute;

    using NUnit.Framework;

    using TVSorter.Storage;
    using TVSorter.Wrappers;

    /// <summary>
    /// Base classes for the manager tests to provide common functionality.
    /// </summary>
    public abstract class ManagerTestBase
    {
        #region Properties

        /// <summary>
        /// The root directory;
        /// </summary>
        protected IDirectoryInfo Root { get; private set; }

        /// <summary>
        /// The scan manager's settings.
        /// </summary>
        protected Settings Settings { get; private set; }

        /// <summary>
        /// The mocked storage provider.
        /// </summary>
        protected IStorageProvider StorageProvider { get; private set; }

        /// <summary>
        /// Gets the TVShow objects to use in the tests.
        /// </summary>
        protected IEnumerable<TvShow> TestShows
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
        /// Sets up the tests in the fixture.
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            // Create a storage provider.
            this.StorageProvider = Substitute.For<IStorageProvider>();

            // Set Load TvShows to return the TestShows.
            this.StorageProvider.LoadTvShows().Returns(this.TestShows);

            // When LoadSettings is called, set the settings.
            this.StorageProvider.When(x => x.LoadSettings(Arg.Any<Settings>())).Do(
                x =>
                    {
                        x.Arg<Settings>().SourceDirectory = "TV";
                        x.Arg<Settings>().RecurseSubdirectories = false;
                        x.Arg<Settings>().FileExtensions = new List<string> { ".avi" };
                        x.Arg<Settings>().DestinationDirectories = new List<string> { "TV" };
                        x.Arg<Settings>().DestinationDirectory = "TV";
                        this.Settings = x.Arg<Settings>();
                    });

            this.Root = this.CreateTestDirectory(null, "TV")[0];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a test directory with the specified name under the TV directory.
        /// </summary>
        /// <param name="parent">
        /// The parent directory.
        /// </param>
        /// <param name="names">
        /// The names of the directories to create.
        /// </param>
        /// <returns>
        /// The directory info.
        /// </returns>
        protected IDirectoryInfo[] CreateTestDirectory(IDirectoryInfo parent, params string[] names)
        {
            var directories = new IDirectoryInfo[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                var directory = Substitute.For<IDirectoryInfo>();
                directory.Name.Returns(names[i]);
                string fullName = names[i];
                if (parent != null)
                {
                    fullName = parent.FullName + Path.DirectorySeparatorChar + names[i];
                }

                directory.FullName.Returns(fullName);
                directories[i] = directory;
            }

            if (parent != null)
            {
                parent.GetDirectories().Returns(directories);
            }

            return directories;
        }

        /// <summary>
        /// Creates a test file with the specified name under the TV directory.
        /// </summary>
        /// <param name="directory">
        /// The directory that the file is in.
        /// </param>
        /// <param name="names">
        /// The names of the files to create.
        /// </param>
        /// <returns>
        /// The files that have been created.
        /// </returns>
        protected IFileInfo[] CreateTestFile(IDirectoryInfo directory, params string[] names)
        {
            var files = new IFileInfo[names.Length];

            for (int i = 0; i < names.Length; i++)
            {
                var file = Substitute.For<IFileInfo>();
                file.Name.Returns(names[i]);
                file.Extension.Returns(names[i].Substring(names[i].LastIndexOf('.')));
                file.Exists.Returns(true);
                files[i] = file;
            }

            directory.GetFiles().Returns(files);
            return files;
        }

        #endregion
    }
}