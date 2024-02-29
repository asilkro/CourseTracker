using System.Diagnostics;
using CourseTracker.Maui.Supplemental;
using Plugin.LocalNotification;
using SQLite;

namespace CourseTracker.Maui.Models
{
    [Table("Notification")]
    public class Notification
    {
        #region Properties
        [PrimaryKey]
        [AutoIncrement]
        public int NotificationId { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime NotificationDate { get; set; }
        public string RelatedItemType { get; set; } // Course, Assessment
        public int NotificationTriggered { get; set; }
        #endregion

        #region Constructors
        public Notification()
        {
        }

        public Notification(int notificationId, string notificationTitle, string notificationMessage, DateTime notificationDate, string relatedItemType, int? relatedItemId)
        {
            NotificationId = notificationId;
            NotificationTitle = notificationTitle;
            NotificationMessage = notificationMessage;
            NotificationDate = notificationDate;
            RelatedItemType = relatedItemType;
        }

        #endregion
    }
}
