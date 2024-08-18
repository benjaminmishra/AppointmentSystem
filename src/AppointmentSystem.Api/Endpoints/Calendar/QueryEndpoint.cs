using AppointmentSystem.Application;
using AppointmentSystem.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public class QueryEndpoint : Endpoint<QueryRequest,Results<Ok<QueryResponse>, NotFound, BadRequest, ProblemHttpResult>>
{
    private readonly GetAvailableSlotsQueryHandler _getAvailableSlotsQueryHandler;
    public QueryEndpoint(GetAvailableSlotsQueryHandler getAvailableSlotsQueryHandler)
    {
        _getAvailableSlotsQueryHandler = getAvailableSlotsQueryHandler;
    }

    public override void Configure()
    {
        Post("calendar/query");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<QueryResponse>, NotFound, BadRequest, ProblemHttpResult>> ExecuteAsync(QueryRequest request, CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(request.Date, "yyyy-MM-dd", out var day))
            return TypedResults.BadRequest();
        
        var result = await _getAvailableSlotsQueryHandler.HandleAsync(request.Language, request.Products, request.CustomerRating, cancellationToken);
        
        if (result.Value is AvaiableSlotsNotFoundError)
            return TypedResults.NotFound();

        if (result.Value is AvaiableSlotsExceptionError error)
            return TypedResults.Problem(detail: error.Message, statusCode: 500, title: "Unexpected Error");

        if (result.Value is AvailableSlotsRequestValidationError)
            return TypedResults.BadRequest();
        
        return TypedResults.Ok(new QueryResponse(result.AsT0));
    }
}