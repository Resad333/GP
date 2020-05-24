using Registration.Core.ApiContracts;
using Registration.Core.Entity;

namespace Registration.Core.Mapper
{
    public interface IRedbetCustomerRequestMapper
    {

        CustomerPostRequestResponse ToPostContract(Customer entity);
        Customer ToDomain(RedbetCustomerPostRequest redbetCustomerPostRequest);
        Customer ToDomain(RedbetCustomerPutRequest redbetCustomerPutRequest);
        RedbetCustomerPutRequestResponse ToPutContract(Customer entity);
        RedbetCustomerGetRequestResponse ToGetContract(Customer entity);
    }
}
