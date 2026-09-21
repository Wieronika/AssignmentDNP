using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void Show()
    {
        IEnumerable<Post> posts = postRepository.GetMany(); //with this we are getting all the posts we currently have

        Console.WriteLine("\n*** Posts ***");

        foreach (Post post in posts)
        {
            Console.WriteLine($"[{post.Title}, {post.Id}]");
        }
    }
}