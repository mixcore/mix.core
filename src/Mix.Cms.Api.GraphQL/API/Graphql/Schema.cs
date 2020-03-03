using GraphQL.Types;
using GraphQL;
using Api.Database;

namespace Api.Graphql 
{
  public class MySchema 
  {
    private ISchema _schema { get; set; }
    public ISchema GraphQLSchema 
    {  
      get 
      {
        return this._schema;
      }
    }
    
    public MySchema() 
    {
      this._schema = Schema.For(@"
          type Post {
            id: ID,
            title: String,
            image: String
          }

          type Book {
            id: ID
            name: String,
            genre: String,
            published: Date,
            Author: Author
          }

          type Author {
            id: ID,
            name: String,
            books: [Book]
          }

          type Mutation {
            addAuthor(name: String): Author
          }

          type Query {
              books: [Book],              
              posts(first: Int, offset:Int): [Post],
              post(id: Int): Post,
              author(id: ID): Author,
              authors: [Author]
              hello: String
          }
      ", _ =>
      {
        _.Types.Include<Query>();
        _.Types.Include<Mutation>();
      });
    }

  }
}