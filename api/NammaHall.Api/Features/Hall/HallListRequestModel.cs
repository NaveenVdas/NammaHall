namespace NammaHall.Api.Features.Hall;

public sealed class HallListRequestModel
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

    public Application.QueryServices.HallSearchParameters ToSearchParameters() =>
        new()
        {
            District = District,
            Taluk = Taluk,
            Village = Village,
            Date = Date,
            MinGuests = MinGuests,
            MaxGuests = MaxGuests,
            MinPrice = MinPrice,
            MaxPrice = MaxPrice,
            Search = Search
        };
}

