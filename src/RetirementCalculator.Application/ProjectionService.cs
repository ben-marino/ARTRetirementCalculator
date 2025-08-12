using Microsoft.Extensions.Logging;
using RetirementCalculator.Domain;

namespace RetirementCalculator.Application;

public class ProjectionService : IProjectionService
{
    private readonly IRetirementCalculator _calculator;
    private readonly ILogger<ProjectionService> _logger;
    
    public ProjectionService(IRetirementCalculator calculator, ILogger<ProjectionService> logger)
    {
        _calculator = calculator;
        _logger = logger;
    }
    
    public async Task<ServiceResult<ProjectionResult>> CalculateProjectionAsync(ProjectionRequest request)
    {
        try
        {
            // Validate request
            if (request.CurrentAge >= request.RetirementAge)
                return ServiceResult<ProjectionResult>.Failure("Retirement age must be greater than current age");
            
            if (request.CurrentAge < 18 || request.CurrentAge > 100)
                return ServiceResult<ProjectionResult>.Failure("Current age must be between 18 and 100");
                
            if (request.RetirementAge < 55 || request.RetirementAge > 100)
                return ServiceResult<ProjectionResult>.Failure("Retirement age must be between 55 and 100");
                
            if (request.CurrentBalance < 0)
                return ServiceResult<ProjectionResult>.Failure("Current balance cannot be negative");
                
            if (request.AnnualSalary <= 0)
                return ServiceResult<ProjectionResult>.Failure("Annual salary must be greater than zero");
            
            // Calculate projection
            var result = await Task.Run(() => _calculator.CalculateProjection(request));
            
            _logger.LogInformation("Projection calculated for age {CurrentAge} to {RetirementAge}, final balance: {FinalBalance:C}", 
                request.CurrentAge, request.RetirementAge, result.FinalBalance);
            
            return ServiceResult<ProjectionResult>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating projection");
            return ServiceResult<ProjectionResult>.Failure("An error occurred calculating your projection");
        }
    }
}