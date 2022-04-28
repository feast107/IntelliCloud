using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestAndroid
{
    public class Favourite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public bool IsFavour { get; set; }
    }
}
