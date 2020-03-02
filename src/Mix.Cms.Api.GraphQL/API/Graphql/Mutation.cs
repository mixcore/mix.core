using Api.Database;
using GraphQL;

namespace Api.Graphql 
{
  [GraphQLMetadata("Mutation")]
  public class Mutation 
  {
    [GraphQLMetadata("addAuthor")]
    public Author Add(string name)
    {
      using(var db = new StoreContext()) 
      {
        var author = new Author(){ Name = name };
        db.Authors.Add(author);
        db.SaveChanges();
        return author;
      }
    }
  }
}