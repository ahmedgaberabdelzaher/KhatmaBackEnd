using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Utilites;
using KhatmaBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Interfaces
{
    public interface IUserManager
    {
        public ProcessResult<List<User>> GetUsersByGroupId(int groupId);   
        
        public ProcessResult<UserData> GetUserByUserName(string userName);
        public ProcessResult<List<User>> GetAll();
        public int GetAllUsersCount();
        public ProcessResult<UserData> AddNewUser(UserForAdd user);
        public ProcessResult<List<User>> GetUnReadingUsers();
        public List<string> GetUnReadingUsersDevicesToken();
        public ProcessResult<LoginResponseViewModel> Login(LoginViewModel user);
        public bool IsUserNameExist(string userName);
        public ProcessResult<bool> ChangeReadStatus(ChangeReadStatusViewModel changeReadStatusViewModel);
        public ProcessResult<bool> DeleteUser(int userId);
        public ProcessResult<bool> Logout(string deviceId);
        public ProcessResult<bool> AddUserDeviceToken(UserDevice userDevice);

        public void Save();
    }

}
