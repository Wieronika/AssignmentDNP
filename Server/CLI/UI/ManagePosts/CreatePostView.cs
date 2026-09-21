using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public CreatePostView(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.WriteLine("\n*** Create New Post ***");

        Console.Write("Enter post title: ");
        string? title = Console.ReadLine();

        Console.Write("Enter post content: ");
        string? content = Console.ReadLine();

        Console.Write("Enter user ID: ");
        string? userIdInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) ||
            string.IsNullOrWhiteSpace(content))
        {
            Console.WriteLine("Title and content cannot be empty.");
            return;
        }

        if (!int.TryParse(userIdInput, out int userId))
        {
            Console.WriteLine("User ID must be a number.");
            return;
        }

        bool userExists = userRepository
            .GetMany()
            .Any(user => user.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User with this ID does not exist.");
            return;
        }

        Post post = new Post(title, content, userId);

        Post createdPost = await postRepository.AddAsync(post);

        Console.WriteLine($"Post created successfully! ID: {createdPost.Id}");
    }
}