using Application.Queries;
using Application.Services;
using NUnit.Framework;

namespace Application.Testing.Services
{
    [TestFixture]
    public class ContractsServiceTest
    {
        private ContractsService _service;

        [SetUp]
        public void Setup()
        {
            _service = new ContractsService();
        }

        [TestCase("K", "VV", "Plaintiff wins to the defendant")]
        [TestCase("VV", "KN", "Plaintiff loses to the defendant")]
        [TestCase("VV", "N", "Plaintiff and defendant are in a draw")]
        public void SignatureVerify_returns_new_SignatureVerifyResponse(string plaintiff, string defendant, string recap)
        {
            var request = SignatureVerifyRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);

            var result = _service.SignatureVerify(request.Value);

            Assert.True(result.IsCorrect);
            Assert.False(result.IsFail);
            Assert.IsNull(result.Error);

            Assert.True(result.Value.Recap.Equals(recap));
        }

        [TestCase("K1", "VV", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        [TestCase("VV", "K1", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        public void SignatureVerify_returns_error_if_Plaintiff_or_Defendant_contract_creation_fails_because_of_invalid_characters(string plaintiff, string defendant, int code, string error)
        {
            var request = SignatureVerifyRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);

            var result = _service.SignatureVerify(request.Value);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == code);
            Assert.IsTrue(result.Error.Message.Equals(error));
        }

        [TestCase(" ", "VV", 400, "Signatures field cannot be empty")]
        [TestCase("VV", " ", 400, "Signatures field cannot be empty")]
        public void SignatureVerify_returns_error_if_Plaintiff_or_Defendant_contract_creation_fails_because_of_empty_input(string plaintiff, string defendant, int code, string error)
        {
            var request = SignatureVerifyRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);
            Assert.False(request.IsFail);

            var result = _service.SignatureVerify(request.Value);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == code);
            Assert.IsTrue(result.Error.Message.Equals(error));
        }

        [TestCase("N#", "VV", "Plaintiff needs the signature of a Validator to win", 'V')]
        [TestCase("KV#", "NNVV", "Plaintiff needs the signature of a Notary to win", 'N')]
        [TestCase("N#", "VVVV", "Plaintiff needs the signature of a King to win", 'K')]
        [TestCase("VV", "N#", "Defendant needs the signature of a Validator to win", 'V')]
        [TestCase("NNVV", "KV#", "Defendant needs the signature of a Notary to win", 'N')]
        [TestCase("VVVV", "N#", "Defendant needs the signature of a King to win", 'K')]
        public void FillMissingSignature_returns_new_FillMissingSignatureResponse(string plaintiff, string defendant, string recap, char missingSignature)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);

            var result = _service.FillMissingSignature(request.Value);

            Assert.True(result.IsCorrect);
            Assert.False(result.IsFail);
            Assert.IsNull(result.Error);

            Assert.True(result.Value.Recap.Equals(recap));
            Assert.True(result.Value.MissingSignature.Equals(missingSignature));
        }

        [TestCase("K1#", "VV", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        [TestCase("VV#", "K1", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        public void FillMissingSignature_returns_error_if_Plaintiff_or_Defendant_contract_creation_fails_because_of_invalid_characters(string plaintiff, string defendant, int code, string error)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);

            var result = _service.FillMissingSignature(request.Value);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == code);
            Assert.IsTrue(result.Error.Message.Equals(error));
        }

        [TestCase(" ", "#VV", 400, "Signatures field cannot be empty")]
        [TestCase("V#V", " ", 400, "Signatures field cannot be empty")]
        public void FillMissingSignature_returns_error_if_Plaintiff_or_Defendant_contract_creation_fails_because_of_empty_input(string plaintiff, string defendant, int code, string error)
        {
            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);

            Assert.True(request.IsCorrect);
            Assert.False(request.IsFail);

            var result = _service.FillMissingSignature(request.Value);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == code);
            Assert.IsTrue(result.Error.Message.Equals(error));
        }
    }
}
