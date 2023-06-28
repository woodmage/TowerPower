using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerPower
{
    struct CreepStruct
    {
        public string Name;
        public string Description;
        public int HP;
        public int MaxHP;
        public int Speed;
        public int Armor;
        public int Value;
        //public int X, Y;

        public CreepStruct(string name, string description, int hp, int maxhp, int speed, int armor, int value)
        {
            Name = name;
            Description = description;
            HP = hp;
            MaxHP = maxhp;
            Speed = speed;
            Armor = armor;
            Value = value;
        }
    };

    internal class Creep
    {
        public string Name;
        public string Description;
        public int HP;
        public int MaxHP;
        public int Speed;
        public int Armor;
        public int Value;
        public int x, y;
        public List<Image>[] Pictures = new List<Image>[4];
        public int direction;
        private int n;
        private readonly int maxn;

        public Creep(string name, string description, int hp, int maxhp, int speed, int armor, int x, int y, int value, List<Image>[] pictures)
        {
            Name = name;
            Description = description;
            HP = hp;
            MaxHP = maxhp;
            Speed = speed;
            Armor = armor;
            this.x = x;
            this.y = y;
            Value = value;
            Pictures = pictures;
            direction = 0;
            n = 0;
            maxn = Pictures[0].Count;
        }
        public Creep(Creep c)
        {
            Name = c.Name;
            Description = c.Description;
            HP = c.HP;
            MaxHP = c.MaxHP;
            Speed = c.Speed;
            Armor = c.Armor;
            x = c.x;
            y = c.y;
            Value = c.Value;
            Pictures = c.Pictures;
            direction = c.direction;
            n = 0;
            maxn = Pictures[0].Count;
        }
        public bool InSpot(int x, int y) => ((Math.Abs(this.x - x) < 25) && (Math.Abs(this.y - y) < 25));
        public int GetCenterX() => x + 25 / 2;
        public int GetCenterY() => y + 25 / 2;
        public void Move()
        {
            if (direction == 0) x += Speed; //if rightward, move horizontally right
            if (direction == 1) y += Speed; //if downward, move vertically down
            if (direction == 2) x -= Speed; //if leftward, move horizontally left
            if (direction == 3) y -= Speed; //if upward, move vertically up
        }
        public void Paint(Graphics g, int x, int y)
        {
            this.x = x;
            this.y = y;
            Paint(g);
        }
        public void Paint(Graphics g)
        {
            Rectangle src = new(0, 0, 25, 25);
            Rectangle dst = new(x, y, 25, 25);
            g.DrawImage(Pictures[direction][n], dst, src, GraphicsUnit.Pixel);
            if (HP < MaxHP)
            {
                g.FillRectangle(Brushes.Red, x, y - 20, 25, 5);
                int p = HP * 25 / MaxHP;
                g.FillRectangle(Brushes.Green, x, y - 20, p, 5);
            }
            n++;
            if (n >= maxn) n = 0;
        }
    }
}
