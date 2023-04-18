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

        //Send the notification
        var id =AndroidNotificationCenter.SendNotification(notification, "channel_id");

        //If the script is run and a message is already scheduled, cancel it and re-schedule another message
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
