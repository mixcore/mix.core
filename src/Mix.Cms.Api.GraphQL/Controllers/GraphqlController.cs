using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Api.GraphQL.Infrastructure.Models;

namespace Mix.Cms.Api.GraphQL.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
    {
        private static readonly Schema graphQLSchema;
        static GraphqlController()
        {
            var dbContext = new MixCmsContext();
            var tableNameLookup = new TableNameLookup();
            var metaDatabase = new DatabaseMetadata(dbContext, tableNameLookup);
            var schema = new Schema { Query = new Mix.Cms.Api.GraphQL.Infrastructure.GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
            schema.Initialize();
            graphQLSchema = schema;
        }
        [HttpPost]
        public async Task<ActionResult> Get([FromBody] QueryRequest query)
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
            return Ok(result);
        }
    }
}