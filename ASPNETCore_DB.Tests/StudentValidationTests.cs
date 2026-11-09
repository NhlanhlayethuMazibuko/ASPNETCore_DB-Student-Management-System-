using System.ComponentModel.DataAnnotations;
using ASPNETCore_DB.Models;
using ASPNETCore_DB.Tests.Helpers;

namespace ASPNETCore_DB.Tests;

/// <summary>
/// Checks the validation rules on the Student model (the [Required],
/// [StringLength] and [RegularExpression] attributes).
/// </summary>
public class StudentValidationTests
{
    private static List<ValidationResult> Validate(Student student)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(student, new ValidationContext(student), results, validateAllProperties: true);
        return results;
    }

    private static bool HasErrorFor(List<ValidationResult> results, string property) =>
        results.Any(r => r.MemberNames.Contains(property));

    [Fact]
    public void ValidStudent_PassesValidation()
    {
        Assert.Empty(Validate(TestDatabase.MakeStudent()));
    }

    [Theory]
    [InlineData("123456789")]   // 9 digits: too short
    [InlineData("12345678901")] // 11 digits: too long
    [InlineData("12345ABCDE")]  // letters are not allowed
    [InlineData("")]            // required
    public void InvalidStudentNumber_FailsValidation(string studentNumber)
    {
        var student = TestDatabase.MakeStudent(studentNumber: studentNumber);

        Assert.True(HasErrorFor(Validate(student), nameof(Student.StudentNumber)));
    }

    [Theory]
    [InlineData("T")]        // shorter than 2 characters
    [InlineData("Thabo1")]   // digits are not allowed
    [InlineData("")]         // required
    public void InvalidFirstName_FailsValidation(string firstName)
    {
        var student = TestDatabase.MakeStudent(firstName: firstName);

        Assert.True(HasErrorFor(Validate(student), nameof(Student.FirstName)));
    }

    [Fact]
    public void SurnameLongerThan50Characters_FailsValidation()
    {
        var student = TestDatabase.MakeStudent(surname: new string('a', 51));

        Assert.True(HasErrorFor(Validate(student), nameof(Student.Surname)));
    }

    [Fact]
    public void MissingEmail_FailsValidation()
    {
        var student = TestDatabase.MakeStudent();
        student.Email = null;

        Assert.True(HasErrorFor(Validate(student), nameof(Student.Email)));
    }
}
