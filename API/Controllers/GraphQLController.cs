﻿using System;
using System.Linq;
using System.Threading.Tasks;
using API.Queries;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using Schema;

namespace API.Controllers
{
    [Route("graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _executer;
        private readonly ISchema _schema;
        private readonly IDocumentWriter _writer;

        public GraphQLController(ISchema schema, IDocumentExecuter documentExecuter, IDocumentWriter writer)
        {
            _executer = documentExecuter;
            _schema = schema;
            _writer = writer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null) {
                throw new ArgumentNullException(nameof(query));
            }

            var inputs = query.Variables.ToInputs();
            var queryToExecute = query.Query;


            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = queryToExecute,
                Inputs = inputs,
                OperationName = query.OperationName
            };

            var result = await _executer.ExecuteAsync(executionOptions).ConfigureAwait(false);


            if (result.Errors?.Count > 0)
                return BadRequest(result);
            else
                return Ok(result);

        }
    }
}
