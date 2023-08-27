using System;
using System.Collections.Generic;

namespace Assets.Scripts.General
{
    public static class EventManager
    {
        private static Dictionary<string, Action<object[]>> events = new Dictionary<string, Action<object[]>>();


        public static void Subscribe(string key, Action<object[]> handler)
        {
            if (events.TryGetValue(key, out var value))
            {
                value += handler;
                events[key] = value;
            }

            else
                events.Add(key, handler);
        }

        public static void Unsubscribe(string key)
        {
            if (events.TryGetValue(key, out var value))
                events.Remove(key);
        }

        public static void Invoke(string key, params object[] args)
        {
            events[key]?.Invoke(args);
        }
    }
}