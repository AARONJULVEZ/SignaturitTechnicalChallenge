using System.Text.RegularExpressions;
using Domain.Common;

namespace Domain.Entities
{
    public class Contract
    {
        private Contract(string signatureCollection, int signatureValue)
        {
            Signatures = signatureCollection;
            SignaturesValue = signatureValue;
        }

        public string Signatures { get; }
        public int SignaturesValue { get; }

        public static Result<Contract> Create(string signatures)
        {
            if (string.IsNullOrWhiteSpace(signatures))
            {
                Error error = new Error(400, "Signatures field cannot be empty");
                return Result<Contract>.Fail(null, error);
            }

            var missingSignaturesCount = Regex.Matches(signatures, "[#]").Count;
            if (missingSignaturesCount > 1)
            {
                Error error = new Error(400, "It can be up to just one missing signature");
                return Result<Contract>.Fail(null, error);
            }

            if (!CheckStringPattern(missingSignaturesCount == 1 ? signatures.Replace("#", "") : signatures))
            {
                Error error = new Error(400, "Signatures field must contain a combination of just the letters K, N or V");
                return Result<Contract>.Fail(null, error);
            }

            var signaturesValue = ResolveSignatureCollection(missingSignaturesCount == 1 ? signatures.Replace("#", "") : signatures);

            return Result<Contract>.Correct(new Contract(signatures, signaturesValue));
        }

        public ComparisonStatus CompareWithRival(Contract rival)
        {
            if (SignaturesValue > rival.SignaturesValue)
                return ComparisonStatus.Win;

            if (SignaturesValue == rival.SignaturesValue)
                return ComparisonStatus.Draw;

            return ComparisonStatus.Lose;
        }

        public RolesSignatureValue? GetMinimunSignatureNeededToWin(Contract rival)
        {
            var spread = rival.SignaturesValue - SignaturesValue;
            if (spread >= 0 && spread <= 4)
            {
                if (spread == 0)
                {
                    return RolesSignatureValue.Validator;
                }
                else if (spread == 1)
                {
                    return RolesSignatureValue.Notary;
                }
                else
                {
                    var signaturesValue = ResolveSignatureCollection(Signatures.Replace("#", "K"));
                    if (rival.SignaturesValue - signaturesValue < 0)
                    {
                        return RolesSignatureValue.King;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        private static bool CheckStringPattern(string signatures)
        {
            if (!Regex.IsMatch(signatures, "^[KNVknv]+$"))
                return false;

            return true;
        }

        private static int ResolveSignatureCollection(string signatures)
        {
            var amountOfKingsSignatures = Regex.Matches(signatures, "[Kk]").Count;
            var valueOfKingSignature = 5;
            var kingSignatureResult = amountOfKingsSignatures * valueOfKingSignature;

            var amountOfNotarySignatures = Regex.Matches(signatures, "[Nn]").Count;
            var valueOfNotarySignature = 2;
            var notarySignatureResult = amountOfNotarySignatures * valueOfNotarySignature;

            var amountOfValidators = Regex.Matches(signatures, "[Vv]").Count;
            var valueOfValidatorSignature = amountOfKingsSignatures == 0 ? 1 : 0;
            var validatorSignatureResult = amountOfValidators * valueOfValidatorSignature;

            return kingSignatureResult + notarySignatureResult + validatorSignatureResult;
        }
    }
}