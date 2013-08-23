using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TweetSharp;
using System.Diagnostics;

namespace JobPF.Business
{
    public class TweetManager
    {
        private string ConsumerKey { get; set; }
        private string ConsumerSecret { get; set; }
        private string Token { get; set; }
        private string Secret { get; set; }

        public TweetManager(string consumerKey, string consumerSecret, string token, string secret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            Token = token;
            Secret = secret;
        }

        public TwitterStatus SendTweet(string message)
        {
            TwitterService service = new TwitterService(ConsumerKey, ConsumerSecret);
            service.AuthenticateWith(Token, Secret);

            return service.SendTweet(new SendTweetOptions()
                {
                    Status = message
                });
            //return null;
            
        }
    }
}
