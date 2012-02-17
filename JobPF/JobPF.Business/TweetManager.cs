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
        const string consumerKey = "N2GQD4EoHKhMFDHry1EBXA";
        const string consumerSecret = "fkaANyF9vcbFD1eG4BiFHAnNvsJqtVl3ceHLRr7nls";
        const string token = "493799448-UhukodbdT7J2JepcMZekvDS85kFjjNCweG92YLV0";
        const string secret = "kcY7kz26l8qgvCqkWlx9UeOPLHWVG1dkOYmZfKoR0LY";

        public TwitterStatus SendTweet(string message)
        {
            TwitterService service = new TwitterService(consumerKey, consumerSecret);
            service.AuthenticateWith(token, secret);

            return service.SendTweet(message);
            
        }
    }
}
