using System.Collections.Generic;
using GraphQL;
using System.Linq;
using Api.Database;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models.Cms;

namespace Api.Graphql 
{
  public class Query
  {

    [GraphQLMetadata("books")]
    public IEnumerable<Book> GetBooks()
    {
      using(var db = new StoreContext())
      {
        return db.Books
        .Include(b => b.Author)
        .ToList();
      }
    }
        
    [GraphQLMetadata("posts")]
    public IEnumerable<Mix.Cms.Lib.Models.Cms.MixPost> GetPosts(int? first = 5, int? offset = 0)
    {
        using(var db = new MixCmsContext())
        {
            return db.MixPost.Skip(offset.Value).Take(first.Value).ToList();
        }
    }
    [GraphQLMetadata("post")]
    public Mix.Cms.Lib.Models.Cms.MixPost GetPost(int id)
    {
        using(var db = new MixCmsContext())
        {
            return db.MixPost.SingleOrDefault(m=>m.Id== id);
        }
    }

    [GraphQLMetadata("authors")]
    public IEnumerable<Author> GetAuthors() 
    {
      using (var db = new StoreContext())
      {
        return db.Authors
        .Include(a => a.Books)
        .ToList();
      }
    }

    [GraphQLMetadata("author")]
    public Author GetAuthor(int id)
    {
      using (var db = new StoreContext())
      {
        return db.Authors
        .Include(a => a.Books)
        .SingleOrDefault(a => a.Id == id);
      }
    }

    [GraphQLMetadata("hello")]
    public string GetHello()
    {
      return "World";
    }
  }
}