using FluentValidation;
using ContractAPI.Models;

public class ValidationContract : AbstractValidator<Contract>
{
    public ValidationContract()
    {
        RuleFor(contract => contract.PlayerId).GreaterThan(0);
        RuleFor(contract => contract.TeamId).GreaterThan(0);
        RuleFor(contact => contact.YearlySalary).GreaterThan(0);
        RuleFor(contact => contact.TransferFee).GreaterThan(0);
        RuleFor(contract => contract.EndAt).GreaterThan(contract => contract.StartAt);
    }
}