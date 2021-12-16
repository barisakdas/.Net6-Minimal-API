namespace MinimalAPI.Services
{
    public interface IUserService
    {
        public User Get(UserLogin userLogin);
    }
}
