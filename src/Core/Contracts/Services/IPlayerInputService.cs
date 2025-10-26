using Domain.Entities.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Services
{
    public interface IPlayerInputService
    {

        public HumanPlayer GetPlayer();
        public void SetPlayer(HumanPlayer player);

        //public void IncrementGameCount();

        void AddListener(IGameEventListener listener);
    }
}
