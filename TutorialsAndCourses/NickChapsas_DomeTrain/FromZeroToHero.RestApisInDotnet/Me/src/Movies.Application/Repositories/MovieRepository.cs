using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         insert into movies (id, slug, title, yearofrelease)
                                                                         values (@Id, @Slug, @Title, @YearOfRelease);
                                                                         """, movie, transaction: transaction));
        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                result += await connection.ExecuteAsync(new CommandDefinition("""
                                                                              insert into genres (movieId, name)
                                                                              values (@MovieId, @Name);
                                                                              """,
                    new { MovieId = movie.Id, Name = genre }, transaction: transaction));
            }
        }

        transaction.Commit();
        return result == (1 + movie.Genres.Count);
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  select *
                                  from movies
                                  where id = @Id;
                                  """, new { Id = id }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name
                                  from genres
                                  where movieId = @movieId;
                                  """, new { movieId = id }));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  select *
                                  from movies
                                  where slug = @Slug;
                                  """, new { Slug = slug }));

        if (movie is null)
        {
            return null;
        }

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                  select name
                                  from genres
                                  where movieId = @movieId;
                                  """, new { movieId = movie.Id }));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        var result = await connection.QueryAsync(
            new CommandDefinition("""
                                  select m.*, string_agg(g.name, ',') as genres
                                  from movies m left join genres g on m.id = g.movieid
                                  group by id
                                  """));

        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Movie movie)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            new CommandDefinition("""
                                  delete from genres
                                  where movieId = @id;
                                  """, new { id = movie.Id }, transaction: transaction));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(
                new CommandDefinition("""
                                      insert into genres (movieId, name)
                                      values (@MovieId, @Name);
                                      """, new { MovieId = movie.Id, Name = genre }, transaction: transaction));
        }
        
        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  update movies
                                  set title = @Title,
                                      slug = @Slug,
                                      yearofrelease = @YearOfRelease
                                  where id = @Id;
                                  """, movie, transaction: transaction));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(
            new CommandDefinition("""
                                  delete from genres
                                  where movieId = @id;
                                  """, new { id }, transaction: transaction));
        
        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  delete from movies where id = @id;
                                  """, new { id }, transaction: transaction));
        
        transaction.Commit();
        return result > 0;

    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition("""
                                  select count(1) from movies where id = @id
                                  """, new { id }));
    }
}