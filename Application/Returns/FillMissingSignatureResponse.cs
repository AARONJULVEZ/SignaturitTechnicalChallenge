using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Entities;

namespace Application.Returns
{
    public class FillMissingSignatureResponse
    {
        public Contract Plaintiff { get; }
        public Contract Defendant { get; }
        public string Recap { get; }
        public char MissingSignature { get; }

        private FillMissingSignatureResponse(Contract plaintiff, Contract defendant, string recap, char missingSignature)
        {
            Plaintiff = plaintiff;
            Defendant = defendant;
            Recap = recap;
            MissingSignature = missingSignature;
        }

        public static Result<FillMissingSignatureResponse> Create(Contract plaintiff, Contract defendant)
        {
            var plaintiffHasMissingSignature = Regex.IsMatch(plaintiff.Signatures, "[#]");

            var comparation = plaintiff.CompareWithRival(defendant);

            if (plaintiffHasMissingSignature && comparation == ComparisonStatus.Win)
            {
                Error error = new Error(200, $"The plaintiff wins without any extra signature needed");
                return Result<FillMissingSignatureResponse>.Fail(null, error);
            }

            if (!plaintiffHasMissingSignature && comparation == ComparisonStatus.Lose)
            {
                Error error = new Error(200, $"The defendant wins without any extra signature needed");
                return Result<FillMissingSignatureResponse>.Fail(null, error);
            }

            if (plaintiffHasMissingSignature && (comparation == ComparisonStatus.Lose || comparation == ComparisonStatus.Draw))
            {
                var minimunSignature = plaintiff.GetMinimunSignatureNeededToWin(defendant);
                if (minimunSignature == null)
                {
                    Error error = new Error(200, $"The plaintiff needs more than one signature to win");
                    return Result<FillMissingSignatureResponse>.Fail(null, error);
                }

                return Result<FillMissingSignatureResponse>.Correct(
                    new FillMissingSignatureResponse(plaintiff
                                                   , defendant
                                                   , FillRecapWithMissingRole(minimunSignature.Value, true)
                                                   , (char)minimunSignature.Value
                                                   ));
            }

            if (!plaintiffHasMissingSignature && (comparation == ComparisonStatus.Win || comparation == ComparisonStatus.Draw))
            {
                var minimunSignature = defendant.GetMinimunSignatureNeededToWin(plaintiff);
                if (minimunSignature == null)
                {
                    Error error = new Error(200, $"The defendant needs more than one signature to win");
                    return Result<FillMissingSignatureResponse>.Fail(null, error);
                }

                return Result<FillMissingSignatureResponse>.Correct(
                    new FillMissingSignatureResponse(plaintiff
                                                   , defendant
                                                   , FillRecapWithMissingRole(minimunSignature.Value, false)
                                                   , (char)minimunSignature.Value
                                                   ));
            }

            Error processingError = new Error(500, $"Error processing ComparisonStatus");
            return Result<FillMissingSignatureResponse>.Fail(null, processingError);
        }

        private static string FillRecapWithMissingRole(RolesSignatureValue minimunSignature, bool mayPlaintiffWin)
        {
            string party = mayPlaintiffWin ? "Plaintiff" : "Defendant";

            return $"{party} needs the signature of a {minimunSignature} to win";
        }
    }
}
