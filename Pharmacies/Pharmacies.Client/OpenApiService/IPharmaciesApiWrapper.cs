namespace Pharmacies.Client.OpenApiService;

public interface IPharmaciesApiWrapper
{
    Task<PharmacyDto> CreatePharmacy(PharmacyDto newPharmacy);
    Task<PriceDto> CreatePrice(PriceDto newPrice);
    Task<PositionDto> CreatePosition(PositionDto newPosition);
    Task DeletePharmacy(int id);
    Task DeletePrice(int id);
    Task DeletePosition(int id);
    Task<IList<PharmacyDto>> GetAllPharmacies();
    Task<IList<PriceDto>> GetAllPrices();
    Task<IList<PositionDto>> GetAllPositions();
    Task<PharmacyDto> GetPharmacy(int id);
    Task<PriceDto> GetPrice(int id);
    Task<PositionDto> GetPosition(int id);
    Task UpdatePharmacy(int id, PharmacyDto newPharmacy);
    Task UpdatePrice(int id, PriceDto newPrice);
    Task UpdatePosition(int id, PositionDto newPosition);
}