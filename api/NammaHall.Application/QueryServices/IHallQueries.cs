using System.Collections.ObjectModel;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Application.QueryServices;

public interface IHallQueries
{
    Task<ReadOnlyCollection<Hall>> GetHalls(HallSearchParameters parameters, CancellationToken ct);
    Task<Hall?> GetHallById(int hallId, CancellationToken ct);
    Task<bool> CheckAvailability(int hallId, DateOnly date, CancellationToken ct);
}

public sealed class HallSearchParameters
{
    public string? District { get; init; }
    public string? Taluk { get; init; }
    public string? Village { get; init; }
    public DateOnly? Date { get; init; }
    public int? MinGuests { get; init; }
    public int? MaxGuests { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? Search { get; init; }
}

