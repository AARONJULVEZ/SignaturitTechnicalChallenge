using Application.Queries;
using Application.Returns;
using Domain.Common;
using Domain.Entities;

namespace Application.Services
{
    public class ContractsService
    {
        public Result<SignatureVerifyResponse> SignatureVerify(SignatureVerifyRequest request)
        {
            var plaintiffContract = Contract.Create(request.Plaintiff);
            if (plaintiffContract.IsFail)
            {
                return Result<SignatureVerifyResponse>.Fail(null, plaintiffContract.Error);
            }

            var defendantContract = Contract.Create(request.Defendant);
            if (defendantContract.IsFail)
            {
                return Result<SignatureVerifyResponse>.Fail(null, defendantContract.Error);
            }

            return new SignatureVerifyResponse(plaintiffContract.Value, defendantContract.Value);
        }

        public Result<FillMissingSignatureResponse> FillMissingSignature(FillMissingSignatureRequest request)
        {
            var plaintiffContract = Contract.Create(request.Plaintiff);
            if (plaintiffContract.IsFail)
            {
                return Result<FillMissingSignatureResponse>.Fail(null, plaintiffContract.Error);
            }

            var defendantContract = Contract.Create(request.Defendant);
            if (defendantContract.IsFail)
            {
                return Result<FillMissingSignatureResponse>.Fail(null, defendantContract.Error);
            }

            return FillMissingSignatureResponse.Create(plaintiffContract.Value, defendantContract.Value);
        }
    }
}
