/******************************************************************************
 * PostRankSharp 1.0
 * Copyright 2008 William Spaetzel
 * Visit http://spaetzel.com/postranksharp
 * 
 * This code is relased under the Creative Commons Attribution 3.0 License
 * http://creativecommons.org/licenses/by/3.0/
 * 
 * You are free to reuse and modify this code as long as your attribte 
 * the original author: William Spaetzel http://spaetzel.com
 *******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PostRankSharp
{

    /// <summary>
    /// Used to specify Time periods for the TopPosts call
    /// </summary>
    public enum TopPostsPeriod { Day, Week, Month, Year, Auto };

    /// <summary>
    /// Used to specify the level of posts to return for the Feed call
    /// </summary>
    public enum PostRankLevel { All, Good, Great, Best };

    /// <summary>
    /// Contains calls to get PostRank data from PostRank.com
    /// </summary>
    public static class PostRank
    {

        /// <summary>
        /// Returns the PostRank feed Id for the given Feed Url
        /// Throws an exception containing the PostRank error if an error occurs such as if the Url was not found
        /// </summary>
        /// <param name="feedUrl">The url to get the Feed Id for</param>
        /// <returns>the PostRank feed Id for the given Feed Url</returns>
        public static int GetFeedId(string feedUrl)
        {
            string url = Config.UrlBase + "feed_id?appkey=" + Config.ApiKey + "&format=xml&url=" + feedUrl;


            XDocument doc = XDocument.Load(url);

            if (doc.Descendants("error").Count() == 0)
            {
                var id = from result in doc.Descendants("feed_id")
                         select Convert.ToInt32(result.Value.ToString());

                return id.ToList()[0];
            }
            else
            {
                throw new Exception(doc.Descendants("error").First().Value);
            }



        }

        /// <summary>
        /// Returns the top posts for the specified time period and feed.
        /// </summary>
        /// <param name="feedId">The internal AideRSS feed ID</param>
        /// <param name="period">The period to return posts from.</param>
        /// <param name="num">The number of posts to return.</param>
        /// <returns>the top posts for the specified time period and feed.</returns>
        private static IEnumerable<PostRankItem> GetTopPosts(int feedId, string period, int num)
        {

            string url = Config.UrlBase + "top_posts?appkey=" + Config.ApiKey + "&format=rss&feed_id=" + feedId + "&period=" + period + "&num=" + num.ToString();

            XDocument doc = XDocument.Load(url);

            var posts = from item in doc.Descendants("item")
                        select new PostRankItem(item);

            return posts;
        }

        /// <summary>
        /// Returns the top posts for the specified time period and feed.
        /// </summary>
        /// <param name="feedId">The internal AideRSS feed ID</param>
        /// <param name="period">The period to return posts from.</param>
        /// <param name="num">The number of posts to return.</param>
        /// <returns>the top posts for the specified time period and feed.</returns>
        public static IEnumerable<PostRankItem> GetTopPosts(int feedId, TopPostsPeriod period, int num)
        {
            string periodString;

            switch (period)
            {
                case TopPostsPeriod.Day:
                    periodString = "day";
                    break;
                case TopPostsPeriod.Week:
                    periodString = "week";
                    break;
                case TopPostsPeriod.Month:
                    periodString = "month";
                    break;
                case TopPostsPeriod.Year:
                    periodString = "year";
                    break;
                case TopPostsPeriod.Auto:
                default:
                    periodString = "auto";
                    break;
            }

            return GetTopPosts(feedId, periodString, num);
        }

        /// <summary>
        /// Returns the top posts for the specified time period and feed.
        /// </summary>
        /// <param name="feedId">The internal AideRSS feed ID</param>
        /// <param name="period">The number of seconds in the past to return posts from.</param>
        /// <param name="num">The number of posts to return.</param>
        /// <returns>the top posts for the specified time period and feed.</returns>
        public static IEnumerable<PostRankItem> GetTopPosts(int feedId, int periodSeconds, int num)
        {
            return GetTopPosts(feedId, periodSeconds.ToString(), num);
        }

        /// <summary>
        /// Returns entries for a specified feed ID, along with additional PostRank data.
        /// http://postrank.com/api/feed.html
        /// </summary>
        /// <param name="feedId">Internal AideRSS feed ID</param>
        /// <returns></returns>
        public static IEnumerable<PostRankItem> GetFeed(int feedId)
        {
            return GetFeed(feedId, PostRankLevel.All, 30, null);
        }

        /// <summary>
        /// Returns entries for a specified feed ID, along with additional PostRank data.
        /// http://postrank.com/api/feed.html
        /// </summary>
        /// <param name="feedId">Internal AideRSS feed ID</param>
        /// <param name="level">Level of entries to filter out.</param>
        /// <param name="num">Number of entries to return.</param>
        /// <param name="start">(Optional) Entry to start returning from (for pagination).</param>
        /// <returns>entries for a specified feed ID, along with additional PostRank data</returns>
        public static IEnumerable<PostRankItem> GetFeed(int feedId, PostRankLevel level, int num, int? start)
        {
            string levelString;

            switch (level)
            {
                case PostRankLevel.Best:
                    levelString = "best";
                    break;
                case PostRankLevel.Good:
                    levelString = "good";
                    break;
                case PostRankLevel.Great:
                    levelString = "great";
                    break;
                case PostRankLevel.All:
                default:
                    levelString = "all";
                    break;
            }

            return GetFeed(feedId, levelString, num, start);
        }

        /// <summary>
        /// Returns entries for a specified feed ID, along with additional PostRank data.
        /// http://postrank.com/api/feed.html
        /// </summary>
        /// <param name="feedId">Internal AideRSS feed ID</param>
        /// <param name="level">The minimum PostRank of the entries to return</param>
        /// <param name="num">Number of entries to return.</param>
        /// <param name="start">(Optional) Entry to start returning from (for pagination).</param>
        /// <returns>entries for a specified feed ID, along with additional PostRank data</returns>
        public static IEnumerable<PostRankItem> GetFeed(int feedId, decimal level, int num, int? start)
        {
            return GetFeed(feedId, level.ToString(), num, start);
        }

        /// <summary>
        /// Returns entries for a specified feed ID, along with additional PostRank data.
        /// http://postrank.com/api/feed.html
        /// </summary>
        /// <param name="feedId">Internal AideRSS feed ID</param>
        /// <param name="level">Level of entries to filter out.</param>
        /// <param name="num">Number of entries to return.</param>
        /// <param name="start">(Optional) Entry to start returning from (for pagination).</param>
        /// <returns>entries for a specified feed ID, along with additional PostRank data</returns>
        private static IEnumerable<PostRankItem> GetFeed(int feedId, string level, int num, int? start )
        {
            string url = Config.UrlBase + "feed?appkey=" + Config.ApiKey + "&format=rss&feed_id=" + feedId + "&level=" + level + "&num=" + num.ToString();

            if (start != null)
            {
                url += String.Format("&start={0}", start.Value);
            }



            XDocument doc = XDocument.Load(url);

            try
            {
                var posts = from item in doc.Descendants("item")
                            select new PostRankItem(item);


                return posts;
            }
            catch (Exception ex)
            {
                throw new Exception("Problem parsing PostRank feed at " + url, ex);
            }
        }

        /// <summary>
        /// Returns the thematic PostRank for the given urls
        /// http://postrank.com/api/postrank.html
        /// </summary>
        /// <param name="urls">The Urls to get the PostRanks for</param>
        /// <returns>the thematic PostRank for the given urls</returns>
        public static IEnumerable<PostRankItem> GetPostRank(List<string> urls)
        {
            string url = Config.UrlBase + "entry_stats?appkey=" + Config.ApiKey + "&format=rss";

            string urlParameter = "";
            foreach (var curUrl in urls.Where(u => u != ""))
            {
                urlParameter += "&url[]=" + curUrl;
            }

            if (urlParameter != "")
            {
                try
                {
                    string response = Utilities.HttpPost(url, urlParameter);

                    XDocument doc = XDocument.Parse(response);

                    var posts = from item in doc.Descendants("item")
                                select new PostRankItem(item);

                    return posts;
                }
                catch
                {
                    return new List<PostRankItem>();
                }

            }
            else
            {
                return new List<PostRankItem>();
            }
        }


        /// <summary>
        /// Returns the thematic PostRank for the given urls
        /// http://postrank.com/api/postrank.html
        /// </summary>
        /// <param name="urls">The Urls to get the PostRanks for</param>
        /// <param name="feedIds">The Feed Ids that correspond to the given Urls</param>
        /// <returns>the thematic PostRank for the given urls</returns>
        public static IEnumerable<PostRankItem> GetPostRank(List<string> urls, List<int>feedIds)
        {
            string url = Config.UrlBase + "entry_stats?appkey=" + Config.ApiKey + "&format=rss";

            string urlParameter = "";
            foreach (var curUrl in urls.Where(u => u != ""))
            {
                urlParameter += "&url[]=" + curUrl;
            }

            string feedIdParameter = "";
            foreach (var curFeedId in feedIds)
            {
                feedIdParameter += "&feedId=" + curFeedId.ToString();
            }

            if (urlParameter != "")
            {
                try
                {
                    string response = Utilities.HttpPost(url, urlParameter + feedIdParameter);

                    XDocument doc = XDocument.Parse(response);

                    var posts = from item in doc.Descendants("item")
                                select new PostRankItem(item);

                    return posts;
                }
                catch
                {
                    return new List<PostRankItem>();
                }

            }
            else
            {
                return new List<PostRankItem>();
            }
        }

        /// <summary>
        /// Returns a decimal value for the given PostRank string
        /// </summary>
        /// <param name="postRankString">The string to convert to a number</param>
        /// <returns>a decimal value for the given PostRank string</returns>
        public static decimal ParsePostRank(string postRankString)
        {
            if (postRankString.Trim() == string.Empty)
            {
                return 1;
            }
            else
            {
                try
                {
                    return Convert.ToDecimal(postRankString);
                }
                catch
                {
                    // Probably not a decimal
                    switch (postRankString.ToLower().Trim())
                    {
                        case "good":
                            return 2;
                        case "great":
                            return 4;
                        case "best":
                            return 8;
                        case "all":
                            return 1;
                        default:
                            throw new Exception("The given value was not a postRank");

                    }
                }
            }
        }





    }
}
