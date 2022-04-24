using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using Xunit;

namespace Library.FileSeeder.Tests;

public class FileSeederTests
{
    private readonly FileSeeder _sut;

    public FileSeederTests()
    {
        _sut = new FileSeeder(new FileSeederConfiguration());
    }

    [Fact]
    public void Invoking_DefaultConfigurationShouldNotThrowException()
    {
        FluentActions.Invoking(() => new FileSeeder(new FileSeederConfiguration()))
            .Should()
            .NotThrow();
    }

    [Fact]
    public void Invoking_InsufficientPermissionsForPathShouldThrowUnauthorizedAccessException()
    {
        FluentActions.Invoking(() =>
                new FileSeeder(new FileSeederConfiguration() { Path = @"C:\Windows\System32" })
            )
            .Should()
            .Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void Invoking_InvalidPathShouldThrowDirectoryNotFoundException()
    {
        FluentActions.Invoking(() =>
                new FileSeeder(new FileSeederConfiguration() { Path = @"Z:\InvalidPath" })
            )
            .Should()
            .Throw<DirectoryNotFoundException>();
    }

    [Fact]
    public void Invoking_FileExtensionsCanNotBeEmpty()
    {
        FluentActions.Invoking(() =>
                new FileSeeder(
                    new FileSeederConfiguration() { FileExtensions = Array.Empty<string>()})
            )
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("FileExtensions can not be empty.");
    }
    
    [Fact]
    public void Invoking_FileExtensionsCanNotContainDot()
    {
        FluentActions.Invoking(() =>
                new FileSeeder(
                    new FileSeederConfiguration() { FileExtensions = new[] { ".pdf", ".doc"}})
            )
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("FileExtensions can not contain '.'.");
    }
    
    [Fact]
    public void Invoking_FileExtensionsCanNotBeLongerThanFourCharacters()
    {
        FluentActions.Invoking(() =>
                new FileSeeder(
                    new FileSeederConfiguration() { FileExtensions = new[] { "longerthanfourchars" } })
            )
            .Should()
            .Throw<ArgumentException>()
            .WithMessage("FileExtensions can not be longer than 4 characters.");
    }

    [Fact]
    public void GetRandomFileExtension()
    {
        var seeder = new FileSeeder(new FileSeederConfiguration() { FileExtensions = new[] { "doc", "pdf", "xls" } });
        seeder.GetRandomFileExtension().Should().BeOneOf("doc", "pdf", "xls");
    }

    [Theory]
    [InlineData(1000)]
    [InlineData(0)]
    public void SeedFile(int fileSizeInBytes)
    {
        var path = @$"{Path.GetTempPath()}1.tmp";
        FileInfo file = _sut.SeedFile(path, fileSizeInBytes);
        file.Length.Should().Be(fileSizeInBytes);
        file.Delete();
    }

    [Theory]
    [InlineData(10)]
    [InlineData(1)]
    [InlineData(0)]
    public void SeedFiles(int filesToSeed)
    {
        var seeder = new FileSeeder(new FileSeederConfiguration() { MaxFileSizeInMb = 1 });
        List<FileInfo> files = seeder.SeedFiles(filesToSeed);
        files.Count.Should().Be(filesToSeed);
        foreach (FileInfo file in files)
        {
            file.Delete();
        }
    }
}