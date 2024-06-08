using Microsoft.EntityFrameworkCore;
using TripBookingApi.Data;
using TripBookingApi.Domain;

namespace TripBookingApi.Repositories;

public class TripRepository : ITripRepository
{
    private readonly ApplicationDbContext _context;

    public TripRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Trip?> GetTripById(Guid id)
    {
        return await _context.Trips
            .Include(t => t.Registrations)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<List<Trip>> GetAllTrips()
    {
        return await _context.Trips.AsNoTracking().ToListAsync();
    }

    public async Task<List<Trip>> GetTripsByCountry(string country)
    {
        return await _context.Trips
            .Where(t => t.Country == country)
            .ToListAsync();
    }
    
    public async Task<bool> IsTripExists(string name)
    {
        return await _context.Trips
            .AnyAsync(t => t.Name == name);
    }

    public async Task AddTrip(Trip trip)
    {
        await _context.Trips.AddAsync(trip);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTrip(Trip trip)
    {
        _context.Trips.Update(trip);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTrip(Guid id)
    {
        var trip = await GetTripById(id);
        if (trip != null)
        {
            _context.Trips.Remove(trip);
            await _context.SaveChangesAsync();
        }
    }
}