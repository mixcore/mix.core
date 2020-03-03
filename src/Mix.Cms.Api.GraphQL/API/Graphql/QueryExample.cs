using System.Collections.Generic;
using GraphQL;
using System.Linq;
using Api.Database;
using Mix.Cms.Lib.Models.Cms;

namespace Api.Graphql
{
  public class QueryExample
  {

    [GraphQLMetadata("books")]
    public IEnumerable<Book> GetBooks()
    {
      return Enumerable.Empty<Book>();
    }
    [GraphQLMetadata("posts")]
    public IEnumerable<MixPost> GetPosts()
    {
      return Enumerable.Empty<MixPost>();
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