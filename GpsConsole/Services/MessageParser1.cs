using System;
using System.Linq;
using System.Reflection;
using GpsConsole.Interfaces;
using GpsConsole.Models;

namespace GpsConsole.Services
{
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