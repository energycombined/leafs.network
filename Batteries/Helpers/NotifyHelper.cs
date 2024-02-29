using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Batteries.Helpers
{
    /// <summary>
    /// Base class for the NotifyHelper
    /// </summary>
    public class Notify
    {
        public string message { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    /// <summary>
    /// User notifications helper
    /// </summary>
    public static class NotifyHelper
    {
        public enum NotifyType { info, danger, warning, success };

        /// <summary>
        /// Used for generating notifications that are stored in the session and displayed to the user on the next pageload
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="type">Type (info, danger, warning, success)</param>
        /// <param name="url">Inser an url in the notification text</param>
        public static void Notify(string message, NotifyType type, string url)
        {
            var newNotifyMsg = new Notify
            {
                message = message,
                type = type.ToString(),
                url = url
            };

            var session = HttpContext.Current.Session;

            if (session != null)
            {
                if (session["NotificationsList"] == null)
                {
                    var newList = new List<Notify>();
                    newList.Add(newNotifyMsg);
                    session["NotificationsList"] = newList;
                }
                else
                {
                    var notificationsList = session["NotificationsList"] as List<Notify>;
                    notificationsList.Add(newNotifyMsg);
                }
            }
        }
    }
}