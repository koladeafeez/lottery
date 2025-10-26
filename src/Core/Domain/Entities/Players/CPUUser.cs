using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Players
{
    public class CPUUser : Player
    {
        public CPUUser(decimal startingBalance, int id)
            : base(startingBalance, id)
        {
        }

        public override bool IsHuman => false;

        public override string GetPlayerType() => "CPU";
    }
}
