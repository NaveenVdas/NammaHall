namespace NammaHall.Api.Features.Admin.Hall;

public sealed class AdminHallListRequestModel
{
    public string? Search { get; init; }
    public string? District { get; init; }
    public string? Taluk { get; init; }
    public string? Village { get; init; }
    public bool? IsPublished { get; init; }

    public Application.Handlers.Admin.Hall.AdminHallSearchParameters ToSearchParameters() =>
        new()
        {
            Search = Search,
            District = District,
            Taluk = Taluk,
            Village = Village,
            IsPublished = IsPublished
        };
}

