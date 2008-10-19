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
    /// Contains the values for a single item returned from the PostRank api
    /// </summary>
    public class PostRankItem
    {
        /// <summary>
        /// Creates a new empty postRankItem
        /// </summary>
        public PostRankItem()
        {
        }

        /// <summary>
        /// Creates a new postRankItem with its values filled in with the ones contained in the given XElement
        /// </summary>
        /// <param name="item">The element to use to fill the item in with</param>
        public PostRankItem(XElement item)
        {
            Link = Utilities.GetElementValue(item.Element("link"));
            Title = Utilities.GetElementValue(item.Element("title"));
            PostRank = Convert.ToDouble(Utilities.GetElementValue(item.Element(Config.PostRankNs + "postrank")));
            Postrank_color = Utilities.GetElementValue(item.Element(Config.PostRankNs + "postrank_color"));
            OriginalLink = Utilities.GetElementValue(item.Element(Config.PostRankNs + "original_link"));    
            Guid = Utilities.GetElementValue(item.Element("guid"));

            try
            {
                PubDate = DateTime.Parse(Utilities.GetElementValue(item.Element("pubDate")));
            }
            catch (FormatException)
            {
                PubDate = null;
            }
            
        }


        

        /// <summary>
        /// The date the item was published
        /// </summary>
        public DateTime? PubDate { get; set; }

        /// <summary>
        /// The PostRank tracking Link for this item
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// The Original link of the item
        /// </summary>
        public string OriginalLink { get; set; }

        /// <summary>
        /// The item's title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The item's PostRank
        /// </summary>
        public double PostRank { get; set; }

        /// <summary>
        /// The Color that can be used to render the PostRank visually
        /// </summary>
        public string Postrank_color { get; set; }

        /// <summary>
        /// The unique identifier for this item
        /// </summary>
        public string Guid { get; set; }
    }
}
