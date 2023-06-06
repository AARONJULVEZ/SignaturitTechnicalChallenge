using Application.Queries;
using NUnit.Framework;

namespace Application.Testing.Queries
{
    [TestFixture]
    public class SignatureVerifyRequestTest
    {
        [TestCase("K", "VV")]
        [TestCase("VV", "KN")]
        public void SignatureVerifyRequest_Create_returns_new_SignatureVerifyRequest(string plaintiff, string defendant)
        {
            var request = SignatureVerifyRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);
            Assert.False(request.IsFail);
            Assert.IsNull(request.Error);
        }

        [TestCase("K#", "VV", 400, "Plaintiff cannot have any missing signature")]
        [TestCase("VV", "KN#", 400, "Defendant cannot have any missing signature")]
        public void SignatureVerifyRequest_Create_returns_error_when_introduce_hastagh(string plaintiff, string defendant, int code, string error)
        {
            var request = SignatureVerifyRequest.Create(plaintiff, defendant);

            Assert.True(request.IsFail);
            Assert.False(request.IsCorrect);
            Assert.IsNotNull(request.Error);
            Assert.True(request.Error.Code == code);
            Assert.True(request.Error.Message.Equals(error));
        }
    }
}
