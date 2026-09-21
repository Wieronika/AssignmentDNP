using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public SinglePostView(
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public async Task ShowAsync()
    {
        Console.Write("\nEnter post ID: ");
        string? input = Console.ReadLine();

        if (!int.TryParse(input, out int postId))
        {
            Console.WriteLine("Post ID must be a number.");
            return;
        }
        
        bool postExists = postRepository.GetMany().Any(post => post.Id == postId);

        if (!postExists)
        {
            Console.WriteLine("Post with this ID does not exist.");
            return;
        }
        

        Post post = await postRepository.GetSingleAsync(postId);

        Console.WriteLine("\n*** Post ***");
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Content: {post.Body}");
        Console.WriteLine($"User ID: {post.UserId}");

        List<Comment> comments = commentRepository
            .GetMany()
            .Where(comment => comment.PostId == postId)
            .ToList();

        Console.WriteLine("\nComments:");

        if (comments.Count == 0)
        {
            Console.WriteLine("No comments yet.");
        }
        else
        {
            foreach (Comment comment in comments)
            {
                Console.WriteLine($"- {comment.Body} (User ID: {comment.UserId})");
            }
        }

        Console.WriteLine("\n1) Add comment");
        Console.WriteLine("<) Back");

        string? choice = Console.ReadLine();

        if (choice == "1")
        {
            await AddCommentAsync(postId);
        }
    }

    private async Task AddCommentAsync(int postId)
    {
        Console.Write("Enter your user ID: ");
        string? userInput = Console.ReadLine();

        if (!int.TryParse(userInput, out int userId))
        {
            Console.WriteLine("User ID must be a number.");
            return;
        }

        bool userExists = userRepository.GetMany().Any(user => user.Id == userId);

        if (!userExists)
        {
            Console.WriteLine("User with this ID does not exist.");
            return;
        }

        Console.Write("Enter your comment: ");
        string? body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment cannot be empty.");
            return;
        }

        Comment comment = new Comment(body, userId, postId);

        await commentRepository.AddAsync(comment);

        Console.WriteLine("Comment added successfully!");
    }
}