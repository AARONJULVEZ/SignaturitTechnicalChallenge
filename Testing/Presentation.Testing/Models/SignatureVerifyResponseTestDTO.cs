using Newtonsoft.Json;

namespace Presentation.Testing.Models
{
    public class SignatureVerifyResponseTestDTO
    {
        public ContractTestDTO Plaintiff { get; set; }
        public ContractTestDTO Defendant { get; set; }
        public string Recap { get; set; }

        [JsonConstructor]
        public SignatureVerifyResponseTestDTO(ContractTestDTO plaintiff, ContractTestDTO defendant, string recap)
        {
            Plaintiff = plaintiff;
            Defendant = defendant;
            Recap = recap;
        }
    }
}
