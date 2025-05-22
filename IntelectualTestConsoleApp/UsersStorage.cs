public class UsersStorage
{
    public List<User> Users { get; }

    public UsersStorage()
    {
        Users = FileSystem.Load<User>(FileNames.Users);
    }
}