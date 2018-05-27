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
            Player player = new Player();
            Enemy enemy = new Enemy(player);
            cc: player.GetMassage();
            Console.WriteLine("player hp {0}    enemy hp {1}",player.hp,enemy.hp);
            if (player .hp<=0)
            {
                Console.WriteLine("you lose the game");
                Console.ReadKey();
                return;
                
            }
            if (enemy.hp<=0)
            {
                Console.WriteLine("you win the game");
                Console.ReadKey();
                return;
            }
            else
            {
                goto cc;
            }

        }
    }
    delegate void ChoiceEventHandler(string choice);
    class Player
    {
        public event ChoiceEventHandler choiceEvent;
        public int hp { get; set; }
        public Player()
        {
            hp = 100;

        }
        public void  GetMassage()
        {
            Console.WriteLine("please writedown jiandao or shitou or bu");
            string choice = Console.ReadLine().ToString();
            choiceEvent(choice);
            
        }
        
    }
    class Enemy
    {
        public int hp { get; set; }
        Player player;
        List <string > clist;
        public Enemy(Player player)
        {
            hp = 100;
            this.player = player;
            clist = new List<string>() { "shitou", "jiandao", "bu" };
            player.choiceEvent += Panduan;
        }
        public void Panduan(string pchoice)
        {
            Random random = new Random();
            string echoice= clist[random.Next(0, 3)];
            if (pchoice==echoice )
            {
                Console.WriteLine("pingju");
                return;
            }
            if (pchoice=="shitou"&&echoice=="jiandao")
            {
                Console.WriteLine("you win");
                hp -= 10;
                return;
            }
            if (pchoice=="shitou"&&echoice=="bu")
            {
                Console.WriteLine("you lose");
                player.hp -= 10;
                return;
            }
            if (pchoice =="jiandao"&&echoice=="shitou")
            {
                Console.WriteLine("you lose");
                player.hp -= 10;
                return;
            }
            if (pchoice =="jiandao"&&echoice =="bu")
            {
                Console.WriteLine("you win");
                hp -= 10;
                return;
            }
            if (pchoice=="bu"&&echoice=="shitou")
            {
                Console.WriteLine("you win");
                hp -= 10;
                return;
            }
            if (pchoice=="bu"&&echoice =="jiandao")
            {
                Console.WriteLine("you lose");
                player.hp -= 10;
                return;
            }
            else
            {
                Console.WriteLine("error");
                return;
            }
        }
        
    }
    
}
