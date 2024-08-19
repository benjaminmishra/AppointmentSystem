using AppointmentSystem.Application.Queries;
using AppointmentSystem.Domain;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public class QueryEndpoint : Endpoint<QueryRequest,Results<Ok<QueryResponse>, NotFound, BadRequest<string>, ProblemHttpResult>>
{
    private const string InputDateFormat = "yyyy-MM-dd";
    
    private readonly IGetAvailableSlotsQueryHandler _getAvailableSlotsQueryHandler;
    
    public QueryEndpoint(IGetAvailableSlotsQueryHandler getAvailableSlotsQueryHandler)
    {
        _getAvailableSlotsQueryHandler = getAvailableSlotsQueryHandler;
    }

    public override void Configure()
    {
        Post("calendar/query");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<QueryResponse>, NotFound, BadRequest<string>, ProblemHttpResult>>ExecuteAsync(
        QueryRequest request, 
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(request.Date, InputDateFormat, out var day))
            return TypedResults.BadRequest($"Date can only be entered in {InputDateFormat}");

        var result = await _getAvailableSlotsQueryHandler
            .HandleAsync(
                request.Language, 
                request.Products,
            request.CustomerRating, 
                day, 
                cancellationToken);

        if (result.Value is AvaiableSlotsError error)
        {
            if (error is AvaiableSlotsNotFoundError)
                return TypedResults.NotFound();

            if (error is AvaiableSlotsExceptionError)
                return TypedResults.Problem(detail: error.Message, statusCode: 500, title: "Unexpected Error");

            if (error is AvailableSlotsRequestValidationError)
                return TypedResults.BadRequest(error.Message);
        }
        
        return TypedResults.Ok(new QueryResponse(result.AsT0));
    }
}