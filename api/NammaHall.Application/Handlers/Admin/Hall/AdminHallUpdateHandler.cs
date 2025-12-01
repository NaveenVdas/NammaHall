using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;
using NammaHall.Domain.DomainModel.HallAggregate;

namespace NammaHall.Application.Handlers.Admin.Hall;

public sealed class AdminHallUpdateHandler : IAdminHallUpdateHandler
{
    private readonly IHallRepository _hallRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminHallUpdateHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork)
    {
        _hallRepository = hallRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(int hallId, AdminHallUpdateModel model, CancellationToken ct)
    {
        var hall = await _hallRepository.GetByIdWithImages(hallId, ct);
        if (hall == null)
        {
            return false;
        }

        // Update hall properties
        hall.Name = model.Name;
        hall.AddressLine = model.AddressLine;
        hall.Village = model.Village;
        hall.Taluk = model.Taluk;
        hall.District = model.District;
        hall.PostalCode = model.PostalCode;
        hall.Capacity = model.Capacity;
        hall.PriceFrom = model.PriceFrom;
        hall.PriceTo = model.PriceTo;
        hall.IsAc = model.IsAc;
        hall.HasDiningHall = model.HasDiningHall;
        hall.HasParking = model.HasParking;
        hall.HasRooms = model.HasRooms;
        hall.OwnerName = model.OwnerName;
        hall.OwnerPhone = model.OwnerPhone;
        hall.AlternatePhone = model.AlternatePhone;
        hall.GoogleMapsUrl = model.GoogleMapsUrl;
        hall.IsPublished = model.IsPublished;
        hall.IsVerified = model.IsVerified;
        hall.UpdatedAtUtc = DateTime.UtcNow;

        // Update images - remove existing and add new
        hall.Images.Clear();
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

        _hallRepository.Update(hall);
        await _unitOfWork.SaveChanges(ct);

        return true;
    }
}

