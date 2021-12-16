namespace MinimalAPI.Services
{
    public interface IMovieService
    {
        public Movie Create(Movie movie);
        public Movie GetById(int id);
        public List<Movie> GetAll();
        public Movie Update(Movie movie);
        public bool Delete(int id);

    }
}
