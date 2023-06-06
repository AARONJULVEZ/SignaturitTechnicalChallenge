using Application.Queries;
using Application.Returns;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignatureController : ApiBaseController
    {
        private readonly ILogger<SignatureController> _logger;
        private readonly ContractsService _service;

        public SignatureController(ILogger<SignatureController> logger, ContractsService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet("{plaintiff}/{defendant}/SignatureVerify")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<SignatureVerifyResponse> SignatureVerify(string plaintiff, string defendant)
        {
            _logger.LogInformation($"SignatureController/SignatureVerify Start -> Plaintiff: {plaintiff}, Defendant: {defendant}", DateTimeOffset.UtcNow);

            var request = SignatureVerifyRequest.Create(plaintiff, defendant);
            if (request.IsFail)
            {
                return ResolveResult(request);
            }

            var result = _service.SignatureVerify(request.Value);
            if (result.IsFail)
            {
                return ResolveResult(result);
            }

            _logger.LogInformation($"SignatureController/SignatureVerify finish -> Result: {result.Value}", DateTimeOffset.UtcNow);

            return ResolveResult(result);
        }

        [HttpGet("{plaintiff}/{defendant}/FillMissingSignature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FillMissingSignatureResponse>> FillMissingSignature(string plaintiff, string defendant)
        {
            _logger.LogInformation($"SignatureController/FillMissingSignature Start -> Plaintiff: {plaintiff}, Defendant: {defendant}", DateTimeOffset.UtcNow);

            var request = FillMissingSignatureRequest.Create(plaintiff, defendant);
            if (request.IsFail)
            {
                return ResolveResult(request);
            }

            var result = _service.FillMissingSignature(request.Value);
            if (result.IsFail)
            {
                return ResolveResult(result);
            }

            _logger.LogInformation($"SignatureController/FillMissingSignature finish -> Result: {result.Value}", DateTimeOffset.UtcNow);

            return ResolveResult(result);
        }
    }
}