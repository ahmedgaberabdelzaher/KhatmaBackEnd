using AutoMapper;
using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Managers.Interfaces;
using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using Microsoft.CodeAnalysis.FlowAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Classes
{

    public class UserManager : IUserManager
    {
        KhatmaContext _KhatmaContext;
        IMapper _Mapper;
        public UserManager(KhatmaContext khatmaContext, IMapper mapper)
        {
            _KhatmaContext = khatmaContext;
            _Mapper = mapper;
        }

        public ProcessResult<UserData> AddNewUser(UserForAdd user)
        {
            var Setting = _KhatmaContext.KhatmaSettings.Where(c=>c.KhatmaId==user.KhatmaId).AsEnumerable().LastOrDefault();
            if (Setting != null)
            {
                if (Setting.LastDistributedPage < 604)
                {
                    user.PageNo = Setting.LastDistributedPage + 1;
                }
                else
                {
                    user.PageNo = 1;
                }
            }
            else
            {
                user.PageNo = 1;
            }
            user.PageDistributedDate = DateTime.UtcNow;
            var userViewModel = _Mapper.Map<UserForAdd, User>(user);
            _KhatmaContext.Add(userViewModel);
            Setting.LastDistributedPage = user.PageNo;
            _KhatmaContext.KhatmaSettings.Update(Setting);
            Save();
            if (String.IsNullOrEmpty(user.Password))
            {
                var LastCreatedUser = _KhatmaContext.Users.AsEnumerable().LastOrDefault();
                userViewModel.Password = (LastCreatedUser?.Id).ToString();
                _KhatmaContext.Users.Update(userViewModel);
                Save();
            }
            return new ProcessResult<UserData>()
            {
               Data = GetUserByUserName(user.UserName).Data,
                IsSucceeded = true,
                Message = "User Added Sucessfuly",
                TotalUserCount = GetAllUsersCount()

            };
        }

        public ProcessResult<bool> ChangeReadStatus(ChangeReadStatusViewModel changeReadStatusViewModel)
        {
            User user = GetUserById(changeReadStatusViewModel.UserId);
            if (user != null)
            {
                user.IsRead = changeReadStatusViewModel.Status;
                user.ReadedDate = DateTime.UtcNow;
                _KhatmaContext.Update(user);
                Save();
                return new ProcessResult<bool>()
                {
                    Data = true,
                    IsSucceeded = true,
                    Status = "200"
                };
            }
            else
            {
                return new ProcessResult<bool>()
                {
                    Data = false,
                    IsSucceeded = false,
                    Status = "400"
                };
            }
        }

        private User GetUserById(int userId)
        {
            return _KhatmaContext.Users.Find(userId);
        }

        public ProcessResult<List<User>> GetAll()
        {
            return new ProcessResult<List<User>>("GetAll") { Data = _KhatmaContext.Users.ToList(), IsSucceeded = true, Status = "Ok", Exception = null };

        }


        public int GetAllUsersCount()
        {
            return _KhatmaContext.Users.Count();
        }

        public ProcessResult<UserData> GetUserByUserName(string userName)
        {
          //  var IsUserNameExists = IsUserNameExist(userName);
            if (IsUserNameExist(userName))
            {
                var data = _KhatmaContext.Users.Where(c => c.UserName == userName).AsEnumerable<User>().ToList().First();
                return new ProcessResult<UserData>("GetUserByUserName") { Data = _Mapper.Map<UserData>(data), IsSucceeded = true, Status = "Ok", Exception = null };

            }
            else {
                return new ProcessResult<UserData>("GetUserByUserName") { Data = null, IsSucceeded = true, Status = "Ok", Exception = null,Message="user Name Not exist" };
            }
        }

        public ProcessResult<List<User>> GetUsersByGroupId(int groupId)
        {
            var data = _KhatmaContext.Users.Where(c => c.GroupId == groupId).ToList();
            return new ProcessResult<List<User>>("GetUsersByGroupId") { Data = data, IsSucceeded = true, Status = "Ok", Exception = null };
        }

        public bool IsUserNameExist(string userName)
        {
            return _KhatmaContext.Users.Any(c => c.UserName == userName);
        }

        public ProcessResult<LoginResponseViewModel> Login(LoginViewModel Loginuser)
        {
            var user = _KhatmaContext.Users.Where(c => c.UserName == Loginuser.UserName && c.Password == Loginuser.Password).ToList().FirstOrDefault();
            if (user != null)
            {
                if (!String.IsNullOrEmpty(Loginuser.DeviceToken))
                {
                    var userDevice = _KhatmaContext.userDevices.Where(c => c.UserID == user.Id).ToList().LastOrDefault();
                    if (userDevice != null)
                    {
                        userDevice.DeviceToken = Loginuser.DeviceToken;
                        _KhatmaContext.userDevices.Update(userDevice);
                    }
                    else
                    { 
                    var newUserDevice=new UserDevice()
                    { 
                    UserID=user.Id,
                    DeviceToken=Loginuser.DeviceToken                  
                    
                    };
                  _KhatmaContext.userDevices.Add(newUserDevice);
                    }
                    Save();

                }
                var group = _KhatmaContext.UserGroups.Find(user.GroupId);
                var response = new LoginResponseViewModel();
                response.User_Group = _Mapper.Map<Group, UserGroup>(group);
                var Setting = _KhatmaContext.KhatmaSettings.ToList().Last();
                response.KhatmaCount = Setting.KhatmaCount;
                response.GroupId = user.GroupId;
                response.PageNo = user.PageNo;
                response.Password = user.Id.ToString();
                response.Role = user.Role;
                response.UserName = user.UserName;
                response.UserId = user.Id;
                response.UsersCount = _KhatmaContext.Users.Count();
                return new ProcessResult<LoginResponseViewModel>()
                {
                    Data = response,
                    IsSucceeded = true,
                    Status = "200"
                };
            }
            else
            {
                return new ProcessResult<LoginResponseViewModel>()
                {
                    Data = null,
                    IsSucceeded = false,
                    Status = "200",
                    Message = "Invalid User Name Or Password"
                };
            }
        }

        public void Save()
        {
            _KhatmaContext.SaveChanges();
        }

        public ProcessResult<bool> DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            if (user != null)
            {
                _KhatmaContext.Users.Remove(user);
                Save();
            var totlUserCount=_KhatmaContext.Users.Count();
                return new ProcessResult<bool>()
                {
                    Data = true,
                    IsSucceeded = true,
                    Status = "200",
                    TotalUserCount=totlUserCount
                };
            }
            else
            {
                return new ProcessResult<bool>()
                {
                    Data = false,
                    IsSucceeded = false,
                    Status = "400",
                    Message="User not found"
                };
            }
        }

        public ProcessResult<List<User>> GetUnReadingUsers()
        {
            var users = _KhatmaContext.Users.Where(c=>c.IsRead!=true).ToList();
            return new ProcessResult<List<User>>() { Data = users, IsSucceeded = true };
        }
        public List<string> GetUnReadingUsersDevicesToken()
        {
            var users = GetUnReadingUsers().Data;
            var userDrvicesData = _KhatmaContext.userDevices.ToList();
            List<string> userDevices = new List<string>();
            if (users!=null&&users.Count>0)
            {
                if (userDrvicesData?.Count>0)
                {
                    foreach (var item in users)
                    {
                        var userdevice = userDrvicesData.Where(c => c.UserID == item.Id).FirstOrDefault();
                        if (userdevice!=null)
                        {
                            userDevices.Add(userdevice.DeviceToken);
                        }
                    }
                }
            
            }
            return userDevices;
        }

        public ProcessResult<bool> Logout(string deviceId)
        {
            var userDevice = _KhatmaContext.userDevices.Where(c => c.DeviceToken == deviceId).ToList();
            _KhatmaContext.RemoveRange(userDevice);
            Save();
            return new ProcessResult<bool>()
            {
                Data = true,
                IsSucceeded = true,
                Status = "200",
            };
        }

        public ProcessResult<bool> AddUserDeviceToken(UserDevice userDevice)
        {
          //  UserDevice userDevice = new UserDevice() { DeviceToken = deviceId, UserID = UserId };
            _KhatmaContext.Add(userDevice);
            Save();
            return new ProcessResult<bool>()
            {
                Data = true,
                IsSucceeded = true,
                Status = "200",
            };
        }

    }
}
