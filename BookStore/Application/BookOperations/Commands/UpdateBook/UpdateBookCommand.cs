using BookStore.DBOperations;

namespace BookStore.Application.BookOperations.Commands.UpdateBook;

public class UpdateBookCommand
{
    private readonly IBookStoreDbContext _dbContext;

    public int Id { get; set; }
    public UpdateBookModel Model { get; set; } = null!;

    public UpdateBookCommand(IBookStoreDbContext dbContext) => _dbContext = dbContext;

    public void Handle()
    {
        var book = _dbContext.Books.FirstOrDefault(x => x.Id == Id);
        if (book is null)
            throw new InvalidOperationException("Book is not exist!");

        book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;
        book.Title = Model.Title != default ? Model.Title : book.Title;

        _dbContext.SaveChanges();
    }
}