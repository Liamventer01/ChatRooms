namespace ChatRooms.Helpers
{
    public class FormatTime
    {
        public static string FormatTimeStamp(DateTime messageTime, DateTime currentTime)
        {
            if (messageTime.Date == currentTime.Date)
            {
                return "Today at " + messageTime.ToString("h:mm tt").ToUpper();
            }
            else if (messageTime.Date == currentTime.Date.AddDays(-1))
            {
                return "Yesterday at " + messageTime.ToString("h:mm tt").ToUpper();
            }
            else
            {
                return messageTime.ToString("MM/dd/yyyy h:mm tt").ToUpper();
            }
        }
    }
}
