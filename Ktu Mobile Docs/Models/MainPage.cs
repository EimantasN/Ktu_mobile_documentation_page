using KTU_Mobile_Data;
using System.Collections.Generic;

namespace Ktu_Mobile_Docs.Models
{
    public class MainPage
    {
        public string Git_hub_link { get; set; }
        public string Gmail { get; set; }
        public string Google_store_app { get; set; }

        public IEnumerable<User> Memebers { get; set; }
    }
}