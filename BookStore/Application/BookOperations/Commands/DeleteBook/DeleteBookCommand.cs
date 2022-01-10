using BookStore.DBOperations;

namespace BookStore.Application.BookOperations.Commands.DeleteBook;

public class DeleteBookCommand
{
    private readonly BookStoreDbContext _dbContext;

    public int Id { get; set; }

    public DeleteBookCommand(BookStoreDbContext dbContext) => _dbContext = dbContext;

    public void Handle()
    {
        var book = _dbContext.Books.FirstOrDefault(x => x.Id == Id);
        if (book is null)
            throw new InvalidOperationException("Book is not exist!");

        _dbContext.Books.Remove(book);
        _dbContext.SaveChanges();
    }
}