using System;
using System.Collections.Generic;
using System.Text;

namespace GpsConsole.Interfaces
{
    public interface IRepository<in T>
    where T : IEntity
    {
        void SaveToDatabase(T entity);

    }
}
