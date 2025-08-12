# Business Assumptions

This document outlines the key business assumptions, limitations, and simplifications made in the Retirement Calculator implementation. These assumptions were necessary to deliver a working solution within the time constraints while maintaining accuracy for the core use cases.

## Financial Assumptions

### Superannuation System Parameters

#### Super Guarantee Rate
- **Current Rate**: 11.5% of ordinary time earnings
- **Source**: Australian Taxation Office (current as of 2024)
- **Assumption**: Rate remains constant throughout projection period
- **Reality**: Rate is scheduled to increase to 12% in July 2025
- **Impact**: Slightly conservative projections for future years

#### Contribution Caps
- **Concessional Cap**: $27,500 annually (2024-25 financial year)
- **Source**: Australian Taxation Office
- **Assumption**: Cap remains constant (not indexed)
- **Reality**: Caps are typically indexed annually
- **Impact**: May overstate cap constraints in future years

#### Tax Rates
- **Contributions Tax**: 15% on all concessional contributions
- **Assumption**: Single flat rate applies to all income levels
- **Reality**: Division 293 tax (additional 15%) applies to high income earners (>$250,000)
- **Impact**: Overstates benefits for high-income earners

### Investment Assumptions

#### Expected Returns
- **Conservative**: 5.5% per annum
- **Balanced**: 7.5% per annum (default)
- **Growth**: 9.5% per annum
- **Source**: Industry averages for typical superannuation investment options
- **Assumption**: Constant returns without volatility
- **Reality**: Returns vary significantly year-to-year with market cycles
- **Impact**: Smooth projections may not reflect real-world experience

#### Return Calculation Methodology
- **Approach**: Annual compounding with mid-year contribution timing
- **Assumption**: Contributions made evenly throughout the year
- **Reality**: Most employer contributions are made monthly or quarterly
- **Impact**: Minimal difference in total returns

### Economic Assumptions

#### Salary Growth
- **Default Rate**: 3.0% per annum
- **Source**: Long-term average wage growth in Australia
- **Assumption**: Consistent growth throughout career
- **Reality**: Salary growth varies with career progression, economic cycles, job changes
- **Impact**: May understate growth for early-career or overstate for late-career

#### Inflation Rate
- **Default Rate**: 2.5% per annum
- **Source**: Reserve Bank of Australia inflation target (2-3% band)
- **Assumption**: Constant inflation rate
- **Reality**: Inflation varies with economic conditions
- **Impact**: Purchasing power calculations may be imprecise

## Employment Assumptions

### Career Continuity
- **Assumption**: Continuous employment from current age to retirement
- **Reality**: Career breaks for unemployment, parental leave, illness, study
- **Impact**: Overstates lifetime contributions for many individuals

### Income Stability
- **Assumption**: Steady salary growth without major disruptions
- **Reality**: Job changes, promotions, industry shifts, economic downturns
- **Impact**: May not reflect individual career trajectories

### Single Employer
- **Assumption**: Employer contributions follow standard Super Guarantee rules
- **Reality**: Some employers provide additional contributions, salary sacrifice options
- **Impact**: Conservative estimate for employees with enhanced packages

## Regulatory Assumptions

### Legislative Stability
- **Assumption**: Current superannuation rules remain unchanged
- **Reality**: Government policy changes affect caps, rates, tax treatment
- **Impact**: Projections may become outdated with legislative changes

### Tax System Stability
- **Assumption**: Current tax rates and structures continue
- **Reality**: Tax reforms may change contribution taxation, withdrawal rules
- **Impact**: Tax calculations may not reflect future policy environment

## Personal Financial Assumptions

### No Additional Contributions
- **Assumption**: Only employer contributions considered
- **Excluded**: 
  - Voluntary personal contributions
  - Salary sacrifice arrangements
  - Spouse contributions
  - Government co-contributions
- **Impact**: Conservative projections for individuals making additional contributions

### No Early Access
- **Assumption**: No withdrawals before retirement age
- **Reality**: Early access for hardship, first home buyers, medical conditions
- **Impact**: Overstates final balances for individuals with early withdrawals

