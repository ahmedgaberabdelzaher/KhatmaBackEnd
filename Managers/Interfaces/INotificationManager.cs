using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Interfaces
{
    public interface INotificationManager
    {
        public Task<string> sendNotificationAsync();
    }
}
