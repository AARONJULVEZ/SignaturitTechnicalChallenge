using Application.Queries;
using NUnit.Framework;

namespace Application.Testing.Queries
{
    [TestFixture]
    public class FillMissingSignatureRequestTest
    {
        [TestCase("K#", "VV")]
        [TestCase("VV", "KN#")]
        public void FillMissingSignatureRequest_Create_returns_new_FillMissingSignatureRequest(string plaintiff, string defendant)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);
            Assert.False(request.IsFail);
            Assert.IsNull(request.Error);
        }

        [TestCase("K", "VV", 400, "A missing signature represented by # is needed to proceed")]
        [TestCase("VV", "KN", 400, "A missing signature represented by # is needed to proceed")]
        public void FillMissingSignatureRequest_returns_error_when_there_are_no_missing_signatures(string plaintiff, string defendant, int code, string error)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsFail);
            Assert.False(request.IsCorrect);
            Assert.IsNotNull(request.Error);
            Assert.IsTrue(request.Error.Code == code);
            Assert.IsTrue(request.Error.Message.Equals(error));
        }

        [TestCase("K#", "VV#", 400, "It can't be missing signatures from both parties")]
        [TestCase("V#V", "#KN", 400, "It can't be missing signatures from both parties")]
        public void FillMissingSignatureRequest_returns_error_when_both_parties_are_missing_signatures(string plaintiff, string defendant, int code, string error)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsFail);
            Assert.False(request.IsCorrect);
            Assert.IsNotNull(request.Error);
            Assert.IsTrue(request.Error.Code == code);
            Assert.IsTrue(request.Error.Message.Equals(error));
        }
    }
}
