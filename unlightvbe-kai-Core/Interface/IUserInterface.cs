﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserInterface
    {
        public void ShowStartScreen(ShowStartScreenModel data);
        public void ShowBattleMessage(string message);
        public ReadActionModel ReadAction();
        public void DrawActionCard(DrawCardModel data);
        public void DrawEventCard(DrawCardModel data);
        public void UpdateData(UpdateDataModel data);
        public void ReadActionReceive(ReadActionReceiveModel data);
    }
}
