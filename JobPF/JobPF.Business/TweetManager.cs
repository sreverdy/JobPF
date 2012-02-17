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
        const string consumerKey = "NOT ON GITHUB";
        const string consumerSecret ="NOT ON GITHUB";
        const string token = "NOT ON GITHUB";
        const string secret = "NOT ON GITHUB";

        public TwitterStatus SendTweet(string message)
        {
            TwitterService service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(token, secret);

            return service.SendTweet(message);
            
        }
    }
}
