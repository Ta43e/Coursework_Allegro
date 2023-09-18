using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.Model.BD
{
    public class AdminList
    {
        public int Id { get; set; }
        public List<Songs> Songs { get; set;}
        public AdminList()
        {
            this.Songs = new List<Songs>();
        }
    }
}
