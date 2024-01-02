using Core.ResultTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Endpoints;

public interface IErrorHandlerFactory
{
    IResult HandleFailure(Error[] errors);
}

public class DefaultErrorHandlerFactory : IErrorHandlerFactory
{
    public IResult HandleFailure(Error[] errors)
    {
        return Results.BadRequest(
            CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                errors));
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error[]? errors = null)
    {
        return new ProblemDetails
        {
            Title = title,
            Type = errors[0].Code,
            Detail = errors[1].Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}