using System.Text.Json.Serialization;

namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }

    public User User { get; set; } = null!;
    public Post Post { get; set; } = null!;

    private Comment()
    {
    }

    [JsonConstructor]
    public Comment(string body, int userId, int postId)
    {
        Body = body;
        UserId = userId;
        PostId = postId;
    }
}