### Single Superannuation Fund
- **Assumption**: All superannuation held in one fund with consistent fees/returns
- **Reality**: Multiple funds with different fee structures and investment options
- **Impact**: Simplified analysis may not reflect portfolio complexity

### No Insurance Premiums
- **Assumption**: No deduction for life/TPD insurance premiums
- **Reality**: Most superannuation funds charge insurance premiums
- **Impact**: Overstates net contributions and growth

## Calculation Simplifications

### Mathematical Approach
- **Method**: Discrete annual calculations
- **Assumption**: Annual compounding is sufficient accuracy
- **Reality**: Daily or monthly compounding may provide slightly higher returns
- **Impact**: Marginally conservative return calculations

### Timing Assumptions
- **Contribution Timing**: Mid-year for earnings calculation
- **Assumption**: Average timing approximates reality
- **Reality**: Actual contribution timing varies by employer
- **Impact**: Minimal impact on long-term projections

### Rounding and Precision
- **Approach**: Standard decimal precision for financial calculations
- **Assumption**: Rounding errors are negligible over projection period
- **Impact**: Minimal cumulative effect on final results

## Risk Factors Not Modeled

### Market Volatility
- **Excluded**: Sequence of returns risk, market crashes, bull/bear cycles
- **Impact**: Real experience will show much more variability

### Longevity Risk
- **Excluded**: Life expectancy considerations for retirement planning
- **Impact**: Projections don't address adequacy for retirement income needs

### Regulatory Risk
- **Excluded**: Future changes to superannuation system
- **Impact**: Projections may not reflect future policy environment

### Economic Risk
- **Excluded**: Recession, inflation spikes, productivity changes
- **Impact**: Economic cycles significantly affect real outcomes

## Known Limitations

### Conservative Bias
The calculator tends toward conservative estimates due to:
- Exclusion of additional contributions
- Constant contribution caps (not indexed)
- No modeling of enhanced employer packages
- No government co-contributions

### Optimistic Bias  
The calculator may be optimistic in:
- Assuming continuous employment
- Not deducting insurance premiums
- Using simplified tax calculations
- Assuming constant positive returns

### Scope Limitations
The calculator does not address:
- Retirement income needs assessment
- Age Pension eligibility and amounts
- Transition to retirement strategies
- Estate planning considerations
- Tax-effective withdrawal strategies

## Validation and Benchmarking

### Industry Comparison
- Default assumptions align with major super fund calculators
- Return assumptions match industry standards
- Tax calculations follow current ATO guidelines

### Sensitivity Analysis
Key assumptions most likely to affect outcomes:
1. **Investment returns** (±2% significantly changes final balance)
2. **Salary growth** (±1% materially affects contributions)
3. **Contribution caps** (indexation affects high earners)
4. **Employment continuity** (career breaks reduce final balance)

### Accuracy Expectations
- **Short-term (5-10 years)**: High accuracy for stable employment
- **Medium-term (10-20 years)**: Reasonable approximation with noted limitations
- **Long-term (20+ years)**: Directional guidance only due to compounding uncertainties

## Usage Guidelines

### Appropriate Use Cases
- Initial retirement planning estimates
- Comparing investment option scenarios
- Understanding impact of contribution caps
- Educational purposes about superannuation growth

### Inappropriate Use Cases
- Precise retirement income planning
- Legal or financial advice decisions
- Compliance with financial services regulations
- Basis for major financial decisions without professional advice

### Recommendations for Users
1. **Regular Updates**: Re-run calculations with current data annually
2. **Professional Advice**: Consult qualified financial advisors for comprehensive planning
3. **Multiple Scenarios**: Test different return and growth assumptions
4. **Conservative Planning**: Use conservative assumptions for important decisions

## Disclaimer

This calculator provides estimates only and should not be relied upon for financial planning decisions. Actual outcomes will vary based on individual circumstances, market performance, and regulatory changes. Users should seek professional financial advice for comprehensive retirement planning.

The assumptions documented here reflect the state of the Australian superannuation system as of 2024 and may become outdated as regulations, economic conditions, and personal circumstances change.