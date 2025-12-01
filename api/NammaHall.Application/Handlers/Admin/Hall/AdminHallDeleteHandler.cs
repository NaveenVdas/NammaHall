using NammaHall.Application.Repository;
using NammaHall.Application.UnitOfWork;

namespace NammaHall.Application.Handlers.Admin.Hall;

public sealed class AdminHallDeleteHandler : IAdminHallDeleteHandler
{
    private readonly IHallRepository _hallRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminHallDeleteHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork)
    {
        _hallRepository = hallRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(int hallId, CancellationToken ct)
    {
        var hall = await _hallRepository.GetById(hallId, ct);
        if (hall == null)
        {
            return false;
        }

        _hallRepository.Delete(hall);
        await _unitOfWork.SaveChanges(ct);

        return true;
    }
}

