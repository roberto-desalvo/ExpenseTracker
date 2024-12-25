using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDS.ExpenseTracker.Desktop.WPF.Commands
{
    public class EventAggregator
    {
        private static readonly Lazy<EventAggregator> instance = new(() => new EventAggregator());
        public static EventAggregator Instance => instance.Value;

        private readonly Dictionary<Type, List<Delegate>> subscribers = new();

        public void Subscribe<T>(Action<T> action)
        {
            if (!subscribers.ContainsKey(typeof(T)))
            {
                subscribers[typeof(T)] = new List<Delegate>();
            }
            subscribers[typeof(T)].Add(action);
        }

        public void Publish<T>(T message)
        {
            if (subscribers.ContainsKey(typeof(T)))
            {
                foreach (var action in subscribers[typeof(T)].Cast<Action<T>>())
                {
                    action(message);
                }
            }
        }
    }
}
