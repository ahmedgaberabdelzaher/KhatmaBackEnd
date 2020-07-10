using AutoMapper;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Managers.Interfaces;
using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Classes
{
    public class GroupManager : IGroupManager
    {
        KhatmaContext _KhatmaContext;
        IMapper _Mapper;
        public GroupManager(KhatmaContext khatmaContext, IMapper mapper)
        {
            _KhatmaContext = khatmaContext;
            _Mapper = mapper;
        }
        public ProcessResult<int> AddNewGroup(GroupForCreateViewModel groupForCreateViewModel)
        {
            var groupViewModel = _Mapper.Map<GroupForCreateViewModel, Group>(groupForCreateViewModel);
            _KhatmaContext.Add(groupViewModel);
            Save();
            var groupid = _KhatmaContext.Groups.ToList().Last().Id;
            return new ProcessResult<int>() { Data = groupid, IsSucceeded = true, Message = "Group Added", Status = "201" };

        }

        public ProcessResult<List<GroupViewModel>> GetAll()
        {
            var Groups = _KhatmaContext.Groups;
            var MapedGroups = _Mapper.Map<List<GroupViewModel>>(Groups);
         return  new ProcessResult<List<GroupViewModel>>()
            {
                Data =MapedGroups ,
                IsSucceeded = true,
                Message="",
                Status="200"
            };
        }

        public bool IsGroupExist(string GroupName)
        {
            return _KhatmaContext.Groups.Any(c => c.Name == GroupName);
        }
        public void Save()
        {
            _KhatmaContext.SaveChanges();
        }
    }
}
