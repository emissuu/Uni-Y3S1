using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Problems;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Problems;
using Xunit;

namespace Tests.Api.Integration.Problems;

public class ProblemControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Problem _getFirstTestProblem;
    private readonly Problem _getSecondTestProblem;
    private readonly Problem _createValidTestProblem;
    private readonly Problem _createTakenTestProblem;
    private readonly Problem _createInvalidTestProblem;
    private readonly Problem _updateValidTestProblem;
    private readonly Problem _updateTakenTestProblem;
    private readonly Problem _updateInvalidTestProblem;
    
    private readonly string _getAllProblemsRoute;
    private readonly string _getOneProblemRoute;
    private readonly string _createProblemRoute;
    private readonly string _updateExistingProblemRoute;
    private readonly string _updateNonExistingProblemRoute;
    private readonly string _deleteExistingProblemRoute;
    private readonly string _deleteNonExistingProblemRoute;

    public ProblemControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _getFirstTestProblem = ProblemsTestData.GetFirstTestProblem();
        _getSecondTestProblem = ProblemsTestData.GetSecondTestProblem();
        _createValidTestProblem = ProblemsTestData.CreateValidTestProblem();
        _createTakenTestProblem = ProblemsTestData.CreateTakenTestProblem();
        _createInvalidTestProblem = ProblemsTestData.CreateInvalidTestProblem();
        _updateValidTestProblem = ProblemsTestData.UpdateValidTestProblem();
        _updateTakenTestProblem = ProblemsTestData.UpdateTakenTestProblem();
        _updateInvalidTestProblem = ProblemsTestData.UpdateInvalidTestProblem();

        var nonExistentProblemId = ProblemsTestData.NonExistentTestProblemId();

        _getAllProblemsRoute = "problems";
        _getOneProblemRoute = $"problems/{_getFirstTestProblem.Id}";
        _createProblemRoute = "problems";
        _updateExistingProblemRoute = $"problems/{_updateTakenTestProblem.Id}";
        _updateNonExistingProblemRoute = $"problems/{nonExistentProblemId}";
        _deleteExistingProblemRoute = $"problems/{_createTakenTestProblem.Id}";
        _deleteNonExistingProblemRoute = $"problems/{nonExistentProblemId}";
    }
    
    public async Task InitializeAsync()
    {
        await Context.Problems.AddAsync(_getFirstTestProblem);
        await Context.Problems.AddAsync(_getSecondTestProblem);
        await Context.Problems.AddAsync(_createTakenTestProblem);
        await Context.Problems.AddAsync(_updateTakenTestProblem);

        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }

    public async Task DisposeAsync()
    {
        Context.Problems.RemoveRange(Context.Problems);
        
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }
    
    [Fact]
    public async Task ShouldGetAllProblems()
    {
        // Act
        var response = await Client.GetAsync(_getAllProblemsRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        dbProblems.Should().Contain(x => x.Id == _getFirstTestProblem.Id);
        dbProblems.Should().Contain(x => x.Id == _getSecondTestProblem.Id);
        dbProblems.Should().Contain(x => x.Id == _createTakenTestProblem.Id);
        dbProblems.Should().Contain(x => x.Id == _updateTakenTestProblem.Id);
        
    }

    [Fact]
    public async Task ShouldGetOneProblem()
    {
        // Act
        var response = await Client.GetAsync(_getOneProblemRoute);
        
        // Assert 
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProblemDto>();
        responseDto.Should().NotBeNull();
        
        responseDto.Title.Should().Be(_getFirstTestProblem.Title);
        responseDto.Code.Should().Be(_getFirstTestProblem.Code);
        responseDto.Answer.Should().Be(_getFirstTestProblem.Answer);
        responseDto.Category.Should().Be(_getFirstTestProblem.Category);
        responseDto.Difficulty.Should().Be(_getFirstTestProblem.Difficulty);
    }
    
    [Fact]
    public async Task ShouldCreateProblem()
    {
        // Arrange
        var request = new CreateProblemDto(
            _createValidTestProblem.Title,
            _createValidTestProblem.Code,
            _createValidTestProblem.Answer,
            _createValidTestProblem.Category,
            _createValidTestProblem.Difficulty
        );
        
        // Act
        var response = await Client.PostAsJsonAsync(_createProblemRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProblemDto>();
        responseDto.Should().NotBeNull();
        
        var dbProblem = await Context.Problems.FirstOrDefaultAsync(
            x => x.Id == new ProblemId(responseDto.Id));
        
        dbProblem.Should().NotBeNull();
        dbProblem.Title.Should().Be(_createValidTestProblem.Title);
        dbProblem.Code.Should().Be(_createValidTestProblem.Code);
        dbProblem.Answer.Should().Be(_createValidTestProblem.Answer);
        dbProblem.Category.Should().Be(_createValidTestProblem.Category);
        dbProblem.Difficulty.Should().Be(_createValidTestProblem.Difficulty);
    }
    
    [Fact]
    public async Task ShouldNotCreateProblemBecauseTitleIsAlreadyTaken()
    {
        // Arrange
        var request = new CreateProblemDto(
            _createTakenTestProblem.Title,
            _createTakenTestProblem.Code,
            _createTakenTestProblem.Answer,
            _createTakenTestProblem.Category,
            _createTakenTestProblem.Difficulty
        );
        
        // Act
        var response = await Client.PostAsJsonAsync(_createProblemRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var dbProblem = await Context.Problems.SingleAsync(x => x.Title == _createTakenTestProblem.Title);
        dbProblem.Id.Value.Should().Be(_createTakenTestProblem.Id.Value);
    }
    
    [Fact]
    public async Task ShouldNotCreateProblemBecauseRequestIsInvalid()
    {
        // Arrange
        var problemsInDb = await Context.Problems.CountAsync();
        var request = new CreateProblemDto(
            _createInvalidTestProblem.Title,
            _createInvalidTestProblem.Code,
            _createInvalidTestProblem.Answer,
            _createInvalidTestProblem.Category,
            _createInvalidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PostAsJsonAsync(_createProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var dbProblemCount = await Context.Problems.CountAsync();
        dbProblemCount.Should().Be(problemsInDb);
    }
    
    [Fact]
    public async Task ShouldUpdateProblem()
    {
        // Arrange
        var request = new UpdateProblemDto(
            _updateValidTestProblem.Title,
            _updateValidTestProblem.Code,
            _updateValidTestProblem.Answer,
            _updateValidTestProblem.Category,
            _updateValidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProblemDto>();
        responseDto.Should().NotBeNull();
        responseDto.Id.Should().Be(_updateTakenTestProblem.Id.Value);
        
        var dbProblem = await Context.Problems.FirstOrDefaultAsync(x => x.Id == _updateTakenTestProblem.Id);
        dbProblem.Should().NotBeNull();
        dbProblem.Title.Should().Be(_updateValidTestProblem.Title);
        dbProblem.Code.Should().Be(_updateValidTestProblem.Code);
        dbProblem.Answer.Should().Be(_updateValidTestProblem.Answer);
        dbProblem.Category.Should().Be(_updateValidTestProblem.Category);
        dbProblem.Difficulty.Should().Be(_updateValidTestProblem.Difficulty);
    }
    
    [Fact]
    public async Task ShouldUpdateProblemWithTheSameTitle()
    {
        // Arrange
        var request = new UpdateProblemDto(
            _updateTakenTestProblem.Title,
            _updateValidTestProblem.Code,
            _updateValidTestProblem.Answer,
            _updateValidTestProblem.Category,
            _updateValidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<ProblemDto>();
        responseDto.Should().NotBeNull();
        responseDto.Id.Should().Be(_updateTakenTestProblem.Id.Value);
        
        var dbProblem = await Context.Problems.FirstOrDefaultAsync(x => x.Id == _updateTakenTestProblem.Id);
        dbProblem.Should().NotBeNull();
        dbProblem.Title.Should().Be(_updateTakenTestProblem.Title);
        dbProblem.Code.Should().Be(_updateValidTestProblem.Code);
        dbProblem.Answer.Should().Be(_updateValidTestProblem.Answer);
        dbProblem.Category.Should().Be(_updateValidTestProblem.Category);
        dbProblem.Difficulty.Should().Be(_updateValidTestProblem.Difficulty);
    }

    [Fact]
    public async Task ShouldNotUpdateProblemBecauseTitleAlreadyTaken()
    {
        // Arrange
        var request = new UpdateProblemDto(
            _createTakenTestProblem.Title,
            _updateValidTestProblem.Code,
            _updateValidTestProblem.Answer,
            _updateValidTestProblem.Category,
            _updateValidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var dbProblem = await Context.Problems.SingleAsync(x => x.Id == _updateTakenTestProblem.Id);
        dbProblem.Title.Should().Be(_updateTakenTestProblem.Title);
        dbProblem.Code.Should().Be(_updateTakenTestProblem.Code);
        dbProblem.Answer.Should().Be(_updateTakenTestProblem.Answer);
        dbProblem.Category.Should().Be(_updateTakenTestProblem.Category);
        dbProblem.Difficulty.Should().Be(_updateTakenTestProblem.Difficulty);
    }

    [Fact]
    public async Task ShouldNotUpdateProblemBecauseRequestIsInvalid()
    {
        // Arrange
        var request = new UpdateProblemDto(
            _updateInvalidTestProblem.Title,
            _updateInvalidTestProblem.Code,
            _updateInvalidTestProblem.Answer,
            _updateInvalidTestProblem.Category,
            _updateInvalidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var dbProblem = await Context.Problems.SingleAsync(x => x.Id == _updateTakenTestProblem.Id);
        dbProblem.Title.Should().Be(_updateTakenTestProblem.Title);
        dbProblem.Code.Should().Be(_updateTakenTestProblem.Code);
        dbProblem.Answer.Should().Be(_updateTakenTestProblem.Answer);
        dbProblem.Category.Should().Be(_updateTakenTestProblem.Category);
        dbProblem.Difficulty.Should().Be(_updateTakenTestProblem.Difficulty);
    }
    
    [Fact]
    public async Task ShouldNotUpdateProblemBecauseProblemNotFound()
    {
        // Arrange
        var request = new UpdateProblemDto(
            _updateValidTestProblem.Title,
            _updateValidTestProblem.Code,
            _updateValidTestProblem.Answer,
            _updateValidTestProblem.Category,
            _updateValidTestProblem.Difficulty);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateNonExistingProblemRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task ShouldDeleteProblem()
    {
        // Act
        var response = await Client.DeleteAsync(_deleteExistingProblemRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbProblem = await Context.Problems.FirstOrDefaultAsync(x => x.Id == _createTakenTestProblem.Id);
        dbProblem.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotDeleteProblemBecauseProblemNotFound()
    {
        // Arrange
        var dbProblemsCount = await Context.Problems.CountAsync();
        
        // Act
        var response = await Client.DeleteAsync(_deleteNonExistingProblemRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var dbProblemCount = await Context.Problems.CountAsync();
        dbProblemCount.Should().Be(dbProblemsCount);
    }
}