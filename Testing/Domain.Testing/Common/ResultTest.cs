using Domain.Common;
using NUnit.Framework;

namespace Domain.Test.Common
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void Result_by_default_is_Correct()
        {
            var result = Result.Correct();

            Assert.True(result.IsCorrect);
            Assert.False(result.IsFail);
            Assert.Null(result.Error);
        }

        [Test]
        public void Result_throws_exception_at_creating_Fail_result_without_Error()
        {
            var result = Result.Fail(null);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == 500);
            Assert.IsTrue(result.Error.Message == "Response mapping error, null error value");
        }

        [Test]
        public void Result_without_Error_is_Fail()
        {
            var error = new Error(1, "ONE");
            var result = Result.Fail(error);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == 1);
            Assert.IsTrue(result.Error.Message == "ONE");
        }

        [Test]
        public void Correct_result_returns_value()
        {
            var value = new object();
            var result = Result<object>.Correct(value);

            Assert.True(result.IsCorrect);
        }

        [Test]
        public void Correct_result_with_null_throws_exception()
        {
            var result = Result<object>.Correct(null);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == 500);
            Assert.IsTrue(result.Error.Message == "Response mapping error, null value");
        }

        [Test]
        public void Fail_result_without_Error_throws_exception()
        {
            var result = Result<object>.Fail(new object(), null);

            Assert.True(result.IsFail);
            Assert.False(result.IsCorrect);
            Assert.NotNull(result.Error);
            Assert.IsTrue(result.Error.Code == 500);
            Assert.IsTrue(result.Error.Message == "Response mapping error, null error value");
        }

        [Test]
        public void Fail_result_with_null_and_Error_is_fail()
        {
            var error = new Error(1, "ONE");
            var result = Result<object>.Fail(null, error);

            Assert.True(result.IsFail);
            Assert.Null(result.Value);
        }

        [Test]
        public void Correct_Result_when_assign_value()
        {
            var value = new List<object>();
            Result<IEnumerable<object>> result = value;

            Assert.True(result.IsCorrect);
        }
    }
}
