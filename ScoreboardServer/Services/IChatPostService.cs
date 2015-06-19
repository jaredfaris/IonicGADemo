using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    interface IChatPostService
    {
        void PostToSlack(Player submittingPlayer, string textToPost);
    }
}
