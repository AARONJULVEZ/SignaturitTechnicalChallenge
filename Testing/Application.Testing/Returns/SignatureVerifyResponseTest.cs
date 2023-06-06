using Application.Returns;
using Domain.Entities;
using NUnit.Framework;

namespace Application.Testing.Returns
{
    [TestFixture]
    public class SignatureVerifyResponseTest
    {
        [TestCase("K", "VV", "Plaintiff wins to the defendant")]
        [TestCase("VV", "N", "Plaintiff and defendant are in a draw")]
        [TestCase("V", "N", "Plaintiff loses to the defendant")]
        public void SignatureVerifyResponse_Create_returns_new_SignatureVerifyResponse(string plaintiff, string defendant, string recap)
        {
            var plaintiffContract = Contract.Create(plaintiff);

            Assert.True(plaintiffContract.IsCorrect);
            Assert.False(plaintiffContract.IsFail);
            Assert.IsNull(plaintiffContract.Error);

            var defendantContract = Contract.Create(defendant);

            Assert.True(defendantContract.IsCorrect);
            Assert.False(defendantContract.IsFail);
            Assert.IsNull(defendantContract.Error);

            var response = new SignatureVerifyResponse(plaintiffContract.Value, defendantContract.Value);

            Assert.IsNotNull(response);
            Assert.AreEqual(recap, response.Recap);
        }
    }
}
