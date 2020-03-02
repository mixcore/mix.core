using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Database;
using Api.Graphql;
using GraphQL;
using GraphQL.NewtonsoftJson;
using Microsoft.AspNetCore.Mvc;

namespace Mix.Cms.Api.GraphQL.Controllers
{
  [Route("api/graphql")]
  [ApiController]
  public class GraphqlController: ControllerBase 
  {
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] GraphQLQuery query) 
    {
      var schema = new MySchema();
      var inputs = query.Variables.ToInputs();

      var result = await new DocumentExecuter().ExecuteAsync(_ =>
      {
        _.Schema = schema.GraphQLSchema;
        _.Query = query.Query;
        _.OperationName = query.OperationName;
        _.Inputs = inputs;
      });

      if (result.Errors?.Count > 0)
      {
        return BadRequest(result.Errors);
      }

      return Ok(result);
    }
  }
}