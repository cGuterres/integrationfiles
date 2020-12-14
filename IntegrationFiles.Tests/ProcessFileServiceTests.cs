using FluentAssertions;
using IntegrationFile.Domain.Services.Implementation;
using IntegrationFiles.Domain.Configuration;
using IntegrationFiles.Domain.Constants;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IntegrationFiles.Tests
{
    [TestClass]
    public sealed class ProcessFileServiceTests
    {
        private ProcessFileService _service;
        private AppSettings _appSettings;

        [TestInitialize]
        public void Initialize()
        {
            _appSettings = new AppSettings
            {
                ImportPath = "C:\\HOMEPATH\\DATA\\IN\\",
                ExportPath = "C:\\HOMEPATH\\DATA\\OUT\\",
                FileExtension = ".dat"
            };

            _service = new ProcessFileService(Options.Create<AppSettings>(_appSettings));
        }

        [TestMethod]
        public void ProcessFileWhenDirectoryInNotExists()
        {
            // Arrage
            string message = Constants.DIRECTORY_NOT_FOUND;
            _appSettings.ImportPath = string.Empty;

            // Act
            var result = _service.ReadFiles();

            // Assert
            result.Should().Equals(message);
        }

        [TestMethod]
        public void ProcessFileWhenNotExistsFilesInDirectoryImport()
        {
            // Arrange
            string message = Constants.NO_FILE_TO_PROCESS;

            // Act
            var result = _service.ReadFiles();

            // Assert
            result.Should().Equals(message);
        }
    }
}
