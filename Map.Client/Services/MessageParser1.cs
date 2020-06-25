namespace Map.Client.Services
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Map.Client.Interfaces;
    using Map.Client.Models;

    public class MessageParser1 : IMessageParser
    {
        public IMessage<IEntity> Parse(PlainMessage message)
        {
            var result =
                Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(x => !x.IsAbstract && !x.IsInterface && x.GetInterfaces().Any(y =>
                        y.IsGenericType && y.GetGenericTypeDefinition() == typeof(IMessage<>)))
                    .Select(x => (IMessage<IEntity>)Activator.CreateInstance(x))
                    .FirstOrDefault(x => x != null && x.CanParse(message));

            return result;
        }
    }
}