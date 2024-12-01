using QuizApp.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Test;

public class BaseRepoTest : IDisposable
{
    internal SqliteConnection Connection { get; }
    internal DbContextOptions<QuizContext> Options { get; }

    public BaseRepoTest()
    {
        // Create and open a shared SqliteConnection
        Connection = new SqliteConnection("DataSource=:memory:");
        Connection.Open();

        // Configure DbContextOptions to use this connection
        Options = new DbContextOptionsBuilder<QuizContext>()
            .UseSqlite(Connection)
            .Options;

        // Ensure the database schema is created
        using var context = new QuizContext(Options);
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        // Clean up the connection after all tests are done
        Connection.Close();
    }

    #region IDisposable Support
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Connection.Close();
                Connection.Dispose();
            }

            disposedValue = true;
        }
    }

    #endregion
}