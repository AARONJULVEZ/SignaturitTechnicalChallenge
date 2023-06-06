using Domain.Common;
using NUnit.Framework;

namespace Domain.Testing.Common
{
    [TestFixture]
    public class ErrorTest
    {
        [Test]
        public void Code_cannot_be_0()
        {
            var code = 0;
            var value = "";
            var error = new Error(code, value);

            Assert.True(error.Code == 500);
            Assert.True(error.Message == "Response mapping error, code value must be superior to 0");
        }

        [Test]
        public void Code_cannot_be_less_than_0()
        {
            var code = -10;
            var value = "";
            var error = new Error(code, value);

            Assert.True(error.Code == 500);
            Assert.True(error.Message == "Response mapping error, code value must be superior to 0");
        }

        [Test]
        public void Message_cannot_be_empty()
        {
            var code = 400;
            var value = "";
            var error = new Error(code, value);

            Assert.True(error.Code == 500);
            Assert.True(error.Message == "Response mapping error, message must have value");
        }

        [Test]
        public void Message_cannot_be_white_espace()
        {
            var code = 400;
            var value = " ";
            var error = new Error(code, value);

            Assert.True(error.Code == 500);
            Assert.True(error.Message == "Response mapping error, message must have value");
        }

        [Test]
        public void Valid_input_creates_Error()
        {
            var code = 400;
            var value = "Bad request";
            var error = new Error(code, value);

            Assert.True(error.Code == code);
            Assert.True(error.Message == value);
        }
    }
}
