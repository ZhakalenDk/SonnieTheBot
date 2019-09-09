using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.OS.FacebookHook.Data
{
    /// <summary>
    /// Represents a facebook data object
    /// </summary>
    public class FacebookData
    {
        /// <summary>
        /// ID of the object
        /// </summary>
        public string id = null;
        /// <summary>
        /// The facebook feed object retrieved from a group
        /// </summary>
        public FacebookFeed feed = null;

        /// <summary>
        /// Represents a facebook group feed
        /// </summary>
        public class FacebookFeed
        {
            /// <summary>
            /// The retrieved data of the feed
            /// </summary>
            public List<FacebookPost> data = null;
            /// <summary>
            /// The feed paging
            /// </summary>
            public FacebookPage paging = null;

            /// <summary>
            /// Represents a facebook post
            /// </summary>
            public class FacebookPost
            {
                public string full_picture = null;
                /// <summary>
                /// The written content of the post
                /// </summary>
                public string message = null;
                /// <summary>
                /// When the post was last updated
                /// </summary>
                public string updated_time = null;
                /// <summary>
                /// The ID of the post
                /// </summary>
                public string id = null;
                /// <summary>
                /// The data feed on the comments section of this post
                /// </summary>
                public FacebookCommentFeed<FacebookComment> comments = null;

                /// <summary>
                /// Get the Date and time of which the post was updated
                /// </summary>
                /// <returns></returns>
                public DateTime GetPostTime ()
                {
                    string[] updateTime = this.updated_time.Split ( 'T' );
                    string[] date = updateTime[0].Split ( '-' );
                    string[] time = updateTime[1].Split ( ':' );

                    return new DateTime ( int.Parse ( date[0] ), int.Parse ( date[1] ), int.Parse ( date[2] ), int.Parse ( time[0] ), int.Parse ( time[1] ), int.Parse ( time[2].Split ( '+' )[0] ) );
                }

                /// <summary>
                /// Represents a data feed on the comments section of a Facebook post
                /// </summary>
                /// <typeparam name="T">Determines which datatype should be stored</typeparam>
                public class FacebookCommentFeed<T>
                {
                    /// <summary>
                    /// The data from the comments section
                    /// </summary>
                    public List<T> data = null;
                    public FacebookCommentPage paging = null;

                    /// <summary>
                    /// Represents a comments page
                    /// </summary>
                    public class FacebookCommentPage
                    {
                        public FacebookInnerPaging cursers = null;
                        public class FacebookInnerPaging
                        {
                            public string before = null;
                            public string after = null;
                        }
                    }

                }

                /// <summary>
                /// Represents a Facebook comment
                /// </summary>
                public class FacebookComment
                {
                    /// <summary>
                    /// THe ID of the comment
                    /// </summary>
                    public string id = null;
                    /// <summary>
                    /// The string conetent of the comment
                    /// </summary>
                    public string message = null;
                    /// <summary>
                    /// The Data feed of the subcomment section
                    /// </summary>
                    public FacebookCommentFeed<FacebookSubComment> comments = null;

                    /// <summary>
                    /// Represents a Facebook subcomment. (A comment within a comment)
                    /// </summary>
                    public class FacebookSubComment
                    {
                        /// <summary>
                        /// The ID of the subcomment
                        /// </summary>
                        public string id = null;
                        /// <summary>
                        /// THe string content of the subcomment
                        /// </summary>
                        public string message = null;
                    }
                }
            }

            /// <summary>
            /// Represents a facebook Page
            /// </summary>
            public class FacebookPage
            {
                public string previous = null;
                public string next = null;
            }
        }
    }
}
