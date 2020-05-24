using Registration.Core.ApiContracts;
using Registration.Core.Entity;

namespace Registration.Core.Mapper
{
    public interface IMrgreenCustomerRequestMapper
    {
        CustomerPostRequestResponse ToPostContract(Customer entity);
        Customer ToDomain(MrgreenCustomerPostRequest mrgreenCustomerPostRequest);
        Customer ToDomain(MrgreenCustomerPutRequest mrgreenCustomerPutRequest);
        MrgreenCustomerPutRequestResponse ToPutContract(Customer entity);
        MrgreenCustomerGetRequestResponse ToGetContract(Customer entity);
    }
}
