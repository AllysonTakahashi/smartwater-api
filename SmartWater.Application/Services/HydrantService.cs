using Microsoft.Extensions.Logging;
using SmartWater.Application.Interfaces;
using SmartWater.Domain.Entities;

namespace SmartWater.Application.Services;

public class HydrantService : IHydrantService
{
    private readonly IHydrantRepository _hydrantRepository;
    private readonly ILogger<HydrantService> _logger;

    public HydrantService(IHydrantRepository hydrantRepository, ILogger<HydrantService> logger)
    {
        _hydrantRepository = hydrantRepository;
        _logger = logger;
    }

    public Task<Hydrant> CreateAsync(string code, string location)
    {
        var hydrant = new Hydrant(code, location);
        _logger.LogInformation("Hydrant '{Code}' created at '{Location}'.", code, location);
        return _hydrantRepository.AddAsync(hydrant);
    }

    public Task<IReadOnlyList<Hydrant>> GetAllAsync()
        => _hydrantRepository.GetAllAsync();

    public Task<Hydrant?> GetByIdAsync(int id)
        => _hydrantRepository.GetByIdAsync(id);

    public async Task<HydrantInspection> AddInspectionAsync(
        int hydrantId, decimal pressure, decimal flowRate, string inspectedBy, string? notes)
    {
        var hydrant = await _hydrantRepository.GetByIdAsync(hydrantId);

        if (hydrant is null)
        {
            _logger.LogWarning("Inspection failed: hydrant {HydrantId} not found.", hydrantId);
            throw new KeyNotFoundException($"Hydrant {hydrantId} not found.");
        }

        var inspection = new HydrantInspection(hydrant.Id, pressure, flowRate, inspectedBy, notes);
        await _hydrantRepository.AddInspectionAsync(inspection);

        _logger.LogInformation(
            "Inspection added to hydrant {HydrantId} by '{InspectedBy}'. Pressure: {Pressure} mca.",
            hydrantId, inspectedBy, pressure);

        return inspection;
    }

    public Task<IReadOnlyList<HydrantInspection>> GetInspectionsByHydrantIdAsync(int hydrantId)
        => _hydrantRepository.GetInspectionsByHydrantIdAsync(hydrantId);
}
