using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ScoreboardServer.Models;
using ScoreboardServer.Services;

namespace ScoreboardServer.Controllers
{
    public class RegisterScoreController : ApiController
    {
        private IVotePersister _votePersister;
        private ScoreboardContext _scoreboardContext;
        private IChatPostService _chatPostService;
        private IJokeGenerator _jokeGenerator;

        public RegisterScoreController()
        {
            _scoreboardContext = new ScoreboardContext();
            _votePersister = new VotePersister(_scoreboardContext);
            _chatPostService = new ChatPostService();
            _jokeGenerator = new JokeGenerator();
        }

        [Route("Score/{playerId}")]
        public string Get(int playerId)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            return _votePersister.GetCountBy(player).ToString();
        }

        [Route("Score/{playerId}")]
        public void Post(int playerId)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            _votePersister.PersistVote(player);

            var jokeText = "I clicked a button!";
            _chatPostService.PostToSlack(player, jokeText); 
        }

        [Route("Words/{playerId}/{yellText}")]
        public void Post(int playerId, string yellText)
        {
            var player = _votePersister.FindPlayerBy(playerId);
            _votePersister.PersistVote(player);

            var jokeText = _jokeGenerator.GenerateJoke(player, yellText);
            _chatPostService.PostToSlack(player, jokeText);
        }

        [Route("Players/")]
        public List<Player> GetPlayers()
        {
            var players =  _votePersister.GetAllPlayers();

           return players;
        }

        [Route("Scoreboard/")]
        public List<Player> GetScoreboard()
        {
            var players = _votePersister.GetAllPlayers();

            players.ForEach(p =>
            {
                var validVotes = p.Votes.Where(v => v.CreatedAt >= DateTime.UtcNow.AddMinutes(-5)).ToList();
                p.Votes = validVotes;
            });

            return players;
        } 
    }
}
