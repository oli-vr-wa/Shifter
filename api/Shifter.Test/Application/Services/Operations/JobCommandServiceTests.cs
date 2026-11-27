using FluentAssertions;
using NSubstitute;
using Shifter.Application.DTOs.Operations;
using Shifter.Application.Interfaces.Repositories.Core;
using Shifter.Application.Interfaces.Repositories.Operations;
using Shifter.Application.Interfaces.Services.Locations;
using Shifter.Application.Services.Operations;
using Shifter.Core.Entities.Operations;

namespace Shifter.Test.Application.Services.Operations;

public class JobCommandServiceTests
{
    private readonly IJobRepository _jobRepository;
    private readonly IAddressHandler _addressHandler;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JobCommandService _sut;

    private readonly Guid _clientId;

    public JobCommandServiceTests()
    {
        _jobRepository = Substitute.For<IJobRepository>();
        _addressHandler = Substitute.For<IAddressHandler>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _unitOfWork.CommitAsync(Arg.Any<CancellationToken>()).Returns(1);

        _sut = new JobCommandService(_jobRepository, _addressHandler, _unitOfWork);
        _clientId = Guid.NewGuid();
    }

    [Fact]
    public async Task CreateAsync_WhenJobIsValid_AddsJobAndReturnsSuccess()
    {
        var jobDto = new JobDto(
            Guid.Empty,
            _clientId,
            "Test Job",
            "This is a test job.",
            "PO12345",
            JobPriority.Normal,
            JobStatus.Pending,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(7),
            null,
            null);

        var result = await _sut.CreateAsync(jobDto);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        _jobRepository.Received(1).Add(Arg.Any<Job>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenJobIsNull_ReturnsFailure()
    {
        JobDto jobDto = null!;
        var result = await _sut.CreateAsync(jobDto);
        result.IsSuccess.Should().BeFalse();
        _jobRepository.DidNotReceive().Add(Arg.Any<Job>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenClientIdIsEmpty_ReturnsFailure()
    {
        var jobDto = new JobDto(
            Guid.Empty,
            Guid.Empty, // Invalid client ID
            "Test Job",
            "This is a test job.",
            "PO12345",
            JobPriority.Normal,
            JobStatus.Pending,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(7),
            null,
            null);
        var result = await _sut.CreateAsync(jobDto);
        result.IsSuccess.Should().BeFalse();
        _jobRepository.DidNotReceive().Add(Arg.Any<Job>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenClientIdIsNull_AddsJobAndReturnsSuccess()
    {
        var jobDto = new JobDto(
            Guid.Empty,
            null, // Null client ID
            "Test Job",
            "This is a test job.",
            "PO12345",
            JobPriority.Normal,
            JobStatus.Pending,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(7),
            null,
            null);
        var result = await _sut.CreateAsync(jobDto);
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        _jobRepository.Received(1).Add(Arg.Any<Job>());
        await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateAsync_WhenJobTitleIsEmpty_ReturnsFailure()
    {
        var jobDto = new JobDto(
            Guid.Empty,
            _clientId, 
            string.Empty,   
            "This is a test job.",
            "PO12345",
            JobPriority.Normal,
            JobStatus.Pending,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            DateTime.UtcNow.AddDays(7),
            null,
            null);

        var result = await _sut.CreateAsync(jobDto);

        result.IsSuccess.Should().BeFalse();
        _jobRepository.DidNotReceive().Add(Arg.Any<Job>());
        await _unitOfWork.DidNotReceive().CommitAsync(Arg.Any<CancellationToken>());
    }
}
