using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserInterfaceAsync : IUserInterface
    {
        public Task ShowStartScreenAsync(ShowStartScreenModel data);
        public Task ShowBattleMessageAsync(string message);
        public Task DrawActionCardAsync(DrawActionCardModel data);
        public Task UpdateDataAsync(UpdateDataModel data);
    }
}
