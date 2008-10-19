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
    /// Contains common values for use in PostRankSharp
    /// </summary>
    public class Config
    {
        private static string _apiKey = "";

        /// <summary>
        /// The base URL for all API calls
        /// </summary>
        public static string UrlBase
        {
            get
            {
                return "http://api.postrank.com/v1/";
            }
        }

        /// <summary>
        /// The namespace used for AideRss RSS extensions
        /// </summary>
        public static XNamespace PostRankNs
        {
            get
            {
                XNamespace ns = "http://aiderss.com/xsd/2007-11-30/aiderss-2007-11-30.xsd";
                return ns;
            }
        }

        /// <summary>
        /// The current API key (your domain)
        /// </summary>
        public static string ApiKey
        {
            get
            {
                return _apiKey;
            }
        }

        /// <summary>
        /// Must call this before calling any of the other API calls
        /// </summary>
        /// <param name="apiKey">Your API key, usually your domain</param>
        public static void SetConfigurations(string apiKey)
        {
            _apiKey = apiKey;
        }
    }
}
