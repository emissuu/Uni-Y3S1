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

public class ProblemControllerFilterTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Problem _getFirstTestProblem;
    private readonly Problem _getSecondTestProblem;
    private readonly Problem _createValidTestProblem;
    private readonly Problem _createTakenTestProblem;
    private readonly Problem _updateValidTestProblem;
    private readonly Problem _updateTakenTestProblem;
    
    private readonly string _getFilteredProblemsRoute;
    
    public ProblemControllerFilterTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _getFirstTestProblem = ProblemsTestData.GetFirstTestProblem();
        _getSecondTestProblem = ProblemsTestData.GetSecondTestProblem();
        _createValidTestProblem = ProblemsTestData.CreateValidTestProblem();
        _createTakenTestProblem = ProblemsTestData.CreateTakenTestProblem();
        _updateValidTestProblem = ProblemsTestData.UpdateValidTestProblem();
        _updateTakenTestProblem = ProblemsTestData.UpdateTakenTestProblem();
        
        _getFilteredProblemsRoute = $"problems/filter";
    }
    
    
    public async Task InitializeAsync()
    {
        await Context.Problems.AddAsync(_getFirstTestProblem);
        await Context.Problems.AddAsync(_getSecondTestProblem);
        await Context.Problems.AddAsync(_createTakenTestProblem);
        await Context.Problems.AddAsync(_createValidTestProblem);
        await Context.Problems.AddAsync(_updateTakenTestProblem);
        await Context.Problems.AddAsync(_updateValidTestProblem);

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
    public async Task ShouldGetProblemsFilteredByNothing()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByTitleToLowerCase()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: "gettable",
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Title.ToLower().Contains(request.Title!.ToLower())).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }

    [Fact]
    public async Task ShouldGetProblemsFilteredByTitleToUpperCase()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: "GETTABLE",
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Title.ToLower().Contains(request.Title!.ToLower())).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCodeToLowerCase()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: "return",
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Code.ToLower().Contains(request.Code!.ToLower())).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCodeToUpperCase()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: "RETURN",
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Code.ToLower().Contains(request.Code!.ToLower())).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByAnswer()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: "4",
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Answer.ToLower().Contains(request.Answer!.ToLower())).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCategory()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: Category.Math,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(x => x.Category == request.Category).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByDifficulty()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: Difficulty.Easy,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(x => x.Difficulty == request.Difficulty).ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCreatedAtFrom()
    {
        // Arrange
        var problem = await Context.Problems.ElementAtAsync(2);
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: problem.CreatedAt,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems
            .Where(x => x.CreatedAt >= request.CreatedAtFrom)
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCreatedAtTo()
    {
        // Arrange
        var problem = await Context.Problems.ElementAtAsync(4);
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: problem.CreatedAt);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems
            .Where(x => x.CreatedAt <= request.CreatedAtTo)
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCreatedAtFromAndCreateAtTo()
    {
        // Arrange
        var problemFrom = await Context.Problems.ElementAtAsync(2);
        var problemTo = await Context.Problems.ElementAtAsync(5);
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: null,
            Difficulty: null,
            CreatedAtFrom: problemFrom.CreatedAt,
            CreatedAtTo: problemTo.CreatedAt);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems
            .Where(x => x.CreatedAt >= request.CreatedAtFrom 
                        && x.CreatedAt <= request.CreatedAtTo)
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCategoryAndDifficulty()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: null,
            Answer: null,
            Category: Category.String,
            Difficulty: Difficulty.Easy,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Difficulty == request.Difficulty 
                 && x.Category == request.Category)
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetProblemsFilteredByCategoryAndCode()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: null,
            Code: "return",
            Answer: null,
            Category: Category.String,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
            x => x.Category == request.Category 
                 && x.Code.ToLower().Contains(request.Code!.ToLower()))
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
    
    [Fact]
    public async Task ShouldGetNoProblemsFilteredByTitleAndCategory()
    {
        // Arrange
        var request = new SearchProblemDto(
            Title: "gettable",
            Code: null,
            Answer: null,
            Category: Category.Math,
            Difficulty: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredProblemsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<ProblemDto>>();
        responseDto.Should().NotBeNull();
        
        var dbProblems = await Context.Problems.Where(
                x => x.Category == request.Category 
                     && x.Title.ToLower().Contains(request.Title!.ToLower()))
            .ToListAsync();
        dbProblems.Count.Should().Be(responseDto.Count);
        foreach (var dbProblem in dbProblems)
        {
            responseDto.Should().Contain(x => x.Id == dbProblem.Id.Value);
        }
    }
}