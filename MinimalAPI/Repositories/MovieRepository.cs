namespace MinimalAPI.Repositories
{
    public class MovieRepository
    {
        public static List<Movie> Movies = new()
        {
            new() { ID = 1, Title = "Star Wars", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. ", Rating = 8.2 },
            new() { ID = 2, Title = "Avengers: End Game", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. ", Rating = 7.5 },
            new() { ID = 3, Title = "Dune", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. ", Rating = 8.5},
            new() { ID = 4, Title = "God Father", Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. ", Rating = 5.5 },
        };

    }
}
