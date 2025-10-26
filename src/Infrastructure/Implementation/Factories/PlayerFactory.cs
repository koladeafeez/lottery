
using Contracts.Factories;
using Domain.Entities.Players;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        private static int _nextId = 1;
        private static readonly object _lock = new object();
        public Player? CreatePlayer(PlayerTypeEnum type, decimal startingBalance)
        {
            Player? player = null;
            lock (_lock)
            {
                switch(type)
                {
                    case PlayerTypeEnum.Human:
                        {
                            player = new HumanPlayer(startingBalance, _nextId);
                            break;
                        }
                    case PlayerTypeEnum.CPU:
                        {
                             player = new CPUUser(startingBalance, _nextId);
                            break;
                        }               
                    
                };
                _nextId++;
                return player;

            }
        }

        public List<Player> CreateMultiplePlayers(PlayerTypeEnum type, int count, decimal startingBalance)
        {
            var players = new List<Player>();
            for (int i = 0; i < count; i++)
            {
                players.Add(CreatePlayer(type, startingBalance));
            }
            return players;
        }


        public void ResetCounter(int value)
        {
            lock (_lock)
            {
                _nextId = value;
            }
        }
    }
}
