using FastEndpoints;
using FluentValidation;
using JetBrains.Annotations;
using TripBookingApi.Data;
using TripBookingApi.Repositories;

namespace TripBookingApi.Endpoints;

public class RegisterForTripRequest
{
    public string Email { get; set; }
}

[UsedImplicitly]
public class RegisterForTripEndpoint : Endpoint<RegisterForTripRequest>
{
    private readonly ITripRepository _repository;
    private readonly ApplicationDbContext _context;

    public RegisterForTripEndpoint(ITripRepository repository, ApplicationDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public override void Configure()
    {
        Post("/trips/{id:guid}/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterForTripRequest req, CancellationToken ct)
    {
        var tripId = Route<Guid>("id");
        var trip = await _repository.GetTripById(tripId);
        if (trip == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        
        var registrationResult = trip.Register(req.Email);

        if (registrationResult.IsFailure)
        {
            AddError(registrationResult.Error);
            await SendErrorsAsync(400, ct);
            return;
        }
        
        await _context.SaveChangesAsync(ct);
        
        await SendOkAsync(new { Message = "Registration successful" }, ct);
    }
}

[UsedImplicitly]
public class RegisterForTripRequestValidator : Validator<RegisterForTripRequest>
{
    public RegisterForTripRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("Invalid email format.");
    }
}