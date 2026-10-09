using System.Text.Json.Serialization;

namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }

    public int UserId { get; }
    public int PostId { get; }

    [JsonConstructor]
    public Comment(string body, int userId, int postId)
    {
        Body = body;
        UserId = userId;
        PostId = postId;
    }
}