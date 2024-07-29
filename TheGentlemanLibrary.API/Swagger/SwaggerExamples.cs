using Swashbuckle.AspNetCore.Filters;
using TheGentlemanLibrary.Application.Models.Authors.Commands;
using TheGentlemanLibrary.Application.Models.Books.Commands;
using TheGentlemanLibrary.Application.Models.Orders.Commands;
using TheGentlemanLibrary.Application.Models.Users.Commands;

namespace TheGentlemanLibrary.API.Swagger
{
    public class OrderRequestExamples : IMultipleExamplesProvider<CreateOrderCommand>
    {
        public IEnumerable<SwaggerExample<CreateOrderCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Standard Order",
                new CreateOrderCommand(Id: 0, UserId: 101, BookId: 201, Price: 29.99m));

            yield return SwaggerExample.Create(
                "Premium Book Order",
                new CreateOrderCommand(Id: 0, UserId: 102, BookId: 202, Price: 49.99m));

            yield return SwaggerExample.Create(
                "Budget Book Order",
                new CreateOrderCommand(Id: 0, UserId: 103, BookId: 203, Price: 9.99m));
        }
    }

    public class BookRequestExamples : IMultipleExamplesProvider<CreateBookCommand>
    {
        public IEnumerable<SwaggerExample<CreateBookCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Modern Novel",
                new CreateBookCommand(Id: 0, Title: "The Great Gatsby", Pages: 180, AuthorId: 301, DateRange: "20th century") { UserId = 2});

            yield return SwaggerExample.Create(
                "Classic Literature",
                new CreateBookCommand(Id: 0, Title: "Pride and Prejudice", Pages: 432, AuthorId: 302, DateRange: "19th century") { UserId = 2 });

            yield return SwaggerExample.Create(
                "Contemporary Book",
                new CreateBookCommand(Id: 0, Title: "The Midnight Library", Pages: 288, AuthorId: 303, DateRange: "2020") { UserId = 2 });
        }
    }

    public class AuthorRequestExamples : IMultipleExamplesProvider<CreateAuthorCommand>
    {
        public IEnumerable<SwaggerExample<CreateAuthorCommand>> GetExamples()
        {
            yield return SwaggerExample.Create(
                "Modern Author",
                new CreateAuthorCommand(
                    Id: 0,
                    Country: "United Kingdom",
                    Name: "J.K. Rowling",
                    Biography: "British author, best known for the Harry Potter series.",
                    DateRange: "1965 - present"));

            yield return SwaggerExample.Create(
                "Classic Author",
                new CreateAuthorCommand(
                    Id: 0,
                    Country: "United States",
                    Name: "Mark Twain",
                    Biography: "American writer, humorist, entrepreneur, publisher, and lecturer.",
                    DateRange: "1835 - 1910"));

            yield return SwaggerExample.Create(
                "Ancient Author",
                new CreateAuthorCommand(
                    Id: 0,
                    Country: "Greece",
                    Name: "Homer",
                    Biography: "Author of the Iliad and the Odyssey.",
                    DateRange: "8th century BC"));
        }
    }

    public class UserLoginRequestExample : IExamplesProvider<LoginCommand>
    {
        public LoginCommand GetExamples()
        {
            return new LoginCommand(Email: "Admin@gmail.com", Password: "Admin123!");
        }
    }
}
