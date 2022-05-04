using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace nSeed.Models
{
    public class DownloadedTorrent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
    }
}
