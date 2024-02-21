using LyricsScraperNET;
using LyricsScraperNET.Models.Requests;

bool isDone = false;
while (!isDone)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("|------------------------------------------------------------------------------------------------------------------------------------------------|\n|Hello, and welcome to Lab 5                                                                                                                     |\n|------------------------------------------------------------------------------------------------------------------------------------------------|\n|Tired of searching a dozen different sites for lyrics to your favorite songs?                                                                   |\n|Have no fear - LyricScraperNET is here.                                                                                                         |\n|Press any key for two consecutive prompts, one for the name of the artist whose song you'd like to search for, and another for the song name.   |\n|The API will then search for it and attempt to return some lyrics. Press escape to exit.                                                        |\n|Happy singing (coding)!                                                                                                                         |\n|------------------------------------------------------------------------------------------------------------------------------------------------|\n");
    var input = Console.ReadKey();

    if (input.Key == ConsoleKey.Escape)
    {
        isDone = true;
        Console.ForegroundColor= ConsoleColor.White;
        Console.WriteLine("\nAAdios, amigo.");
    }

    else
    {
        await LyricFinder();
    }
}

static async Task LyricFinder()
{
    Console.ForegroundColor = ConsoleColor.Yellow;

    Console.WriteLine("Artist Name (First Last):");
    string? artistInput = Console.ReadLine();

    Console.WriteLine("Song Name:");
    string? songInput = Console.ReadLine();

    ILyricsScraperClient lyricsScraperClient
        = new LyricsScraperClient()
            .WithAllProviders();

    var searchRequest = new ArtistAndSongSearchRequest(artist: artistInput, song: songInput);
    var searchResult = lyricsScraperClient.SearchLyric(searchRequest);

    Console.ForegroundColor = ConsoleColor.White;

    Task timeout = Task.Delay(10000);
    Task result = await Task.WhenAny(Result(), timeout);

    if (result == timeout)
    { 
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: search timed out! Please try again later.\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }

    Task Result()
    {
        if (!searchResult.IsEmpty())
            Console.WriteLine($"\n{searchResult.LyricText}");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n---------------------------------------------------------\nHere's your results. Press enter to continue...");
        Console.ReadLine();
        Console.Clear();

        return Task.CompletedTask;
    }
}