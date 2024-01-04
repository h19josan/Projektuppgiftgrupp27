using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Projektuppgiftgrupp27
{ }


internal class Program
{
    static string filePath = "blogPosts.json";

    static List<BlogPost> blogPosts = new List<BlogPost>();

    static void Main()
    {
        LoadPostsFromFile();
        while (true)
        {
            Console.WriteLine("1. Skapa nytt inlägg");
            Console.WriteLine("2. Visa alla inlägg");
            Console.WriteLine("3. Sök inlägg");
            Console.WriteLine("4. Ta bort inlägg");
            Console.WriteLine("5. Avsluta");

            Console.Write("Välj ett alternativ: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreatePost();
                    break;
                case "2":
                    DisplayAllPosts();
                    break;
                case "3":
                    SearchPosts();
                    break;
                case "4":
                    DeletePost();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }
        }
    }
    static void LoadPostsFromFile()
    {
        if (File.Exists(filePath)) 
        {
        string jsonData = File.ReadAllText(filePath);
            blogPosts = JsonConvert.DeserializeObject<List<BlogPost>>(jsonData);
        }
    }

    static void SavePostsToFile()
    {
        string jsonData = JsonConvert.SerializeObject(blogPosts);
        File.WriteAllText(filePath, jsonData);
    }
    static void CreatePost()
    {
        Console.Write("Ange titel: ");
        string title = Console.ReadLine();

        Console.Write("Ange datum (YYYY-MM-DD): ");
        string dateStr = Console.ReadLine();
        DateTime date;
        while (!DateTime.TryParse(dateStr, out date))
        {
            Console.WriteLine("Ogiltigt datumformat. Försök igen (YYYY-MM-DD): ");
            dateStr = Console.ReadLine();
        }

        Console.Write("Ange innehåll: ");
        string content = Console.ReadLine();

        BlogPost post = new BlogPost { Title = title, Date = date, Content = content };
        blogPosts.Add(post);

        Console.WriteLine("Inlägget har lagts till i dagboken.");
        SavePostsToFile();
    }


    static void DisplayAllPosts()
    {
        if (blogPosts.Count == 0)
        {
            Console.WriteLine("Inga inlägg i dagboken ännu.");
            return;
        }

        blogPosts = blogPosts.OrderBy(post => post.Date).ToList();

        foreach (var post in blogPosts)
        {
            Console.WriteLine($"Datum: {post.Date.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"Titel: {post.Title}");
            Console.WriteLine($"Innehåll: {post.Content}");
            Console.WriteLine();
        }
    }

    static void SearchPosts()
    {
        Console.Write("Skriv in sökord: ");
        string searchKeyword = Console.ReadLine().ToLower();

        var matchingPosts = blogPosts.Where(post => post.Title.ToLower().Contains(searchKeyword));

        if (matchingPosts.Count() == 0)
        {
            Console.WriteLine("Inga matchande inlägg hittades.");
            return;
        }

        foreach (var post in matchingPosts)
        {
            Console.WriteLine($"Datum: {post.Date.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"Titel: {post.Title}");
            Console.WriteLine($"Innehåll: {post.Content}");
            Console.WriteLine();
        }
    }

    static void DeletePost()
    {
        Console.Write("Skriv in titeln på inlägget du vill ta bort: ");
        string deleteTitle = Console.ReadLine();

        var postToDelete = blogPosts.FirstOrDefault(post => post.Title.Equals(deleteTitle, StringComparison.OrdinalIgnoreCase));

        if (postToDelete != null)
        {
            blogPosts.Remove(postToDelete);
            Console.WriteLine($"Inlägget med titeln '{deleteTitle}' har tagits bort.");
        }
        else
        {
            Console.WriteLine($"Inlägget med titeln '{deleteTitle}' hittades inte.");
        }
        SavePostsToFile();
    }
}

class BlogPost
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public string Content { get; set; }
}
