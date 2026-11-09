using ASPNETCore_DB.Repositories;
using ASPNETCore_DB.Tests.Helpers;

namespace ASPNETCore_DB.Tests;

public class StudentRepoTests : IDisposable
{
    private readonly TestDatabase _db = new();
    private readonly StudentRepo _repo;

    public StudentRepoTests()
    {
        _repo = new StudentRepo(_db.Context);
    }

    public void Dispose() => _db.Dispose();

    [Fact]
    public void Create_SavesStudent_SoItCanBeFoundByStudentNumber()
    {
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111"));

        var found = _repo.Details("1111111111");

        Assert.NotNull(found);
        Assert.Equal("Thabo", found.FirstName);
    }

    [Fact]
    public void Details_ReturnsNull_WhenStudentDoesNotExist()
    {
        Assert.Null(_repo.Details("9999999999"));
    }

    [Fact]
    public void ByEmail_FindsTheMatchingStudent()
    {
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111", email: "a@example.com"));
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "2222222222", email: "b@example.com"));

        var found = _repo.ByEmail("b@example.com");

        Assert.NotNull(found);
        Assert.Equal("2222222222", found.StudentNumber);
    }

    [Fact]
    public void Edit_UpdatesTheSavedStudent()
    {
        var student = _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111"));

        student.Surname = "Dlamini";
        _repo.Edit(student);

        Assert.Equal("Dlamini", _repo.Details("1111111111")!.Surname);
    }

    [Fact]
    public void IsExist_ReturnsTrue_WhenStudentExists()
    {
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111"));

        Assert.True(_repo.IsExist("1111111111"));
    }

    [Fact]
    public void IsExist_ReturnsFalse_WhenStudentDoesNotExist()
    {
        // This test caught a bug: IsExist used to return true for missing students.
        Assert.False(_repo.IsExist("9999999999"));
    }

    [Fact]
    public void Delete_RemovesStudent_AndReturnsTrue()
    {
        var student = _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111"));

        var deleted = _repo.Delete(student);

        Assert.True(deleted);
        Assert.Null(_repo.Details("1111111111"));
    }

    [Fact]
    public void GetStudents_WithSearch_ReturnsOnlyMatchingStudentNumbers()
    {
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111", email: "a@example.com"));
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "2222222222", email: "b@example.com"));
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1112223333", email: "c@example.com"));

        var results = _repo.GetStudents("111", sortOrder: null!).ToList();

        Assert.Equal(2, results.Count);
        Assert.All(results, s => Assert.Contains("111", s.StudentNumber));
    }

    [Fact]
    public void GetStudents_WithoutSortOrder_SortsBySurnameAToZ()
    {
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "1111111111", surname: "Zulu", email: "a@example.com"));
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "2222222222", surname: "Botha", email: "b@example.com"));
        _repo.Create(TestDatabase.MakeStudent(studentNumber: "3333333333", surname: "Mokoena", email: "c@example.com"));

        var surnames = _repo.GetStudents(null!, null!).Select(s => s.Surname).ToList();

        Assert.Equal(new[] { "Botha", "Mokoena", "Zulu" }, surnames);
    }

    [Theory]
    [InlineData("name_desc", new[] { "3333333333", "1111111111", "2222222222" })]
    [InlineData("number_desc", new[] { "3333333333", "2222222222", "1111111111" })]
    [InlineData("Date", new[] { "2222222222", "1111111111", "3333333333" })]
    [InlineData("date_desc", new[] { "3333333333", "1111111111", "2222222222" })]
    public void GetStudents_SortsByTheRequestedOrder(string sortOrder, string[] expectedNumbers)
    {
        _repo.Create(TestDatabase.MakeStudent("1111111111", surname: "Mokoena", email: "a@example.com",
            enrollmentDate: new DateTime(2023, 6, 1)));
        _repo.Create(TestDatabase.MakeStudent("2222222222", surname: "Botha", email: "b@example.com",
            enrollmentDate: new DateTime(2022, 2, 1)));
        _repo.Create(TestDatabase.MakeStudent("3333333333", surname: "Zulu", email: "c@example.com",
            enrollmentDate: new DateTime(2025, 1, 10)));

        var numbers = _repo.GetStudents(null!, sortOrder).Select(s => s.StudentNumber).ToArray();

        Assert.Equal(expectedNumbers, numbers);
    }
}
