using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerPower
{
    struct TurretStruct
    {
        public string Name;
        public string Description;
        public int FireType; //0 = beam  1 = radius  2 = high beam  3 = high radius
        public int Range;
        public int Damage;
        public int NumberShots;
        public int Speed;
        public int TargetType;
        public int Price;
        public TurretStruct(string name, string description, int firetype, int range, int damage, int numbershots, int speed, int targettype, int price)
        {
            Name = name;
            Description = description;
            FireType = firetype;
            Range = range;
            Damage = damage;
            NumberShots = numbershots;
            Speed = speed;
            TargetType = targettype;
            Price = price;
        }
    };
    internal class Turret
    {
        public string Name;
        public string Description;
        public int FireType;
        public int Range;
        public float Damage;
        public int NumberShots;
        public int Speed;
        public int TargetType;
        public List<Image> Pictures;
        private int x, y;
        private int n;
        private readonly int maxn;
        private int s;
        public int price;

        public Turret(string name, string description, int fireType, int range, float damage, int numberShots, int speed, int targetType, List<Image> pictures)
        {
            Name = name;
            Description = description;
            FireType = fireType;
            Range = range;
            Damage = damage;
            NumberShots = numberShots;
            Speed = speed;
            TargetType = targetType;
            Pictures = pictures;
            n = 0;
            x = y = 0;
            s = 0;
            maxn = Pictures.Count;
        }
        public Turret(Turret t)
        {
            Name = t.Name;
            Description = t.Description;
            FireType = t.FireType;
            Range = t.Range;
            Damage = t.Damage;
            NumberShots = t.NumberShots;
            Speed = t.Speed;
            TargetType = t.TargetType;
            Pictures = t.Pictures;
            n = 0;
            x = y = 0;
            s = 0;
            maxn = Pictures.Count;
        }
        public void SetPrice(int price) => this.price = price;
        public void AddValue(int amount) => price += amount;
        public int Sell() => price / 2;
        public int GetCenterX() => x + 50;
        public int GetCenterY() => y + 50;
        public void SetPosition(int x, int y)
        {
            this.x = x / 100 * 100;
            this.y = y / 100 * 100;
        }
        public void SetPosition(Point p) => SetPosition(p.X, p.Y);
        public bool AtPosition(int x, int y) => ((this.x == x / 100 * 100) && (this.y == y / 100 * 100));
        public bool AtPosition(Point p) => AtPosition(p.X, p.Y);
        public float GetDistance(Creep c) => (float)Math.Sqrt((double)(Math.Pow(GetCenterX() - c.GetCenterX(), 2) + Math.Pow(GetCenterY() - c.GetCenterY(), 2)));
        public bool GetRangeCreep(Creep c) => (int)GetDistance(c) < Range;
        public float FireAtCreeps(List<Creep> creeps)
        {
            int save_s = s;
            float totaldamage = 0;
            foreach (Creep c in creeps)
            {
                s = save_s;
                int damage = (int)FireAtCreep(c);
                totaldamage += damage;
                //c.HP -= damage;
            }
            return totaldamage;
        }
        public float FireAtCreep(Creep c)
        {
            float dam = 0;
            if (GetRangeCreep(c))
            {
                if ((FireType == 1) || (FireType == 3)) s = 0;
                if (s == 0)
                    dam = Damage * NumberShots;
                if (FireType != 4) //the only armor piercing shots are firetype 4
                    dam /= c.Armor;
                s += Speed;
                if (s > 10) s = 0;
            }
            c.HP -= (int)dam;
            return dam;
        }
        public void PaintNoAnim(Graphics g, int x, int y)
        {
            this.x = x;
            this.y = y;
            PaintNoAnim(g);
        }
        public void PaintNoAnim(Graphics g)
        {
            PaintPicture(g, 0);
        }
        private void PaintPicture(Graphics g, int n)
        {
            Rectangle src = new(0, 0, 100, 100);
            Rectangle dst = new(x, y, 100, 100);
            g.DrawImage(Pictures[n], dst, src, GraphicsUnit.Pixel);
        }
        public void Paint(Graphics g, int x, int y)
        {
            this.x = x;
            this.y = y;
            Paint(g);
        }
        public void Paint(Graphics g)
        {
            PaintPicture(g, n);
            n++;
            if (n >= maxn) n = 0;
        }
    }
}
