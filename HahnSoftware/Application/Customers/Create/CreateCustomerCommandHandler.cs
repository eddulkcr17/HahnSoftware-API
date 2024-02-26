using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using MediatR;

namespace Application.Customers.Create;

internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Unit>
{

    private readonly ICustomerRepository _customerRepository;

    private readonly IUnitWork _unitWork;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitWork unitWork)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        _unitWork = unitWork ?? throw new ArgumentNullException(nameof(unitWork));
    }

    public async Task<Unit> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
    {
        if(PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber){
            throw new ArgumentException(nameof(phoneNumber));
        }
        if(Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address){
            throw new ArgumentException(nameof(address));
        }
        var customer = new Customer(
            new CustomerId(Guid.NewGuid()),
            command.Name,
            command.LastName,
            command.Email,
            phoneNumber,
            address,
            true
        );

        await _customerRepository.Add(customer);
        await _unitWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}