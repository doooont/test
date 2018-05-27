using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
    class PlayerState
    {
        public float hpMax { get; set; } 
        public float HpCurrent
        {
            get { return hpCurrent; }
            set
            {
                if (value >=hpMax)
                {
                    hpCurrent = hpMax;
                }
                else
                {
                    hpCurrent = value;
                }
            }
        }

        private float hpCurrent;
        public float ATK { get; set; }
        public float DEF { get; set; }
        public float Mana { get; set; }
        public float Manacurrent
        {
            get { return manacurrent; }
            set
            {
                if (value >=Mana )
                {
                    manacurrent = Mana;
                }
                else
                {
                    manacurrent = value;
                }
            }
        }

        private float manacurrent;
        public float speed { get; set; }
        public PlayerState()
        {
            hpMax = 100;
            hpCurrent = 100;
            Mana = 50;
            Manacurrent = 50;
            ATK = 15;
            DEF = 10;
            speed = 8;
        }


    }
}
