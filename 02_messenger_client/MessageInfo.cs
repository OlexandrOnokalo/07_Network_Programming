using System;

namespace _02_messenger_client
{
    public class MessageInfo
    {
        public MessageInfo(string nick, string text, DateTime time)
        {
            Nick = nick;
            Text = text;
            Time = time;
        }

        public string Nick { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool IsSystem => string.IsNullOrEmpty(Nick);

        public override string ToString()
        {
            if (IsSystem)
                return $"— {Text} —";
            return $"{Nick}: {Text} ({Time:HH:mm})";
        }
    }
}
