using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostRankSharp;

namespace PostRankTester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the api key
            PostRankSharp.Config.SetConfigurations("spaetzel.com");

            string feedUrl = "http://feeds.feedburner.com/spaetzel";

            Console.WriteLine(string.Format("Running PostRank tests on {0}", feedUrl));

            // Get the feed Id

            int feedId = PostRank.GetFeedId(feedUrl);

            Console.WriteLine(String.Format("Feed Id is {0}", feedId));

            // Get the PostRank of all of the items in the feed

            IEnumerable<PostRankItem> feed = PostRank.GetFeed(feedId);

            Console.WriteLine(String.Format("The PostRank of '{0}' is {1}", feed.ElementAt(5).Title, feed.ElementAt(5).PostRank));

            // Get the top post from the last month
            PostRankItem topPost = PostRank.GetTopPosts(feedId, TopPostsPeriod.Month, 1).First();

            Console.WriteLine(String.Format("The top post in the last month is '{0}' with a PostRank of {1}", topPost.Title, topPost.PostRank));


            // Compare the PostRank's of several URLs
            var urls = new List<string>()
            {
                "http://flickr.com/photos/14009462@N00/2654539960/",
                "http://www.flickr.com/photos/21418584@N07/2447928272/",
                "http://www.flickr.com/photos/pilou/2655293624/"
            };

            var thematicItems = PostRank.GetPostRank(urls);

            foreach (var curItem in thematicItems)
            {
                Console.WriteLine("The Thematic PostRank of '{0}' is {1}", curItem.Title.Length > 0 ? curItem.Title : curItem.Link, curItem.PostRank);
            }


            // Get the feed Id for a 2nd blog
            int feedId2 = PostRank.GetFeedId("http://feeds.feedburner.com/deysca");

            // Compare the postranks of posts from the two feeds

            urls = new List<string>()
            {
                "http://feeds.feedburner.com/~r/Deysca/~3/329747930/",
                "http://feeds.feedburner.com/~r/spaetzel/~3/326933691/",
            };

            var feedIds = new List<int>()
            {
                feedId2,
                feedId
            };

            thematicItems = PostRank.GetPostRank(urls, feedIds);

            foreach (var curItem in thematicItems)
            {
                Console.WriteLine("The Feed thematic PostRank of '{0}' is {1}", curItem.Title.Length > 0 ? curItem.Title : curItem.Link, curItem.PostRank);
            }




            Console.WriteLine("Press Enter to Continue");

            Console.ReadLine();
        }
    }
}
