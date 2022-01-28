using static System.Net.WebRequestMethods;

namespace Blip.Services
{
    public class GamesLibraryService
    {
        private const string FilesLocation = "games";

        private readonly HttpClient _httpClient;

        public string[] GameNames { get; private set; } = new[]
        {
            "15PUZZLE",
            "BLINKY",
            "BLITZ",
            "BRIX",
            "CONNECT4",
            "GUESS",
            "HIDDEN",
            "INVADERS",
            "KALEID",
            "MAZE",
            "MERLIN",
            "MISSILE",
            "PONG",
            "PONG2",
            "PUZZLE",
            "SYZYGY",
            "TANK",
            "TETRIS",
            "TICTAC",
            "UFO",
            "VBRIX",
            "VERS",
            "WIPEOFF"
        };

        public GamesLibraryService(HttpClient httpClient)
            => _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        public async Task<byte[]> DownloadGameAsync(string gameName)
            => await _httpClient.GetByteArrayAsync(GetGameFilePath(gameName));

        private string GetGameFilePath(string gameName) 
            => $"{FilesLocation}/{gameName}";
    }
}
