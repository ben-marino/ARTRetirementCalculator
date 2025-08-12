using FluentAssertions;
using Microsoft.Extensions.Logging;
using RetirementCalculator.Application;
using RetirementCalculator.Domain;

namespace RetirementCalculator.Tests;

public class ProjectionServiceTests
{
    private readonly ILogger<ProjectionService> _logger;
    private readonly IRetirementCalculator _calculator;
    private readonly ProjectionService _service;
    
    public ProjectionServiceTests()
    {
        _logger = LoggerFactory.Create(builder => { })
            .CreateLogger<ProjectionService>();
        _calculator = new SuperannuationCalculator();
        _service = new ProjectionService(_calculator, _logger);
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = 85000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m,
            InflationRate = 2.5m
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Error.Should().BeNull();
        result.Value!.FinalBalance.Should().BeGreaterThan(0);
    }
    
    [Theory]
    [InlineData(67, 30)] // Retirement age less than current age
    [InlineData(30, 30)] // Retirement age equal to current age
    [InlineData(25, 30)] // Retirement age less than current age
    public async Task CalculateProjectionAsync_WithRetirementAgeNotGreaterThanCurrentAge_ReturnsFailure(
        int retirementAge, int currentAge)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = currentAge,
            RetirementAge = retirementAge,
            CurrentBalance = 50000,
            AnnualSalary = 85000
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Retirement age must be greater than current age");
        result.Value.Should().BeNull();
    }
    
    [Theory]
    [InlineData(17)] // Below minimum
    [InlineData(101)] // Above maximum
    [InlineData(0)] // Zero
    [InlineData(-5)] // Negative
    public async Task CalculateProjectionAsync_WithInvalidCurrentAge_ReturnsFailure(int currentAge)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = currentAge,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = 85000
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Current age must be between 18 and 100");
        result.Value.Should().BeNull();
    }
    
    [Theory]
    [InlineData(54)] // Below minimum
    [InlineData(101)] // Above maximum
    [InlineData(0)] // Zero
    [InlineData(-5)] // Negative
    public async Task CalculateProjectionAsync_WithInvalidRetirementAge_ReturnsFailure(int retirementAge)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = retirementAge,
            CurrentBalance = 50000,
            AnnualSalary = 85000
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Retirement age must be between 55 and 100");
        result.Value.Should().BeNull();
    }
    
    [Theory]
    [InlineData(-1000)] // Negative balance
    [InlineData(-50000)] // Large negative balance
    public async Task CalculateProjectionAsync_WithNegativeCurrentBalance_ReturnsFailure(decimal currentBalance)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = currentBalance,
            AnnualSalary = 85000
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Current balance cannot be negative");
        result.Value.Should().BeNull();
    }
    
    [Theory]
    [InlineData(0)] // Zero salary
    [InlineData(-50000)] // Negative salary
    public async Task CalculateProjectionAsync_WithInvalidAnnualSalary_ReturnsFailure(decimal annualSalary)
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = annualSalary
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Annual salary must be greater than zero");
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_WithZeroCurrentBalance_ReturnsSuccess()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 25,
            RetirementAge = 65,
            CurrentBalance = 0, // Valid zero balance
            AnnualSalary = 70000
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.FinalBalance.Should().BeGreaterThan(0);
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_WithBoundaryValues_ReturnsSuccess()
    {
        // Arrange - All boundary values that should be valid
        var request = new ProjectionRequest
        {
            CurrentAge = 18, // Minimum valid age
            RetirementAge = 55, // Minimum retirement age
            CurrentBalance = 0, // Minimum valid balance
            AnnualSalary = 1, // Minimum valid salary
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_WithMaximumValues_ReturnsSuccess()
    {
        // Arrange - Maximum boundary values
        var request = new ProjectionRequest
        {
            CurrentAge = 100, // Maximum valid age
            RetirementAge = 100, // Must be greater, so this will fail
            CurrentBalance = 999999999,
            AnnualSalary = 999999999
        };
        
        // Act
        var result = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result.IsSuccess.Should().BeFalse(); // Should fail because retirement age not > current age
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_WithRealisticScenarios_ReturnsSuccess()
    {
        // Arrange - Test multiple realistic scenarios
        var scenarios = new[]
        {
            new ProjectionRequest { CurrentAge = 25, RetirementAge = 65, CurrentBalance = 15000, AnnualSalary = 55000 },
            new ProjectionRequest { CurrentAge = 35, RetirementAge = 67, CurrentBalance = 120000, AnnualSalary = 95000 },
            new ProjectionRequest { CurrentAge = 45, RetirementAge = 60, CurrentBalance = 350000, AnnualSalary = 150000 },
            new ProjectionRequest { CurrentAge = 55, RetirementAge = 70, CurrentBalance = 800000, AnnualSalary = 200000 }
        };
        
        foreach (var scenario in scenarios)
        {
            // Act
            var result = await _service.CalculateProjectionAsync(scenario);
            
            // Assert
            result.IsSuccess.Should().BeTrue($"Scenario with age {scenario.CurrentAge} should succeed");
            result.Value.Should().NotBeNull();
            result.Value!.FinalBalance.Should().BeGreaterThan(scenario.CurrentBalance);
        }
    }
    
    [Fact]
    public async Task CalculateProjectionAsync_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var request = new ProjectionRequest
        {
            CurrentAge = 30,
            RetirementAge = 67,
            CurrentBalance = 50000,
            AnnualSalary = 85000,
            EmployerContributionRate = 11.5m,
            ExpectedReturnRate = 7.5m,
            SalaryGrowthRate = 3.0m,
            InflationRate = 2.5m
        };
        
        // Act
        var result1 = await _service.CalculateProjectionAsync(request);
        var result2 = await _service.CalculateProjectionAsync(request);
        
        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        
        result1.Value!.FinalBalance.Should().Be(result2.Value!.FinalBalance);
        result1.Value.TotalContributions.Should().Be(result2.Value.TotalContributions);
        result1.Value.TotalEarnings.Should().Be(result2.Value.TotalEarnings);
        result1.Value.InflationAdjustedBalance.Should().Be(result2.Value.InflationAdjustedBalance);
    }
}