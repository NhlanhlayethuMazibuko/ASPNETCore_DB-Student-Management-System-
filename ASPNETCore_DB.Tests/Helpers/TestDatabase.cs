using ASPNETCore_DB.Data;
using ASPNETCore_DB.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCore_DB.Tests.Helpers;

/// <summary>
/// Creates a fresh SQLite database that lives only in memory, so every test
/// starts with a clean database and never touches the real Students.db file.
/// </summary>
public sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;

    public SQLiteDBContext Context { get; }

    public TestDatabase()
    {
        // An in-memory SQLite database exists only while this connection is open.
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<SQLiteDBContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new SQLiteDBContext(options);
        Context.Database.EnsureCreated();
    }

    public static Student MakeStudent(
        string studentNumber = "2220447620",
        string firstName = "Thabo",
        string surname = "Nkosi",
        string email = "thabo@example.com",
        DateTime? enrollmentDate = null) => new()
    {
        StudentNumber = studentNumber,
        FirstName = firstName,
        Surname = surname,
        Email = email,
        EnrollmentDate = enrollmentDate ?? new DateTime(2024, 1, 15),
        Photo = "default.PNG",
    };

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
