using System.Threading.Tasks;

namespace KTU_Mobile_Services
{
    public interface IData
    {
        Task<KTU_Mobile_Data.User> GetMember(string email);
        Task addlog(string desc, int? id, string email, string name);
    }
}