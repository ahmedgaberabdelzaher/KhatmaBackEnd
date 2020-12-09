using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Managers.Classes;
using KhatmaBackEnd.Managers.Interfaces;
using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KhatmaBackEnd.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        KhatmaContext _KhatmaContext;
        IMapper _Mapper;
        IUserManager _UserManager;
        INotificationManager _notificationManager;
        public UserController(KhatmaContext khatmaContext, IMapper Mapper, IUserManager userManager,INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
            _UserManager = userManager;
            _KhatmaContext = khatmaContext;
            _Mapper = Mapper;
        }
        [HttpGet("ByGroupId/{groupId}")]
        public IActionResult GetUsersByGroupId(int groupId)
        {
            //return Ok(_KhatmaContext.Users.Where(c=>c.GroupId==groupId));
            var data = _UserManager.GetUsersByGroupId(groupId);
            return Ok(data);
        } 
        [HttpGet("SendNotifictaion/{deviceId}")]
        public IActionResult SendNotifictaion(string deviceId)
        {
            //return Ok(_KhatmaContext.Users.Where(c=>c.GroupId==groupId));
            return Ok(NotificationManager.SendPushNotification(deviceId));
        } 
        
        [HttpGet()]
        public IActionResult GetAllUsers()
        {
           // return Ok(_KhatmaContext.Users);
            return Ok(_UserManager.GetAll());
        }

        [HttpPost()]
        public IActionResult AddNewUser([FromBody]UserForAdd user)
        {
            if (user == null)
            {
                return BadRequest(new ProcessResult<UserForAdd>() {Data=null,IsSucceeded=false,Status="400",MethodName= "AddNewUser",Message="Invalid Data" });
            }
            else
            {
                // var isCurrentNameExist = _KhatmaContext.Users.Any(c => c.UserName == user.UserName);
                var isCurrentNameExist = _UserManager.IsUserNameExist(user.UserName);
                if (isCurrentNameExist)
                {
                    return BadRequest(new ProcessResult<UserForAdd>() { Data = null, IsSucceeded = false, Status = "400", MethodName = "AddNewUser", Message = "User Name IsExist" });
                }
                else
                {

                    return Created("", _UserManager.AddNewUser(_Mapper.Map<UserForAdd>(user)));
                  //  return Ok("User added"+ _KhatmaContext.Users.Count());
                }
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody]LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                return Ok(_UserManager.Login(loginViewModel));
            }
            else
            {
                return BadRequest( new ProcessResult<LoginResponseViewModel>()
                {
                    Data = null,
                    IsSucceeded = false,
                    Status = "400",
                    Message="Invalid Data"
                });
            }
        }

        [HttpPut("ChangeReadStatus")]
        public IActionResult ChangeReadStatus(ChangeReadStatusViewModel changeReadStatusViewModel)
        {
            if (ModelState.IsValid)
            {
                var Resault = _UserManager.ChangeReadStatus(changeReadStatusViewModel);
                return Ok(Resault);
            }
            else
            {
                    return BadRequest(new ProcessResult<bool>()
                    {
                        Data = false,
                        IsSucceeded = false,
                        Message = "Invalid Data All Required",
                        Status = "400"
                    });
                
            }

        }
        
        [HttpDelete("DeleteUser/{userId}")]
         public IActionResult DeleteUser(int userId)
        {
            return Ok(_UserManager.DeleteUser(userId));
        }

        [HttpDelete("LogOut")]
        public IActionResult DeleteUser(string deviceId)
        {
            try
            {
                return Ok(_UserManager.Logout(deviceId));

            }
            catch (Exception ex)
            {
                return Ok(
                                     new ProcessResult<bool>()
                                     {
                                         Data = false,
                                         IsSucceeded = false,
                                         Status = "400",
                                         Message = ex.Message
                                     }
                    ) ;
            }
        }
    }
}