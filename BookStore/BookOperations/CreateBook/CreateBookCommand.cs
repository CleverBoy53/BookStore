using BookStore.DBOperations;

namespace BookStore.BookOperations.CreateBook;

public class CreateBookCommand
{
    private readonly BookStoreDbContext _dbContext;
    public CreateBookModel Model { get; set; }

    public CreateBookCommand(BookStoreDbContext dbContext) => _dbContext = dbContext;

    public void Handle()
    {
        var book = _dbContext.Books.FirstOrDefault(x => x.Title == Model.Title);
        if (book is not null)
            throw new InvalidOperationException("Book is already exist!");

        book = new Book
        {
            Title = Model.Title ?? "",
            GenreId = Model.GenreId,
            PageCount = Model.PageCount,
            PublishDate = Model.PublishDate
        };

        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();
    }
}

public class CreateBookModel
{
    public string? Title { get; set; }
    public int GenreId { get; set; }
    public int PageCount { get; set; }
    public DateTime PublishDate { get; set; }
}