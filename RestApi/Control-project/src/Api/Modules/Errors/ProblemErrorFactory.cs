using Application.Problems.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ProblemErrorFactory
{
    public static ObjectResult ToObjectResult(this ProblemException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                ProblemAlreadyExistsException => StatusCodes.Status409Conflict,
                ProblemNotFoundException => StatusCodes.Status404NotFound,
                ProblemUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Problem handler is not implemented.")
            }
        };
    }
}