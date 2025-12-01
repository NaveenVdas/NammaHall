using System.Collections.ObjectModel;

namespace NammaHall.Api.Features.Hall;

public sealed class HallListViewModel
{
    public ReadOnlyCollection<HallViewModel> Halls { get; init; } = new List<HallViewModel>().AsReadOnly();

    public HallListViewModel(ReadOnlyCollection<Domain.DomainModel.HallAggregate.Hall> halls)
    {
        Halls = halls.Select(h => new HallViewModel(h)).ToList().AsReadOnly();
    }
}

