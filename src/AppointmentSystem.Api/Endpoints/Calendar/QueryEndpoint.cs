using AppointmentSystem.Application.Queries;
using AppointmentSystem.Domain.Errors;
using AppointmentSystem.Domain.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AppointmentSystem.Api.Endpoints.Calendar;

public class QueryEndpoint : Endpoint<QueryRequest, Results<Ok<List<AvailableSlot>>, NotFound, BadRequest<string>, ProblemHttpResult>>
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

    public override async Task<Results<Ok<List<AvailableSlot>>, NotFound, BadRequest<string>, ProblemHttpResult>> ExecuteAsync(
        QueryRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Language))
            return TypedResults.BadRequest("Language cannot be empty or whitespace");

        if (!request.Products.Any())
            return TypedResults.BadRequest("At least one product must be specified");

        if (string.IsNullOrWhiteSpace(request.Rating))
            return TypedResults.BadRequest("Rating cannot be empty or whitespace");
        
        if (!DateOnly.TryParseExact(request.Date, InputDateFormat, out var day))
            return TypedResults.BadRequest($"Date can only be entered in {InputDateFormat}");
        
        if (day == DateOnly.MinValue || day == DateOnly.MaxValue)
            return TypedResults.BadRequest("Date is invalid");

        var result = await _getAvailableSlotsQueryHandler
            .HandleAsync(
                request.Language,
                request.Products,
            request.Rating,
                day,
                cancellationToken);

        if (result.Value is AvailableSlotsError error)
        {
            switch (error)
            {
                case AvailableSlotsExceptionError:
                    return TypedResults.Problem(detail: error.Message, statusCode: 500, title: "Unexpected Error");

                case AvailableSlotsRequestValidationError:
                    return TypedResults.BadRequest(error.Message);
            }
        }

        return TypedResults.Ok(result.AsT0);
    }
}