using Domain.Common;
using Domain.Entities;

namespace Application.Returns
{
    public class SignatureVerifyResponse
    {
        public Contract Plaintiff { get; }
        public Contract Defendant { get; }
        public string Recap { get; }

        public SignatureVerifyResponse(Contract plaintiff, Contract defendant)
        {
            Plaintiff = plaintiff;
            Defendant = defendant;
            Recap = FillRecap();
        }

        private string FillRecap()
        {
            var comparisonResult = Plaintiff.CompareWithRival(Defendant);
            switch (comparisonResult)
            {
                case ComparisonStatus.Win:
                    return "Plaintiff wins to the defendant";

                case ComparisonStatus.Draw:
                    return "Plaintiff and defendant are in a draw";

                case ComparisonStatus.Lose:
                    return "Plaintiff loses to the defendant";
            }

            return "";
        }
    }
}
