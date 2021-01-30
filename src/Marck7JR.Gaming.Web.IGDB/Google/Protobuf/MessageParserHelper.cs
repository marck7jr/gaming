using System;
using System.Reflection;

namespace Google.Protobuf
{
    public static class MessageParserHelper
    {
        public static MessageParser GetMessageParser(Type type) => (MessageParser)type.GetProperty("Parser", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
        public static MessageParser GetMessageParser<T>() where T : IMessage => GetMessageParser(typeof(T));
        public static MessageParser GetMessageParser<T>(this T _) where T : IMessage => GetMessageParser(typeof(T));
    }
}
