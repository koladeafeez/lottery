
using Domain.Entities.Players;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Factories
{
    public interface IPlayerFactory
    {
        Player CreatePlayer(PlayerTypeEnum type, decimal startingBalance);
        List<Player> CreateMultiplePlayers(PlayerTypeEnum type, int count, decimal startingBalance);
        void ResetCounter(int value);
    }
}
