using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            

        }
    }
    delegate void ChoiceEventHandler(string choice);
    class Player
    {
        public event ChoiceEventHandler choiceEvent;
         int hp;
        public Player()
        {
            hp = 100;

        }
        public void  GetMassage(string choice)
        {
            choice = Console.ReadKey().ToString();
            choiceEvent(choice);
            
        }
        
    }
    class Enemy
    {
        int hp;
        Player player;
        public Enemy(Player player)
        {
            hp = 100;
            this.player = player;
            player .choiceEvent+=
        }
        public void Panduan(string pchoice, string echoice)
        {
            if (pchoice != echoice)
            {

            }
        }
        string  EnemyChoice()
        {

        }
    }
    
}
