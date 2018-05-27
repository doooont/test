using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg
{
    class Item
    {

    }
    
    class Equipment
    {
         struct Weapon
        {
          public string name { get; private set; }
          public float atk { get; private set; }
          public float speed { get; private set; }
        }
        struct Armor
        {
            public string name { get; private set; }
            public float def { get; private set; }
            public float speed { get; private set; }
            public float hp { get; private set; }
            public float mana { get; private set; }
        }
        struct Shoes
        {
            public string name { get; private set; }
            public float def { get; private set; }
            public float speed { get; private set; }
            public float hp { get; private set; }
            public float mana { get; private set; }
        }
        
        List<Weapon> weaponlist = new List<Weapon>();
        
        public void GetWeapon()
        {
            Weapon w1= new Weapon();
            w1.name = "木剑";
        }




    }
}
