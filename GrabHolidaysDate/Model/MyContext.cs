using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabHolidaysDate.Model
{
    public class MyContext : DbContext
    {
        public MyContext() : base("HoliDate") { }
        public DbSet<TBL_M_HOLIDAYS> Holiday { get; set; }
    }
}
