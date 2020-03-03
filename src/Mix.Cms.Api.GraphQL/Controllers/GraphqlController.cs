using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EntityGraphQL;
using EntityGraphQL.Schema;
using GraphQL;
//using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Api.Database;
using Api.Graphql;
using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Models.Cms;

namespace Mix.Cms.Api.GraphQL.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphqlController : ControllerBase
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

        //private readonly Schema graphQLSchema;
        //public GraphqlController()
        //{
        //    var dbContext = new MixCmsContext();
        //    var tableNameLookup = new TableNameLookup();
        //    var metaDatabase = new DatabaseMetadata(dbContext, tableNameLookup);
        //    var schema = new Schema { Query = new Mix.Cms.Api.GraphQL.Infrastructure.GraphQLQuery(dbContext, metaDatabase, tableNameLookup) };
        //    schema.Initialize();
        //    graphQLSchema = schema;
        //}
        //[HttpPost]
        //public async Task<ActionResult> Get([FromBody] GraphQLQuery query)
        //{
        //    var result = await new DocumentExecuter().ExecuteAsync(
        //        new ExecutionOptions()
        //        {
        //            Schema = graphQLSchema,
        //            Query = query.Query
        //        }
        //    ).ConfigureAwait(false); 
        //    if (result.Errors?.Count > 0)
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //    return Ok(result);
        //    //var json = new DocumentWriter(indent: true).Write(result.Data); return json;
        //}

        //private readonly MappedSchemaProvider<MixCmsContext> _schemaprovider;

        //public GraphqlController(MappedSchemaProvider<MixCmsContext> schemaprovider)
        //{
        //    this._schemaprovider = schemaprovider;
        //}

        //[HttpPost]
        //public ActionResult post([FromBody]QueryRequest query)
        //{
        //    try
        //    {
        //        using (MixCmsContext dbcontext = new MixCmsContext())
        //        {
        //            var results = dbcontext.QueryObject(query, _schemaprovider);
        //            // gql compile errors show up in results.errors
        //            if (results.Errors.Count == 0)
        //            {
        //                return Ok(results);
        //            }
        //            else
        //            {
        //                return BadRequest(results.Errors);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
    }
}