using Application.Returns;
using Domain.Entities;
using NUnit.Framework;

namespace Application.Testing.Returns
{
    [TestFixture]
    public class FillMissingSignatureResponseTest
    {
        [TestCase("VV#", "VV", "Plaintiff needs the signature of a Validator to win", 'V')]
        [TestCase("VV#", "NV", "Plaintiff needs the signature of a Notary to win", 'N')]
        [TestCase("V#", "NV", "Plaintiff needs the signature of a King to win", 'K')]
        [TestCase("N", "V#V", "Defendant needs the signature of a Validator to win", 'V')]
        [TestCase("VN", "VV#", "Defendant needs the signature of a Notary to win", 'N')]
        [TestCase("VN", "#V", "Defendant needs the signature of a King to win", 'K')]
        public void FillMissingSignatureResponse_Create_returns_new_FillMissingSignatureResponse(string plaintiff, string defendant, string recap, char missingSignature)
        {
            var plaintiffContract = Contract.Create(plaintiff);

            Assert.True(plaintiffContract.IsCorrect);
            Assert.False(plaintiffContract.IsFail);
            Assert.IsNull(plaintiffContract.Error);

            var defendantContract = Contract.Create(defendant);

            Assert.True(defendantContract.IsCorrect);
            Assert.False(defendantContract.IsFail);
            Assert.IsNull(defendantContract.Error);

            var response = FillMissingSignatureResponse.Create(plaintiffContract.Value, defendantContract.Value);

            Assert.True(response.IsCorrect);
            Assert.False(response.IsFail);
            Assert.IsNull(response.Error);
            Assert.AreEqual(recap, response.Value.Recap);
            Assert.AreEqual(missingSignature, response.Value.MissingSignature);
        }

        [TestCase("KVN#", "V", 200, "The plaintiff wins without any extra signature needed")]
        [TestCase("K", "VNNNN#", 200, "The defendant wins without any extra signature needed")]
        public void FillMissingSignatureResponse_Create_returns_error_when_extra_signature_is_not_needed(string plaintiff, string defendant, int code, string error)
        {
            var plaintiffContract = Contract.Create(plaintiff);

            Assert.True(plaintiffContract.IsCorrect);
            Assert.False(plaintiffContract.IsFail);
            Assert.IsNull(plaintiffContract.Error);

            var defendantContract = Contract.Create(defendant);

            Assert.True(defendantContract.IsCorrect);
            Assert.False(defendantContract.IsFail);
            Assert.IsNull(defendantContract.Error);

            var response = FillMissingSignatureResponse.Create(plaintiffContract.Value, defendantContract.Value);

            Assert.True(response.IsFail);
            Assert.False(response.IsCorrect);
            Assert.IsNotNull(response.Error);
            Assert.True(code == response.Error.Code);
            Assert.AreEqual(error, response.Error.Message);
        }

        [TestCase("KVNN", "V#", 200, "The defendant needs more than one signature to win")]
        [TestCase("K#", "KVNNNN", 200, "The plaintiff needs more than one signature to win")]
        public void FillMissingSignatureResponse_Create_returns_error_when_more_than_one_extra_signature_is_needed(string plaintiff, string defendant, int code, string error)
        {
            var plaintiffContract = Contract.Create(plaintiff);

            Assert.True(plaintiffContract.IsCorrect);
            Assert.False(plaintiffContract.IsFail);
            Assert.IsNull(plaintiffContract.Error);

            var defendantContract = Contract.Create(defendant);

            Assert.True(defendantContract.IsCorrect);
            Assert.False(defendantContract.IsFail);
            Assert.IsNull(defendantContract.Error);

            var response = FillMissingSignatureResponse.Create(plaintiffContract.Value, defendantContract.Value);

            Assert.True(response.IsFail);
            Assert.False(response.IsCorrect);
            Assert.IsNotNull(response.Error);
            Assert.True(code == response.Error.Code);
            Assert.AreEqual(error, response.Error.Message);
        }
    }
}
