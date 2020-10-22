using KhatmaBackEnd.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Interfaces
{
    interface IHangFireJobService
    {
        Task<bool> UpdateKhatmaCountHangfire();
        string NotifyUnreadedUsers();
    }
}
