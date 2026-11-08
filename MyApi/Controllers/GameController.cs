using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]

    [Route("api/games")]
    public class GameController : ControllerBase
    {
        public static List<Game> Games { get; set; } = new List<Game>()
        {
            new Game() { Id = 1, Title = "Game 1", Priority = 1, IsCompleted = false },
            new Game() { Id = 2, Title = "Game 2", Priority = 2, IsCompleted = true },
            new Game() { Id = 3, Title = "Game 3", Priority = 3, IsCompleted = false },
            new Game() { Id = 4, Title = "Game 4", Priority = 5, IsCompleted = false },
        };

        [HttpGet]
        [ProducesResponseType(typeof(List<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<Game>> GetGames()
        {
            if (Games == null || Games.Count == 0)
            {
                return NotFound("No games found.");
            }

            return Ok(Games);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult UpdateGame(int id, Game newGame)
        {
            var game = Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }

            if (Games.Any(g => g.Title == newGame.Title && g.Id != id))
            {
                return Conflict("Game with this title already exists.");
            }

            game.Title = newGame.Title;
            game.Priority = newGame.Priority;

            return Ok(game);
        }

        [HttpPost("{id:int}/complete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult CompleteGame(int id)
        {
            var game = Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }

            if (game.IsCompleted)
            {
                return Conflict("Game is already completed.");
            }

            game.IsCompleted = true;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult DeleteGame(int id)
        {
            var game = Games.FirstOrDefault(g => g.Id == id);

            if (game == null)
            {
                return NotFound($"Game with ID {id} not found.");
            }

            if (game.Priority == 5 && !game.IsCompleted)
            {
                return Conflict("Cannot delete a game with priority 5 that is not completed.");
            }

            Games.Remove(game);

            return NoContent();
        }

        [HttpDelete("completed")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult DeleteCompletedGames()
        {
            int count = Games.Count(g => g.IsCompleted);

            Games.RemoveAll(g => g.IsCompleted);

            return Ok(count);
        }

    }
}
