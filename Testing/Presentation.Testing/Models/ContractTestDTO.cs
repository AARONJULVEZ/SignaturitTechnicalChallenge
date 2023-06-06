using Newtonsoft.Json;

namespace Presentation.Testing.Models
{
    public class ContractTestDTO
    {
        public string Signatures { get; set; }
        public int SignaturesValue { get; set; }

        [JsonConstructor]
        public ContractTestDTO(string signatureCollection, int signatureValue)
        {
            Signatures = signatureCollection;
            SignaturesValue = signatureValue;
        }
    }
}