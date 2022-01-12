using System;
using System.Linq;
using AutoMapper;
using BookStore.Application.BookOperations.Commands.UpdateBook;
using BookStore.DBOperations;
using BookStore.Entities;
using FluentAssertions;
using TestSetup;
using Xunit;

namespace Application.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public UpdateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGivenBookIsAlreadyExist_InvalidOperationExecption_ShouldBeReturn()
    {
        //arrange(Hazırlık)
        UpdateBookCommand command = new UpdateBookCommand(_context);
        command.Id = 100;
        command.Model = new UpdateBookModel { Title = "SampleBook" };
        //act(Çalıştırma)
        //assert(Doğrulama)
        FluentActions
        .Invoking(() => command.Handle())
        .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book is not exist!");
    }

    [Fact]
    public void WhenGivenInputsAreValid_Book_ShouldBeUpdated()
    {
        //arrange(Hazırlık)
        UpdateBookCommand command = new UpdateBookCommand(_context);
        var model = new UpdateBookModel { Title = "Hobbit", GenreId = 1 };
        command.Id = 1;
        command.Model = model;

        //act(Çalıştırma)
        UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
        var result = validator.Validate(command);
        FluentActions.Invoking(() => command.Handle()).Invoke();

        //assert(Doğrulama)
        var book = _context.Books.FirstOrDefault(x => x.Title == model.Title);
        result.Errors.Count.Should().Be(0);
        book.Should().NotBeNull();
    }

}