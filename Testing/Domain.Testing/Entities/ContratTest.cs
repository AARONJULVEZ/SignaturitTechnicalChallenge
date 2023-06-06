using Domain.Common;
using Domain.Entities;
using NUnit.Framework;

namespace Domain.Testing.Entities
{
    [TestFixture]
    public class ContractTest
    {
        [TestCase("")]
        [TestCase(" ")]
        public void Create_Contract_returns_error_if_signature_input_is_empty_or_white_space(string signatures)
        {
            var contract = Contract.Create(signatures);

            Assert.True(contract.IsFail);
            Assert.False(contract.IsCorrect);
            Assert.NotNull(contract.Error);
            Assert.IsTrue(contract.Error.Code == 400);
            Assert.IsTrue(contract.Error.Message == "Signatures field cannot be empty");
        }

        [Test]
        public void Create_Contract_returns_error_if_signature_input_has_more_than_one_hashtag()
        {
            var signatures = "K#VV#";
            var contract = Contract.Create(signatures);

            Assert.True(contract.IsFail);
            Assert.False(contract.IsCorrect);
            Assert.NotNull(contract.Error);
            Assert.IsTrue(contract.Error.Code == 400);
            Assert.IsTrue(contract.Error.Message == "It can be up to just one missing signature");
        }

        [TestCase("KMV")]
        [TestCase("NNB")]
        [TestCase("K1#")]
        public void Create_Contract_returns_error_if_signature_input_has_invalid_characters(string signatures)
        {
            var contract = Contract.Create(signatures);

            Assert.True(contract.IsFail);
            Assert.False(contract.IsCorrect);
            Assert.NotNull(contract.Error);
            Assert.IsTrue(contract.Error.Code == 400);
            Assert.IsTrue(contract.Error.Message == "Signatures field must contain a combination of just the letters K, N or V");
        }

        [TestCase("KNV", 7)]
        [TestCase("NNV", 5)]
        [TestCase("VVV", 3)]
        public void Create_Contract_calculate_signatures_value(string signatures, int value)
        {
            var contract = Contract.Create(signatures);

            Assert.True(contract.IsCorrect);
            Assert.False(contract.IsFail);
            Assert.Null(contract.Error);
            Assert.IsTrue(contract.Value.SignaturesValue == value);
        }

        [TestCase("KnNnV", ComparisonStatus.Win)]
        [TestCase("NnN", ComparisonStatus.Draw)]
        [TestCase("Vv", ComparisonStatus.Lose)]
        public void CompareWithRival_returns_the_winner_in_a_trial(string signatures, ComparisonStatus status)
        {
            var contract = Contract.Create(signatures);
            var rivalSignatures = "NNN";
            var rival = Contract.Create(rivalSignatures);

            Assert.True(contract.IsCorrect);
            Assert.True(rival.IsCorrect);
            var result = contract.Value.CompareWithRival(rival.Value);

            Assert.True(result == status);
        }

        [TestCase("K#NV", RolesSignatureValue.Notary)]
        [TestCase("NN#NN", RolesSignatureValue.Validator)]
        [TestCase("NN#V", RolesSignatureValue.King)]
        public void GetMinimunSignatureNeededToWin_returns_the_necesary_role_for_win(string signatures, RolesSignatureValue role)
        {
            var contract = Contract.Create(signatures);
            var rivalSignatures = "NNNN";
            var rival = Contract.Create(rivalSignatures);

            Assert.True(contract.IsCorrect);
            Assert.True(rival.IsCorrect);
            var result = contract.Value.GetMinimunSignatureNeededToWin(rival.Value);

            Assert.NotNull(result);
            Assert.True(result.Value == role);
        }

        [Test]
        public void GetMinimunSignatureNeededToWin_returns_null_if_needs_more_than_one_signature_to_win()
        {
            var signatures = "K#VV";
            var contract = Contract.Create(signatures);
            var rivalSignatures = "KNNNN";
            var rival = Contract.Create(rivalSignatures);

            Assert.True(contract.IsCorrect);
            Assert.True(rival.IsCorrect);
            var result = contract.Value.GetMinimunSignatureNeededToWin(rival.Value);

            Assert.IsNull(result);
            Assert.True((rival.Value.SignaturesValue - contract.Value.SignaturesValue) > 4);
        }
    }
}
