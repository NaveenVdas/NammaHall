using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Application.Handlers.Admin.Hall;

public sealed class AdminHallCreateHandler : IAdminHallCreateHandler
{
    private readonly IHallRepository _hallRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminHallCreateHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork)
    {
        _hallRepository = hallRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(AdminHallCreateModel model, CancellationToken ct)
    {
        var hall = new Domain.DomainModel.HallAggregate.Hall
        {
            Name = model.Name,
            AddressLine = model.AddressLine,
            Village = model.Village,
            Taluk = model.Taluk,
            District = model.District,
            PostalCode = model.PostalCode,
            Capacity = model.Capacity,
            PriceFrom = model.PriceFrom,
            PriceTo = model.PriceTo,
            IsAc = model.IsAc,
            HasDiningHall = model.HasDiningHall,
            HasParking = model.HasParking,
            HasRooms = model.HasRooms,
            OwnerName = model.OwnerName,
            OwnerPhone = model.OwnerPhone,
            AlternatePhone = model.AlternatePhone,
            GoogleMapsUrl = model.GoogleMapsUrl,
            IsPublished = model.IsPublished,
            IsVerified = model.IsVerified,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        // Add images
        int displayOrder = 0;
        foreach (var imageUrl in model.ImageUrls)
        {
            hall.Images.Add(new HallImage
            {
                ImageUrl = imageUrl,
                DisplayOrder = displayOrder++,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = DateTime.UtcNow
            });
        }

        _hallRepository.Create(hall);
        await _unitOfWork.SaveChanges(ct);

        return hall.Id;
    }
}

