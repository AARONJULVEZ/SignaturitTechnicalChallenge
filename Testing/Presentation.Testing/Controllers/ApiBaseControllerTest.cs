using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Presentation.Controllers;

namespace Presentation.Testing.Controllers
{
    public class ApiBaseControllerTest : ApiBaseController
    {
        [Test]
        public void ApiBaseControllerTest_returns_ok_200_based_on_result_input()
        {
            var ok200Return = "Correct";
            var returnOk200 = Result<object>.Correct(ok200Return);

            var result = ResolveResult<object>(returnOk200.Value);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;

            Assert.True(okResult.StatusCode == 200);
            Assert.True(okResult.Value?.Equals("Correct"));
        }

        [Test]
        public void ApiBaseControllerTest_returns_error_200_based_on_result_input()
        {
            var error200Return = "Fail code 200";
            var errorMessage = "Code runs normally but a business rule for the case has been broken";
            var error = new Error(200, errorMessage);
            var returnError200 = Result<object>.Fail(error200Return, error);

            var result = ResolveResult<Result<object>>(returnError200);

            Assert.IsInstanceOf<ObjectResult>(result);
            var result200 = (ObjectResult)result;

            Assert.True(result200.StatusCode == 200);
            Assert.NotNull(result200.Value);

            Assert.IsInstanceOf<Result<object>>(result200.Value);
            var errorResult = (Result<object>)result200.Value;

            Assert.AreEqual(errorResult.Value, error200Return);

            Assert.NotNull(errorResult.Error);
            Assert.AreEqual(errorResult.Error.Code, 200);
            Assert.AreEqual(errorResult.Error.Message, errorMessage);
        }

        [Test]
        public void ApiBaseControllerTest_returns_error_400_BadRequest_based_on_result_input()
        {
            var error400Return = "Fail code 400";
            var errorCode = 400;
            var errorMessage = "BadRequest";
            var error = new Error(errorCode, errorMessage);
            var returnBadRequest400 = Result<object>.Fail(error400Return, error);

            var result = ResolveResult<object>(returnBadRequest400);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;

            Assert.True(badRequestResult.StatusCode == errorCode);
            Assert.AreEqual(badRequestResult.Value, errorMessage);
        }

        [Test]
        public void ApiBaseControllerTest_returns_error_500_based_on_result_input()
        {
            var error500Return = "Fail code 500";
            var errorCode = 500;
            var errorMessage = "500 Internal Server Error";
            var error = new Error(errorCode, errorMessage);
            var return500 = Result<object>.Fail(error500Return, error);

            var result = ResolveResult<object>(return500);

            Assert.IsInstanceOf<ObjectResult>(result);
            var result500 = (ObjectResult)result;

            Assert.True(result500.StatusCode == errorCode);
            Assert.NotNull(result500.Value);

            Assert.IsInstanceOf<Error>(result500.Value);
            var errorResult = (Error)result500.Value;

            Assert.NotNull(errorResult);
            Assert.AreEqual(errorResult.Code, errorCode);
            Assert.AreEqual(errorResult.Message, errorMessage);
        }
    }
}
