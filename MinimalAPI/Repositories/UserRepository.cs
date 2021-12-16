namespace MinimalAPI.Repositories;

public class UserRepository
{
    public static List<User> Users = new()
    {
        new() { ID = 1, FirstName = "Sedat", LastName = "Güney", UserName = "sedatguney", Email = "sedatguney@yahoo.com", Password = "pass123", Role = "Standard" },
        new() { ID = 2, FirstName = "Barış", LastName = "Kuzey", UserName = "bariskuzey", Email = "bariskuzey@yahoo.com", Password = "123pass", Role = "Administrator" },
    };
}
