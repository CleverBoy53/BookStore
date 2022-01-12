using System;
using System.Linq;
using AutoMapper;
using BookStore.Application.BookOperations.Commands.CreateBook;
using BookStore.DBOperations;
using BookStore.Entities;
using FluentAssertions;
using TestSetup;
using Xunit;

namespace Application.BookOperations.Commands.CreateBook;

public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public CreateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    [Fact]
    public void WhenGivenBookTitleIsAlreadyExist_InvalidOperationException_ShouldBeReturn()
    {
        //arrange(Hazırlık)
        CreateBookCommand command = new CreateBookCommand(_context, _mapper);
        command.Model = new CreateBookModel{Title = "Dune"};
        //act(Çalıştırma)
        //assert(Doğrulama)
        FluentActions
        .Invoking(()=> command.Handle())
        .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Book is already exist!");
    }

    [Fact]
    public void WhenGivenInputsAreValid_Book_ShouldBeCreated()
    {
        //arrange(Hazırlık)
        CreateBookCommand command = new CreateBookCommand(_context, _mapper);
        var model = new CreateBookModel
        {
            Title = "WhenGivenInputsAreValid_Book_ShouldBeCreated",
            GenreId = 1,
            AuthorId = 1,
            PageCount = 100,
            PublishDate = DateTime.Now.Date.AddYears(-1)
        };
        command.Model = model;

        //act(Çalıştırma)
        CreateBookCommandValidator validator = new CreateBookCommandValidator();
        var result = validator.Validate(command);
        FluentActions.Invoking(()=> command.Handle()).Invoke();

        //assert(Doğrulama)
        var book = _context.Books.FirstOrDefault(x=> x.Title == model.Title);
        result.Errors.Count.Should().Be(0);
        book.Should().NotBeNull();
    }

}