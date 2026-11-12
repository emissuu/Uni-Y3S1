using System.Net;
using System.Net.Http.Json;
using Api.Dtos;
using Domain.Flashcards;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Tests.Common;
using Tests.Data.Flashcards;
using Xunit;

namespace Api.Tests.Integration.Flashcards;

public class FlashcardControllerTests : BaseIntegrationTest, IAsyncLifetime
{
    private readonly Flashcard _getFirstTestFlashcard;
    private readonly Flashcard _getSecondTestFlashcard;
    private readonly Flashcard _createValidTestFlashcard;
    private readonly Flashcard _createTakenTestFlashcard;
    private readonly Flashcard _createInvalidTestFlashcard;
    private readonly Flashcard _updateValidTestFlashcard;
    private readonly Flashcard _updateTakenTestFlashcard;
    private readonly Flashcard _updateInvalidTestFlashcard;
    
    private readonly string _getAllFlashcardsRoute;
    private readonly string _getOneFlashcardRoute;
    private readonly string _getFilteredFlashcardsRoute;
    private readonly string _createFlashcardRoute;
    private readonly string _updateExistingFlashcardRoute;
    private readonly string _updateNonExistingFlashcardRoute;
    private readonly string _deleteExistingFlashcardRoute;
    private readonly string _deleteNonExistingFlashcardRoute;

    public FlashcardControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _getFirstTestFlashcard = FlashcardsTestData.GetFirstTestFlashcard();
        _getSecondTestFlashcard = FlashcardsTestData.GetSecondTestFlashcard();
        _createValidTestFlashcard = FlashcardsTestData.CreateValidTestFlashcard();
        _createTakenTestFlashcard = FlashcardsTestData.CreateTakenTestFlashcard();
        _createInvalidTestFlashcard = FlashcardsTestData.CreateInvalidTestFlashcard();
        _updateValidTestFlashcard = FlashcardsTestData.UpdateValidTestFlashcard();
        _updateTakenTestFlashcard = FlashcardsTestData.UpdateTakenTestFlashcard();
        _updateInvalidTestFlashcard = FlashcardsTestData.UpdateInvalidTestFlashcard();

        var nonExistentFlashcardId = FlashcardsTestData.NonExistentTestFlashcardId();

