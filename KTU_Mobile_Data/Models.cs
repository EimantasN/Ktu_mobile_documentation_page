using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace KTU_Mobile_Data
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pareigos { get; set; }
        public string Fb { get; set; }
        public string Git { get; set; }
        public string Gmail { get; set; }
        public string Instragram { get; set; }
        public string Image { get; set; }
        public IEnumerable<log> Log { get; set; }
    }

    public class Main_page
    {
        public int Id { get; set; }
        public string Git_hub_link { get; set; }
        public string Gmail { get; set; }
        public string Google_store_app { get; set; }
    }

    public class log
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Nr { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
    }
}