using Registration.Core.ApiContracts;

namespace Registration.Core.Validator
{
    public interface IRedbetCustomerRequestValidator
    {
        string ValidatePostRequest(RedbetCustomerPostRequest request);
        string ValidatePutRequest(RedbetCustomerPutRequest request);

    }
}
