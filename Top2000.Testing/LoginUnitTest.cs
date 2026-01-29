using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TemplateJwtProject.Models.DTOs;
using Xunit;

namespace TemplateJwtProject.Tests
{
    public class LoginUnitTest
    {
        private static IList<ValidationResult> Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);
            Validator.TryValidateObject(model, context, results, true);
            return results;
        }

        [Fact]
        public void LoginDto_Should_Be_Valid_When_Data_Is_Correct()
        {
            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = "123456"
            };

            var results = Validate(dto);

            Assert.Empty(results);
        }

        [Fact]
        public void LoginDto_Should_Fail_When_Email_Is_Invalid()
        {
            var dto = new LoginDto
            {
                Email = "wrong-email",
                Password = "123456"
            };

            var results = Validate(dto);

            Assert.NotEmpty(results);
        }

        [Fact]
        public void LoginDto_Should_Fail_When_Password_Is_Empty()
        {
            var dto = new LoginDto
            {
                Email = "test@test.com",
                Password = ""
            };

            var results = Validate(dto);

            Assert.NotEmpty(results);
        }
    }
}