        _getAllFlashcardsRoute = "flashcards";
        _getOneFlashcardRoute = $"flashcards/{_getFirstTestFlashcard.Id}";
        _getFilteredFlashcardsRoute = $"flashcards/filter";
        _createFlashcardRoute = "flashcards";
        _updateExistingFlashcardRoute = $"flashcards/{_updateTakenTestFlashcard.Id}";
        _updateNonExistingFlashcardRoute = $"flashcards/{nonExistentFlashcardId}";
        _deleteExistingFlashcardRoute = $"flashcards/{_createTakenTestFlashcard.Id}";
        _deleteNonExistingFlashcardRoute = $"flashcards/{nonExistentFlashcardId}";
    }
    
    public async Task InitializeAsync()
    {
        await Context.Flashcards.AddAsync(_getFirstTestFlashcard);
        await Context.Flashcards.AddAsync(_getSecondTestFlashcard);
        await Context.Flashcards.AddAsync(_createTakenTestFlashcard);
        await Context.Flashcards.AddAsync(_updateTakenTestFlashcard);

        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }

    public async Task DisposeAsync()
    {
        Context.Flashcards.RemoveRange(Context.Flashcards);
        
        await Context.SaveChangesAsync();
        Context.ChangeTracker.Clear();
    }

    [Fact]
    public async Task ShouldGetAllFlashcards()
    {
        // Act
        var response = await Client.GetAsync(_getAllFlashcardsRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards.CountAsync();
        dbFlashcardsCount.Should().Be(responseDto.Count);
    }
    
    [Fact]
    public async Task ShouldGetOneFlashcard()
    {
        // Act
        var response = await Client.GetAsync(_getOneFlashcardRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<FlashcardDto>();
        responseDto.Should().NotBeNull();

        responseDto.Question.Should().Be(_getFirstTestFlashcard.Question);
        responseDto.Hint.Should().Be(_getFirstTestFlashcard.Hint);
        responseDto.Answer.Should().Be(_getFirstTestFlashcard.Answer);
        responseDto.Description.Should().Be(_getFirstTestFlashcard.Description);
        responseDto.Score.Should().Be(_getFirstTestFlashcard.Score);
    }

    [Fact]
    public async Task ShouldGetFlashcardsFilteredByQuestion()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: _getFirstTestFlashcard.Question,
            Hint: null,
            Answer: null,
            Description: null,
            Score: null,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards.Where(x => x.Question == _getFirstTestFlashcard.Question).CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.First().Id.Should().Be(_getFirstTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByHint()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: _getFirstTestFlashcard.Hint,
            Answer: null,
            Description: null,
            Score: null,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards.Where(x => x.Hint == _getFirstTestFlashcard.Hint).CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.First().Id.Should().Be(_getFirstTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByAnswer()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: null,
            Answer: _getSecondTestFlashcard.Answer,
            Description: null,
            Score: null,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards.Where(x => x.Answer == _getSecondTestFlashcard.Answer).CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.First().Id.Should().Be(_getSecondTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByDescription()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: null,
            Answer: null,
            Description: _getSecondTestFlashcard.Description,
            Score: null,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards.Where(x => x.Description == _getSecondTestFlashcard.Description).CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.First().Id.Should().Be(_getSecondTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByScore()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: null,
            Answer: null,
            Description: null,
            Score: 95,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();

        var dbFlashcardsCount = await Context.Flashcards.Where(x => x.Score == 95).CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.First().Id.Should().Be(_getFirstTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByDueDate()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: null,
            Answer: null,
            Description: null,
            Score: null,
            DueDateFrom: _getFirstTestFlashcard.DueDate.AddHours(-1),
            DueDateTo: _getSecondTestFlashcard.DueDate,
            CreatedAtFrom: null,
            CreatedAtTo: null);
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards
            .Where(x => x.DueDate >= request.DueDateFrom
                        && x.DueDate <= request.DueDateTo)
            .CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.Should().Contain(x => x.Id == _getFirstTestFlashcard.Id.Value);
        responseDto.Should().Contain(x => x.Id == _getSecondTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldGetFlashcardsFilteredByCreatedAt()
    {
        // Arrange
        var request = new SearchFlashcardDto(
            Question: null,
            Hint: null,
            Answer: null,
            Description: null,
            Score: null,
            DueDateFrom: null,
            DueDateTo: null,
            CreatedAtFrom: DateTime.UtcNow.AddMinutes(-10),
            CreatedAtTo: DateTime.UtcNow.AddMinutes(10));
        
        // Act
        var response = await Client.PostAsJsonAsync(_getFilteredFlashcardsRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<List<FlashcardDto>>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcardsCount = await Context.Flashcards
            .Where(x => x.CreatedAt >= request.CreatedAtFrom
                        && x.CreatedAt <= request.CreatedAtTo)
            .CountAsync();
        responseDto.Count.Should().Be(dbFlashcardsCount);
        responseDto.Should().Contain(x => x.Id == _getFirstTestFlashcard.Id.Value);
        responseDto.Should().Contain(x => x.Id == _getSecondTestFlashcard.Id.Value);
    }
    
    [Fact]
    public async Task ShouldCreateFlashcard()
    {
        // Arrange
        var request = new CreateFlashcardDto(
            _createValidTestFlashcard.Question,
            _createValidTestFlashcard.Hint,
            _createValidTestFlashcard.Answer,
            _createValidTestFlashcard.Description
        );
        
        // Act
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<FlashcardDto>();
        responseDto.Should().NotBeNull();
        
        var dbFlashcard = await Context.Flashcards.FirstOrDefaultAsync(
            x => x.Id == new FlashcardId(responseDto.Id));
        
        dbFlashcard.Should().NotBeNull();
        dbFlashcard.Question.Should().Be(_createValidTestFlashcard.Question);
        dbFlashcard.Hint.Should().Be(_createValidTestFlashcard.Hint);
        dbFlashcard.Answer.Should().Be(_createValidTestFlashcard.Answer);
        dbFlashcard.Description.Should().Be(_createValidTestFlashcard.Description);
        dbFlashcard.Score.Should().Be(_createValidTestFlashcard.Score);
    }

    [Fact]
    public async Task ShouldNotCreateFlashcardBecauseQuestionIsAlreadyTaken()
    {
        // Arrange
        var request = new CreateFlashcardDto(
            _createTakenTestFlashcard.Question,
            _createTakenTestFlashcard.Hint,
            _createTakenTestFlashcard.Answer,
            _createTakenTestFlashcard.Description
        );
        
        // Act
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var dbFlashcard = await Context.Flashcards.SingleAsync(x => x.Question == _createTakenTestFlashcard.Question);
        dbFlashcard.Id.Value.Should().Be(_createTakenTestFlashcard.Id.Value);
    }

    [Fact]
    public async Task ShouldNotCreateFlashcardBecauseRequestIsInvalid()
    {
        // Arrange
        var flashcardsInDb = await Context.Flashcards.CountAsync();
        var request = new CreateFlashcardDto(
            _createInvalidTestFlashcard.Question,
            _createInvalidTestFlashcard.Hint,
            _createInvalidTestFlashcard.Answer,
            _createInvalidTestFlashcard.Description);
        
        // Act
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var dbFlashcardCount = await Context.Flashcards.CountAsync();
        dbFlashcardCount.Should().Be(flashcardsInDb);
    }
    
    [Fact]
    public async Task ShouldUpdateFlashcard()
    {
        // Arrange
        var request = new UpdateFlashcardDto(
            _updateValidTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        
        var responseDto = await response.Content.ReadFromJsonAsync<FlashcardDto>();
        responseDto.Should().NotBeNull();
        responseDto.Id.Should().Be(_updateTakenTestFlashcard.Id.Value);
        
        var dbFlashcard = await Context.Flashcards.FirstOrDefaultAsync(x => x.Id == _updateTakenTestFlashcard.Id);
        dbFlashcard.Should().NotBeNull();
        dbFlashcard.Question.Should().Be(_updateValidTestFlashcard.Question);
        dbFlashcard.Hint.Should().Be(_updateValidTestFlashcard.Hint);
        dbFlashcard.Answer.Should().Be(_updateValidTestFlashcard.Answer);
        dbFlashcard.Description.Should().Be(_updateValidTestFlashcard.Description);
        dbFlashcard.Score.Should().Be(_updateValidTestFlashcard.Score);
    }

    [Fact]
    public async Task ShouldNotUpdateFlashcardBecauseQuestionAlreadyTaken()
    {
        // Arrange
        var request = new UpdateFlashcardDto(
            _createTakenTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var dbFlashcard = await Context.Flashcards.SingleAsync(x => x.Id == _updateTakenTestFlashcard.Id);
        dbFlashcard.Id.Value.Should().Be(_updateTakenTestFlashcard.Id.Value);
        dbFlashcard.Question.Should().Be(_updateTakenTestFlashcard.Question);
        dbFlashcard.Hint.Should().Be(_updateTakenTestFlashcard.Hint);
        dbFlashcard.Answer.Should().Be(_updateTakenTestFlashcard.Answer);
        dbFlashcard.Description.Should().Be(_updateTakenTestFlashcard.Description);
        dbFlashcard.Score.Should().Be(_updateTakenTestFlashcard.Score);
    }

    [Fact]
    public async Task ShouldNotUpdateFlashcardBecauseRequestIsInvalid()
    {
        // Arrange
        var request = new UpdateFlashcardDto(
            _updateInvalidTestFlashcard.Question,
            _updateInvalidTestFlashcard.Hint,
            _updateInvalidTestFlashcard.Answer,
            _updateInvalidTestFlashcard.Description,
            _updateInvalidTestFlashcard.Score,
            _updateInvalidTestFlashcard.DueDate);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var dbFlashcard = await Context.Flashcards.SingleAsync(x => x.Id == _updateTakenTestFlashcard.Id);
        dbFlashcard.Question.Should().Be(_updateTakenTestFlashcard.Question);
        dbFlashcard.Hint.Should().Be(_updateTakenTestFlashcard.Hint);
        dbFlashcard.Answer.Should().Be(_updateTakenTestFlashcard.Answer);
        dbFlashcard.Description.Should().Be(_updateTakenTestFlashcard.Description);
        dbFlashcard.Score.Should().Be(_updateTakenTestFlashcard.Score);
    }

    [Fact]
    public async Task ShouldNotUpdateFlashcardBecauseFlashcardNotFound()
    {
        // Arrange
        var request = new UpdateFlashcardDto(
            _updateValidTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        // Act
        var response = await Client.PutAsJsonAsync(_updateNonExistingFlashcardRoute, request);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteFlashcard()
    {
        // Act
        var response = await Client.DeleteAsync(_deleteExistingFlashcardRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbFlashcard = await Context.Flashcards.FirstOrDefaultAsync(x => x.Id == _createTakenTestFlashcard.Id);
        dbFlashcard.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotDeleteFlashcardBecauseFlashcardNotFound()
    {
        // Arrange
        var dbFlashcardsCount = await Context.Flashcards.CountAsync();
        
        // Act
        var response = await Client.DeleteAsync(_deleteNonExistingFlashcardRoute);
        
        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var dbFlashcardCount = await Context.Flashcards.CountAsync();
        dbFlashcardCount.Should().Be(dbFlashcardsCount);
    }
}