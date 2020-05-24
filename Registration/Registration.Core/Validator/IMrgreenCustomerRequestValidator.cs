using Registration.Core.ApiContracts;

namespace Registration.Core.Validator
{
    public interface IMrgreenCustomerRequestValidator
    {
        string ValidatePutRequest(MrgreenCustomerPutRequest request);
        string ValidatePostRequest(MrgreenCustomerPostRequest request);

    }
}
