namespace MyApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Priority { get; set; }
        public int Rating { get; set; }
        public bool IsCompleted { get; set; }
    }
}
