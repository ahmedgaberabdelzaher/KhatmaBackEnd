using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Interfaces
{
  public  interface IGroupManager
    {
        public ProcessResult<int> AddNewGroup(GroupForCreateViewModel groupForCreateViewModel);
        public bool IsGroupExist(string GroupName);
        public ProcessResult<List<GroupViewModel>> GetAll();
        public void Save();
    }
}
