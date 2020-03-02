using System.Collections.Generic;
using GraphQL;
using System.Linq;
using Api.Database;

namespace Api.Graphql
{
  public class QueryExample
  {

    [GraphQLMetadata("books")]
    public IEnumerable<Book> GetBooks()
    {
      return Enumerable.Empty<Book>();
    }

    [GraphQLMetadata("authors")]
    public IEnumerable<Author> GetAuthors()
    {
      return Enumerable.Empty<Author>();
    }

    [GraphQLMetadata("author")]
    public Author GetAuthor(int id)
    {
      return null;
    }

    [GraphQLMetadata("hello")]
    public string GetHello()
    {
      return "World";
    }
  }
}