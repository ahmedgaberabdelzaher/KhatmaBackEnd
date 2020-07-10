using KhatmaBackEnd.DBContext;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.Managers.Interfaces;
using KhatmaBackEnd.Utilites;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Managers.Classes
{
    public class HangFireJobService : IHangFireJobService
    {
        IUserManager _userManager;
        KhatmaContext _khatmaContext;
        public HangFireJobService(IUserManager userManager,KhatmaContext khatmaContext)
        {
            _userManager = userManager;
            _khatmaContext = khatmaContext;
        }
        public Task<bool> UpdateKhatmaCountHangfire()
        {
            var users = _userManager.GetAll();
        var khatmaSetting = _khatmaContext.Settings.ToList();
            if (users.Data!=null)
            {
                var groupedUsers = users.Data.Where(c => c.Role != "super_admin").GroupBy(c => c.GroupId);
                int GroupIndex = 0;
                foreach (var group in groupedUsers)
                {
                   // var lastPage = users.Data.OrderBy(c=>c.PageNo).Last().PageNo;
                   var lastPage = khatmaSetting.Last().LastDistributedPage;
                    int PageCounter = 0;
                    for (int i = 1; i <= group.ToList().Count(); i++)
                {
                    if (lastPage + i <= 604)
                    {
                            if (PageCounter==0)
                            {
                            group.ToList()[i - 1].PageNo = lastPage + i;
                            }
                            else
                            {
                                PageCounter++;
                                group.ToList()[i - 1].PageNo =PageCounter;
                            }
                        }
                    else
                    {
                        group.ToList()[i - 1].PageNo = 1;
                        lastPage = 1;
                       PageCounter = 1;
                       var khatmaStting = _khatmaContext.Settings.ToList().LastOrDefault();
                       khatmaStting.KhatmaCount = khatmaStting.KhatmaCount+1;
                        }
                        group.ToList()[i - 1].IsRead = false;
                        var LastKhatmaSetting = _khatmaContext.Settings.ToList().Last();
                        LastKhatmaSetting.LastDistributedPage = group.ToList()[i - 1].PageNo;
                        _khatmaContext.Settings.Update(LastKhatmaSetting);

                }

                    if (group.Key==groupedUsers.Last().Key)
                    {
                    //    _khatmaContext.Update(new Setting() { })
                    }
                  //  GroupIndex++;
                }

                //for (int i = 1; i <= group.Count(); i++)
                //{
                //    if (lastPage + i < 604)
                //    {
                //        users.Data[i - 1].PageNo = lastPage + i;
                //    }
                //    else
                //    {
                //        users.Data[i - 1].PageNo = 1;
                //        lastPage = 1;
                //        var khatmaCount = _khatmaContext.Settings.ToList().LastOrDefault();
                //        if (khatmaCount != null)
                //        {
                //            _khatmaContext.Settings.Update(new Setting() { KhatmaCount = khatmaCount.KhatmaCount + 1 });

                //        }
                //        else
                //        {
                //            _khatmaContext.Settings.Add(new Setting() { KhatmaCount = 1 });

                //        }
                //    }
                //    //  _khatmaContext.Users.Update(user);

                //}

                _khatmaContext.Users.UpdateRange(users.Data);
                _khatmaContext.SaveChanges();
            }
            return Task.FromResult(true);
        }
    }
}
