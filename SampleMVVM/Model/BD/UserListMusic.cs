using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.Model.BD
{
    public class UserListMusic
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UserList")]
        public int UserListId { get; set; }
        public UserList UserList { get; set; }

        [ForeignKey("Songs")]
        public int SoungId { get; set; }
        public Songs Songs { get; set; } = null;
    }
}
