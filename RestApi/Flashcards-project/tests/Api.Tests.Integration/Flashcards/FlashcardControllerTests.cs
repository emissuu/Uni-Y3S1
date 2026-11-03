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
    private readonly Flashcard _createValidTestFlashcard;
    private readonly Flashcard _createTakenTestFlashcard;
    private readonly Flashcard _createInvalidTestFlashcard;
    private readonly Flashcard _updateValidTestFlashcard;
    private readonly Flashcard _updateTakenTestFlashcard;
    private readonly Flashcard _updateInvalidTestFlashcard;
    
    private readonly string _createFlashcardRoute;
    private readonly string _updateExistingFlashcardRoute;
    private readonly string _updateNonExistingFlashcardRoute;
    private readonly string _deleteExistingFlashcardRoute;
    private readonly string _deleteNonExistingFlashcardRoute;

    public FlashcardControllerTests(IntegrationTestWebFactory factory) : base(factory)
    {
        _createValidTestFlashcard = FlashcardsTestData.CreateValidTestFlashcard();
        _createTakenTestFlashcard = FlashcardsTestData.CreateTakenTestFlashcard();
        _createInvalidTestFlashcard = FlashcardsTestData.CreateInvalidTestFlashcard();
        _updateValidTestFlashcard = FlashcardsTestData.UpdateValidTestFlashcard();
        _updateTakenTestFlashcard = FlashcardsTestData.UpdateTakenTestFlashcard();
        _updateInvalidTestFlashcard = FlashcardsTestData.UpdateInvalidTestFlashcard();

        var nonExistentFlashcardId = FlashcardsTestData.NonExistentTestFlashcardId();
        
        _createFlashcardRoute = "flashcards";
        _updateExistingFlashcardRoute = $"flashcards/{_updateTakenTestFlashcard.Id}";
        _updateNonExistingFlashcardRoute = $"flashcards/{nonExistentFlashcardId}";
        _deleteExistingFlashcardRoute = $"flashcards/{_createTakenTestFlashcard.Id}";
        _deleteNonExistingFlashcardRoute = $"flashcards/{nonExistentFlashcardId}";
    }
    
    public async Task InitializeAsync()
    {
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
    public async Task ShouldCreateFlashcard()
    {
        var request = new CreateFlashcardDto(
            _createValidTestFlashcard.Question,
            _createValidTestFlashcard.Hint,
            _createValidTestFlashcard.Answer,
            _createValidTestFlashcard.Description
        );
        
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);

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
    public async Task ShouldNotCreateFlashcardBecauseTitleIsAlreadyTaken()
    {
        var request = new CreateFlashcardDto(
            _createTakenTestFlashcard.Question,
            _createTakenTestFlashcard.Hint,
            _createTakenTestFlashcard.Answer,
            _createTakenTestFlashcard.Description
        );
        
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);

        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        
        var dbFlashcard = await Context.Flashcards.SingleAsync(x => x.Question == _createTakenTestFlashcard.Question);
        dbFlashcard.Id.Value.Should().Be(_createTakenTestFlashcard.Id.Value);
    }

    [Fact]
    public async Task ShouldNotCreateFlashcardBecauseRequestIsInvalid()
    {
        var flashcardsInDb = await Context.Flashcards.CountAsync();
        var request = new CreateFlashcardDto(
            _createInvalidTestFlashcard.Question,
            _createInvalidTestFlashcard.Hint,
            _createInvalidTestFlashcard.Answer,
            _createInvalidTestFlashcard.Description);
        
        var response = await Client.PostAsJsonAsync(_createFlashcardRoute, request);
        
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var dbFlashcardCount = await Context.Flashcards.CountAsync();
        dbFlashcardCount.Should().Be(flashcardsInDb);
    }
    
    [Fact]
    public async Task ShouldUpdateFlashcard()
    {
        var request = new UpdateFlashcardDto(
            _updateValidTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
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
    public async Task ShouldNotUpdateFlashcardBecauseTitleAlreadyTaken()
    {
        var request = new UpdateFlashcardDto(
            _createTakenTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
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
        var request = new UpdateFlashcardDto(
            _updateInvalidTestFlashcard.Question,
            _updateInvalidTestFlashcard.Hint,
            _updateInvalidTestFlashcard.Answer,
            _updateInvalidTestFlashcard.Description,
            _updateInvalidTestFlashcard.Score,
            _updateInvalidTestFlashcard.DueDate);
        
        var response = await Client.PutAsJsonAsync(_updateExistingFlashcardRoute, request);
        
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
        var request = new UpdateFlashcardDto(
            _updateValidTestFlashcard.Question,
            _updateValidTestFlashcard.Hint,
            _updateValidTestFlashcard.Answer,
            _updateValidTestFlashcard.Description,
            _updateValidTestFlashcard.Score,
            _updateValidTestFlashcard.DueDate);
        
        var response = await Client.PutAsJsonAsync(_updateNonExistingFlashcardRoute, request);
        
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ShouldDeleteFlashcard()
    {
        var response = await Client.DeleteAsync(_deleteExistingFlashcardRoute);
        
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var dbFlashcard = await Context.Flashcards.FirstOrDefaultAsync(x => x.Id == _createTakenTestFlashcard.Id);
        dbFlashcard.Should().BeNull();
    }

    [Fact]
    public async Task ShouldNotDeleteFlashcardBecauseFlashcardNotFound()
    {
        var dbFlashcardsCount = await Context.Flashcards.CountAsync();
        var response = await Client.DeleteAsync(_deleteNonExistingFlashcardRoute);
        
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var dbFlashcardCount = await Context.Flashcards.CountAsync();
        dbFlashcardCount.Should().Be(dbFlashcardsCount);
    }
}