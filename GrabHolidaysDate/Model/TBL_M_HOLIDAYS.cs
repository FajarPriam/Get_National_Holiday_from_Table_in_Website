using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabHolidaysDate.Model
{
    public class TBL_M_HOLIDAYS
    {
        [Key, Column(Order = 2)]
        public string Tahun { get; set; }
        [Key, Column(Order = 0)]
        public DateTime Date_Holiday { get; set; }
        [Key, Column(Order = 1)]
        public string Keterangan { get; set; }
    }
}
