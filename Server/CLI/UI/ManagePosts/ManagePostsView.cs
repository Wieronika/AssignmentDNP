using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ManagePostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public ManagePostsView(
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
        while (true)
        {
            Console.WriteLine("\n*** Manage Posts ***");
            Console.WriteLine("1) Create new post");
            Console.WriteLine("2) View posts");
            Console.WriteLine("3) View specific post");
            Console.WriteLine("<) Back");
            
            string? input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    CreatePostView createPostView =
                        new CreatePostView(postRepository, userRepository);

                    await createPostView.ShowAsync();
                    break;
                
                case "2":
                    ListPostsView listPostsView = new ListPostsView(postRepository);
                    listPostsView.Show();
                    break;
                
                case "3":
                    SinglePostView singlePostView =
                        new SinglePostView(
                            postRepository,
                            commentRepository,
                            userRepository
                        );

                    await singlePostView.ShowAsync();
                    break;
                
                case "<":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}