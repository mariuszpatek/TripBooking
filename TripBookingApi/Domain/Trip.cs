using TripBookingApi.Shared;

namespace TripBookingApi.Domain;

public class Trip
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Country { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public int NumberOfSeats { get; private set; }
    public ICollection<Registration> Registrations { get; private set; }
   // public byte[] RowVersion { get; private set; } //TODO: Not working for in-memory database

    private Trip()
    {
        // Required by EF Core
    }
    
    public Trip(string name, string country, string description, DateTime startDate, int numberOfSeats)
    {
        Id = Guid.NewGuid();
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        NumberOfSeats = numberOfSeats;
        Registrations = new List<Registration>();
    }

    public void UpdateTrip(string name, string country, string description, DateTime startDate, int numberOfSeats)
    {
        Name = name;
        Country = country;
        Description = description;
        StartDate = startDate;
        NumberOfSeats = numberOfSeats;
    }
    
    public Result Register(string email)
    {
        if (IsEmailRegistered(email))
        {
            return Result.Fail("This email is already registered for the trip.");
        }

        Registrations.Add(new Registration(email, this));
        return Result.Ok();
    }
    
    private bool IsEmailRegistered(string email)
    {
        return Registrations.Any(r => r.Email == email);
    }
}