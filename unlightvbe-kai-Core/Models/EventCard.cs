﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models
{
    public class EventCard : Card
    {
        public EventCard() { }
        public EventCard(EventCard card) : base(card) { }
        public EventCard(Card card) : base(card) { }
    }
}