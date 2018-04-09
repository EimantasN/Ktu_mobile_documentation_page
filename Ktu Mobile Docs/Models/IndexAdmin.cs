using KTU_Mobile_Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ktu_Mobile_Docs.Models
{
    public class IndexAdmin
    {
        public User UserID { get; set; }
        public List<log> latestinfo { get; set; }
        public List<User> members { get; set; }

        public class DisplayLog
        {
            public string User { get; set; }
            public string Desc { get; set; }
            public DateTime Created { get; set; }
        }
        public List<DisplayLog> GetLogToDispay()
        {
            latestinfo.Reverse();
            List<DisplayLog> data = new List<DisplayLog>();
            int i = 0;
            foreach (log info in latestinfo)
            {
                i++;
                string user = null;
                foreach (User a in members)
                {
                    if (a.Log.Any(x => x.Id == info.Id))
                    {
                        user = a.Name;
                        break;
                    }
                }
                var model = new DisplayLog
                {
                    User = user,
                    Desc = info.Description,
                    Created = info.Created
                };
                data.Add(model);
                if (i == 20) { break; }
            }
            return data;
        }
    }
}