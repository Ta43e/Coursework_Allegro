using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.Model.BD
{
    public class PlayList
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public Users User { get; set; }
        public string PlayList_Name { get; set; }
        public string ImagePath { get; set; }
        public string Author { get; set; }
    }
}
