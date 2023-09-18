using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleMVVM.Model.BD
{
    public class Songs
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string ImagePath { get; set; }
        public string SongsPath { get; set; }
        public string Duration { get; set; }
    }
}
