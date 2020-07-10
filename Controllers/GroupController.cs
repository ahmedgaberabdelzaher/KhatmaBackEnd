using AutoMapper;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Managers.Interfaces;
using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Controllers
{
    [ApiController]
    [Route("api/Group")]
    public class GroupController:ControllerBase
    {
        KhatmaContext _KhatmaContext;
        IMapper _Mapper;
        IGroupManager _groupManager;
        public GroupController(KhatmaContext khatmaContext,IMapper Mapper,IGroupManager groupManager)
        {
            _KhatmaContext = khatmaContext;
            _Mapper = Mapper;
            _groupManager = groupManager;
        }
     [HttpGet]
      public IActionResult GetAllGroups()
        {

            return Ok(_groupManager.GetAll());
            //return _Mapper.Map<IEnumerable<GroupViewModel>>(Groups);
        }
        [HttpPost]
        public IActionResult NewGroup(GroupForCreateViewModel groupForCreateViewModel)
        {
            if (groupForCreateViewModel == null)
            {
                return BadRequest(new ProcessResult<bool>() { Data = false, IsSucceeded = false, Status = "400", MethodName = "AddNewGroup", Message = "Invalid Data" });
            }
            else
            {
                if (_groupManager.IsGroupExist(groupForCreateViewModel.Name))
                {
                    return BadRequest(new ProcessResult<bool>() { Data = false, IsSucceeded = false, Status = "400", MethodName = "AddNewGroup", Message = "group Name Exist" });
                }
                return Created("", _groupManager.AddNewGroup(groupForCreateViewModel));
            }
        }
    }
}
