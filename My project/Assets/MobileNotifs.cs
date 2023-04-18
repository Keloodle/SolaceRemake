using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class MobileNotifs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Remove Notifications that have already been displayed
        AndroidNotificationCenter.CancelAllDisplayedNotifications();

        //Notification channel setup
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Notification Channel",
            Importance = Importance.Default,
            Description = "Reminder notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //Setup for the notification itself
        var notification = new AndroidNotification();
        notification.Title = "Play our game!";
        notification.Text = "Buy our stuff!";
        notification.FireTime = System.DateTime.Now.AddSeconds(10);

        //Setup for the notification itself
        var notification2 = new AndroidNotification();
        notification2.Title = "Did you forget us?";
        notification2.Text = "We miss you </3";
        notification2.FireTime = System.DateTime.Now.AddDays(7);

        //Send the notification
        var id =AndroidNotificationCenter.SendNotification(notification, "channel_id");
        var id2 =AndroidNotificationCenter.SendNotification(notification2, "channel_id");

        //If the script is run and a message is already scheduled, cancel it and re-schedule another message
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id2) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification2, "channel_id");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
