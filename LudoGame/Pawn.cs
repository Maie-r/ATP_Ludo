using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoGame
{
    class Pawn
    {
        public int team, id;
        public int pos = 0;

        public Pawn (int team, int id)
        {
            this.team = team;
            this.id = id;
        }

        public int Move(int amount)
        {
            pos+= amount;
            return pos;
        }

        public void Return()
        {
            pos = 0;
        }
    }
}
