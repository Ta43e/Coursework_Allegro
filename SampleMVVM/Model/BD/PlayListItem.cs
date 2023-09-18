using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SampleMVVM.Model.BD
{
    public class PlayListItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PlayList")]
        public int PlayListId { get; set; }
        public PlayList PlayList { get; set; }

        [ForeignKey("Songs")]
        public int SoungId { get; set; }
        public Songs Songs { get; set; }
    }
}
