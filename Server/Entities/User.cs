namespace Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    //one user can write multiple posts
    public List<Post> Posts { get; set; } = new();
    //one user can write multiple comments.
    public List<Comment> Comments { get; set; } = new();

    private User()
    {
    }

    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}