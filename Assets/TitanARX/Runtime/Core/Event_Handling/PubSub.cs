using System.Collections.Generic;
using System;

public interface IMessage { }

public struct MyMessage : IMessage
{
    public string content;
}

public static class PubSub
{
    private static Dictionary<string, Action<IMessage>> _channels = new Dictionary<string, Action<IMessage>>();

    public static void Subscribe<T>(string channel, Action<T> listener) where T : IMessage
    {
        if (!_channels.ContainsKey(channel))
            _channels[channel] = (x) => listener((T)x);
        else
            _channels[channel] += (x) => listener((T)x);
    }

    public static void Unsubscribe<T>(string channel, Action<T> listener) where T : IMessage
    {
        if (_channels.ContainsKey(channel))
            _channels[channel] -= (x) => listener((T)x);
    }

    public static void Publish<T>(string channel, T message) where T : IMessage
    {
        if (_channels.ContainsKey(channel))
            _channels[channel](message);
    }
}