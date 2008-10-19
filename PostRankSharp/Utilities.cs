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
using System.Net;
using System.IO;
using System.Xml.Linq;

namespace PostRankSharp
{
    /// <summary>
    /// Contains Utility methods for the library
    /// </summary>
    internal static class Utilities
    {
        /// <summary>
        /// Makes an HTTP post
        /// </summary>
        /// <param name="uri">The URI to post to</param>
        /// <param name="parameters">The parameters to post</param>
        /// <returns>The response from the server</returns>
        public static string HttpPost(string uri, string parameters)
        {

            WebRequest webRequest = WebRequest.Create(uri);

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(parameters);
            Stream os = null;
            try
            { // send the Post
                webRequest.ContentLength = bytes.Length;   //Count bytes to send
                os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);         //Send it
            }
            catch (WebException ex)
            {
                throw ex;
            }
            finally
            {
                if (os != null)
                {
                    os.Close();
                }
            }

            try
            { // get the response
                WebResponse webResponse = webRequest.GetResponse();
                if (webResponse == null)
                { return null; }
                StreamReader sr = new StreamReader(webResponse.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// Get the value of the given element
        /// Returns the empty string if the element is null
        /// </summary>
        /// <param name="element">The element to get the value for</param>
        /// <returns>The element's value</returns>
        public static string GetElementValue(XElement element)
        {
            return element == null ? String.Empty : element.Value;
        }
    }
}
