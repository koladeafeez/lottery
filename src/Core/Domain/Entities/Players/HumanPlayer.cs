using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Players
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(decimal startingBalance, int id)
            : base(startingBalance, id)
        {
        }


        public override bool IsHuman => true;

        public override string GetPlayerType() => "Human";

    }
}
