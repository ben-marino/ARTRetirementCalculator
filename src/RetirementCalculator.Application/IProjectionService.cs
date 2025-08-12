using RetirementCalculator.Domain;

namespace RetirementCalculator.Application;

public interface IProjectionService
{
    Task<ServiceResult<ProjectionResult>> CalculateProjectionAsync(ProjectionRequest request);
}