using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models
{
    public class Deck_Sub
    {
        public Character character { get; set; }
        public List<EventCard>? eventCards { get; set; }
    }
}
