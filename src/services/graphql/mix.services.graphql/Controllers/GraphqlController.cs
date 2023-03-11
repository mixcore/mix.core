using GraphQL;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms;
using Mix.Mixdb.Entities;
using Mix.Services.Graphql.Lib;
using Mix.Services.Graphql.Lib.Models;

namespace Mix.Services.Graphql.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
    {
        private readonly ISchema graphQLSchema;
        public GraphqlController(ISchema schema)
        {
            graphQLSchema = schema;
        }

        [HttpPost]
        public async Task<IActionResult> Get([FromBody] QueryRequest query)
        {
            var result = await new DocumentExecuter().ExecuteAsync(
                new ExecutionOptions()
                {
                    Schema = graphQLSchema,
                    Query = query.Query
                }
            ).ConfigureAwait(false);
            if (result.Errors?.Count > 0)
            {
                return BadRequest(result.Errors);
            }
            return new ExecutionResultActionResult(result);
        }
    }
}
