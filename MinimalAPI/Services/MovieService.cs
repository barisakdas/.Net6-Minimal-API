namespace MinimalAPI.Services
{
    public class MovieService : IMovieService
    {
        public Movie Create(Movie movie)
        {
            movie.ID = MovieRepository.Movies.Count + 1;
            MovieRepository.Movies.Add(movie);
            return movie;
        }

        public Movie GetById(int id)
        {
            Movie movie = MovieRepository.Movies.FirstOrDefault(x => x.ID == id);

            if (movie == null) return null;
            return movie;
        }

        public List<Movie> GetAll()
        {
            List<Movie> movies = MovieRepository.Movies;

            if (movies is null) return new List<Movie>();
            return movies;
        }

        public Movie Update(Movie movie)
        {
            Movie oldMovie = MovieRepository.Movies.FirstOrDefault(x => x.ID == movie.ID);
            if (oldMovie == null) return null;

            oldMovie.Title = movie.Title;
            oldMovie.Description = movie.Description;
            oldMovie.Rating = movie.Rating;

            return movie;
        }

        public bool Delete(int id)
        {
            Movie movie = MovieRepository.Movies.FirstOrDefault(x => x.ID == id);
            if (movie == null) return false;

            MovieRepository.Movies.Remove(movie);
            return true;
        }
    }
}
