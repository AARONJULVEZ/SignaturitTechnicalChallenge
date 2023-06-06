using System.Text.RegularExpressions;
using Domain.Common;

namespace Application.Queries
{
    public class FillMissingSignatureRequest
    {
        public string Plaintiff { get; }
        public string Defendant { get; }

        private FillMissingSignatureRequest(string plaintiff, string defendant)
        {
            Plaintiff = plaintiff;
            Defendant = defendant;
        }

        public static Result<FillMissingSignatureRequest> Create(string plaintiff, string defendant)
        {
            var plaintiffHasMissingSignature = Regex.IsMatch(plaintiff, "[#]");
            var defendantHasMissingSignature = Regex.IsMatch(defendant, "[#]");
            if (plaintiffHasMissingSignature && defendantHasMissingSignature)
            {
                Error error = new Error(400, $"It can't be missing signatures from both parties");
                return Result<FillMissingSignatureRequest>.Fail(null, error);
            }
            else if (!plaintiffHasMissingSignature && !defendantHasMissingSignature)
            {
                Error error = new Error(400, $"A missing signature represented by # is needed to proceed");
                return Result<FillMissingSignatureRequest>.Fail(null, error);
            }

            return Result<FillMissingSignatureRequest>.Correct(new FillMissingSignatureRequest(plaintiff, defendant));
        }
    }
}
