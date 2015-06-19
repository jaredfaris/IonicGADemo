using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using ScoreboardServer.Models;

namespace ScoreboardServer.Services
{
    class ChatPostService : IChatPostService
    {
        public void PostToSlack(Player submittingPlayer, string textToPost)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "token", ConfigurationManager.AppSettings["slackToken"] },
                    { "channel", ConfigurationManager.AppSettings["slackChannel"] },
                    { "text", textToPost},
                    { "username", submittingPlayer.FirstName},
                    { "icon_emoji", string.Format(":{0}:", submittingPlayer.FirstName.ToLower())}
                };

                var content = new FormUrlEncodedContent(values);

                var response = client.PostAsync(ConfigurationManager.AppSettings["slackUrl"], content);

                response.Result.Content.ReadAsStringAsync();
            }
        }
    }
}