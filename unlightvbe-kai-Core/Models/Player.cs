using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public Deck Deck { get; set; }
    }
}
