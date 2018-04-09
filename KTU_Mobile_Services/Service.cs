using KTU_Mobile_Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KTU_Mobile_Services
{
    public class Service : IData
    {
        private readonly Data _ctx;

        public Service(Data ctx)
        {
            _ctx = ctx;
        }

        public async Task<User> GetMember(string email)
        {
            return await _ctx.Members.SingleOrDefaultAsync(x => x.Gmail == email);
        }

        public async Task addlog(string desc, int? id, string email, string name)
        {
            User member = null;
            if (id != null)
            {
                 member = _ctx.Members.First(x => x.Id == id);
            }
            else if (email != null)
            {
                member = _ctx.Members.First(x => x.Gmail == email);
            }
            else if (name != null)
            {
                member = _ctx.Members.First(x => x.Name == name);
            }
            else { return; }

            if (member != null && desc.Length > 0)
            {
                var logModel = new log
                {
                    Created = DateTime.Now,
                    Description = desc
                };
                User updated = member;
                if (member.Log != null)
                {
                    List<log> a = member.Log.ToList();
                    a.Add(logModel);

                    updated.Log = a;
                }
                else
                {
                    List<log> a = new List<log>();
                    a.Add(logModel);
                    updated.Log = a;
                }
                _ctx.Entry(member).CurrentValues.SetValues(updated);
                await _ctx.SaveChangesAsync();
            }
        }

    }
}