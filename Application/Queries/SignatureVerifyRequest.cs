using System.Text.RegularExpressions;
using Domain.Common;

namespace Application.Queries
{
    public class SignatureVerifyRequest
    {
        public string Plaintiff { get; }
        public string Defendant { get; }

        private SignatureVerifyRequest(string plaintiff, string defendant)
        {
            Plaintiff = plaintiff;
            Defendant = defendant;
        }

        public static Result<SignatureVerifyRequest> Create(string plaintiff, string defendant)
        {
            // We make this filtering in order to build one single Contract entity,
            // there's also the posibility of creating a Contract that works with the #
            // character and another contract entity that doesn't

            if (Regex.IsMatch(plaintiff, "[#]"))
            {
                Error error = new Error(400, "Plaintiff cannot have any missing signature");
                return Result<SignatureVerifyRequest>.Fail(null, error);
            }

            if (Regex.IsMatch(defendant, "[#]"))
            {
                Error error = new Error(400, "Defendant cannot have any missing signature");
                return Result<SignatureVerifyRequest>.Fail(null, error);
            }

            return Result<SignatureVerifyRequest>.Correct(new SignatureVerifyRequest(plaintiff, defendant));
        }
    }
}
