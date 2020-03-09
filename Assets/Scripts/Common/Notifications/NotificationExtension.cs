using Handler = System.Action<object, object>;

namespace Assets.Scripts.Common.Notifications
{
	public static class NotificationExtension
    {
		public static void PostNotification(this object obj, string notificationName)
		{
			NotificationCenter.Instance.PostNotification(notificationName, obj);
		}

		public static void PostNotification(this object obj, string notificationName, object e)
		{
			NotificationCenter.Instance.PostNotification(notificationName, obj, e);
		}

		public static void AddObserver(this object obj, Handler handler, string notificationName)
		{
			NotificationCenter.Instance.AddObserver(handler, notificationName);
		}

		public static void AddObserver(this object obj, Handler handler, string notificationName, object sender)
		{
			NotificationCenter.Instance.AddObserver(handler, notificationName, sender);
		}

		public static void RemoveObserver(this object obj, Handler handler, string notificationName)
		{
			NotificationCenter.Instance.RemoveObserver(handler, notificationName);
		}

		public static void RemoveObserver(this object obj, Handler handler, string notificationName, object sender)
		{
			NotificationCenter.Instance.RemoveObserver(handler, notificationName, sender);
		}
	}
}
