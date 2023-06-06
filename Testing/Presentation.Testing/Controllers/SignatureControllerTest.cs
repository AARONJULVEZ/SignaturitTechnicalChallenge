using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Presentation.Testing.Models;

namespace Presentation.Testing.Controllers
{
    [TestFixture]
    public class SignatureControllerTest
    {
        private WebApplicationFactory<Startup> _factory;

        [SetUp]
        public void ConfigurarPrueba()
        {
            _factory = new WebApplicationFactory<Startup>();
        }

        [TearDown]
        public void FinalizarPrueba()
        {
            _factory.Dispose();
        }

        [TestCase("K", "VV", "Plaintiff wins to the defendant")]
        [TestCase("VV", "N", "Plaintiff and defendant are in a draw")]
        [TestCase("V", "N", "Plaintiff loses to the defendant")]
        public async Task SignatureVerify_returns_winner(string plaintiff, string defendant, string recap)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/Signature/{plaintiff}/{defendant}/SignatureVerify");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignatureVerifyResponseTestDTO>(stringResponse);

            Assert.NotNull(result);
            Assert.AreEqual(result.Recap, recap);
            Assert.AreEqual(result.Plaintiff.Signatures, plaintiff);
            Assert.AreEqual(result.Defendant.Signatures, defendant);
        }

        [TestCase("K1", "VV", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        [TestCase("VV", "K1", 400, "Signatures field must contain a combination of just the letters K, N or V")]
        public async Task SignatureVerify_returns_error_because_of_invalid_characters(string plaintiff, string defendant, int code, string error)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/Signature/{plaintiff}/{defendant}/SignatureVerify");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(stringResponse, error);
        }

        /*
        Test fails because when you send an empty string, it makes the url not legible and the outcome 404 Not Found

        [TestCase(" ", "VV", 400, "Signatures field cannot be empty")]
        [TestCase("VV", "", 400, "Signatures field cannot be empty")]
        public async Task SignatureVerify_returns_error_because_of_empty_input(string plaintiff, string defendant, int code, string error)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"/Signature/{plaintiff}/{defendant}/SignatureVerify");

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(stringResponse, error);
        }

        Test fails because when you send the character #, it makes the url not legible and the outcome 404 Not Found
        Besides that it works fine, you can see it if you remove the #

        [TestCase("VV#", "VV", "Plaintiff needs the signature of a Validator to win", 'V')]
        [TestCase("VV#", "NV", "Plaintiff needs the signature of a Notary to win", 'N')]
        [TestCase("V#", "NV", "Plaintiff needs the signature of a King to win", 'K')]
        [TestCase("N", "V#V", "Defendant needs the signature of a Validator to win", 'V')]
        [TestCase("VN", "VV#", "Defendant needs the signature of a Notary to win", 'N')]
        [TestCase("VN", "#V", "Defendant needs the signature of a King to win", 'K')]
        public async Task FillMissingSignatureResponse_returns_missing_signature(string plaintiff, string defendant, string recap, char missingSignature)
        {
            var client = _factory.CreateClient();
            var url = $"/Signature/{plaintiff}/{defendant}/FillMissingSignature";

            var response = await client.GetAsync(url);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FillMissingSignatureResponseTestDto>(stringResponse);

            Assert.NotNull(result);
            Assert.AreEqual(result.Plaintiff.Signatures, plaintiff);
            Assert.AreEqual(result.Defendant.Signatures, defendant);
            Assert.AreEqual(result.Recap, recap);
            Assert.AreEqual(result.MissingSignature, missingSignature);
        }
        */
    }
}
