# Calculation Approach

## Core Algorithm

### Yearly Calculation Process
1. **Employer Contribution Calculation**: Calculate annual employer contribution based on salary and Super Guarantee rate (11.5%)
2. **Contribution Cap Application**: Apply concessional contribution cap of $27,500
3. **Contributions Tax**: Apply 15% tax on employer contributions (reducing net contribution to 85% of gross)
4. **Investment Earnings**: Calculate annual earnings based on opening balance plus half of annual contribution, multiplied by expected return rate
5. **Balance Update**: Add net contributions and earnings to opening balance
6. **Salary Growth**: Apply annual salary growth rate for next year's calculation

### Mathematical Formula
For each year `i`:
- `EmployerContribution[i] = min(Salary[i] * 0.115, 27500)`
- `NetContribution[i] = EmployerContribution[i] * 0.85`
- `Earnings[i] = (Balance[i-1] + NetContribution[i] / 2) * ReturnRate`
- `Balance[i] = Balance[i-1] + NetContribution[i] + Earnings[i]`
- `Salary[i+1] = Salary[i] * (1 + SalaryGrowthRate)`

## Key Assumptions

### Financial Parameters
- **Super Guarantee rate**: 11.5% (current rate as of 2024)
- **Default investment return**: 7.5% p.a. (balanced investment option)
- **Default salary growth**: 3% p.a. (above inflation growth)
- **Default inflation**: 2.5% p.a.
- **Contributions tax**: 15% on concessional contributions
- **Concessional contribution cap**: $27,500 annually

### Calculation Methodology
- **Earnings timing**: Assumes contributions are made mid-year for earnings calculation
- **Compounding**: Annual compounding of investment returns
- **Inflation adjustment**: Applied at end of projection period using compound inflation rate
- **No career breaks**: Assumes continuous employment and contributions
- **Constant rates**: All rates (return, salary growth, inflation) remain constant throughout projection period

## Known Limitations

### Not Implemented (Scope Limitations)
- **Government co-contributions**: Additional government matching contributions not included
- **Spouse contributions**: Spouse contribution strategies not modeled
- **Insurance premiums**: Life/TPD insurance premium deductions not applied
- **Division 293 tax**: Additional 15% tax on high income earners (>$250k) not implemented
- **Market volatility**: No modeling of investment return variability
- **Regulatory changes**: Future changes to Super Guarantee rates or caps not projected

### Simplifying Assumptions
- **Steady employment**: No periods of unemployment or reduced income
- **No voluntary contributions**: Additional voluntary contributions not modeled
- **No withdrawals**: No early access or partial withdrawals before retirement
- **Single fund**: Assumes all super is held in one fund with consistent fees and returns
- **No pension phase**: Retirement income streams and pension phase calculations not included

## Future Enhancements

### Potential Improvements
1. **Stochastic modeling**: Monte Carlo simulations for return variability
2. **Multiple scenarios**: Conservative/balanced/aggressive investment option modeling
3. **Government benefits**: Include Age Pension and co-contribution calculations
4. **Tax optimization**: Model salary sacrifice and contribution splitting strategies
5. **Dynamic assumptions**: Allow for changing rates over time based on economic cycles

### Technical Enhancements
1. **Monthly calculations**: More granular compounding for increased accuracy
2. **Fee modeling**: Include management fees and insurance premium deductions
3. **Regulatory updates**: Dynamic loading of current contribution caps and tax rates
4. **Validation**: Enhanced input validation and range checking