using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;
using TowerPower.Properties;

namespace TowerPower
{
    public partial class Form1 : Form
    {
        bool sizeset = false;
        Size thissize = new();
        readonly Random rand = new();
        readonly System.Windows.Forms.Timer timer = new();
        readonly PictureBox pictureBox = new();
        readonly PictureBox infoBox = new();
        Bitmap screen = new(1, 1);
        Bitmap background = new(1, 1);
        Bitmap infobmp = new(1, 1);
        readonly Turret[] turrets = new Turret[6];
        readonly Creep[] creeps = new Creep[16];
        readonly List<Turret> turrets1 = new();
        readonly List<Creep>[] creeps1 = new List<Creep>[16];
        readonly int[] numcreeps = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] maxcreeps = { 50, 50, 50, 50, 40, 40, 40, 40, 50, 40, 25, 20, 50, 40, 20, 5 };
        readonly int[,] maximumcreeps = { {  50, 50, 50, 50, 40, 40, 40, 40,  0,  0,  0,  0,  0,  0,  0,  0 }, //0
                                          {  50, 50, 50, 50, 40, 40, 40, 40, 50, 40, 25, 20,  0,  0,  0,  0 }, //1
                                          {  50, 50, 50, 50, 40, 40, 40, 40, 50, 40, 25, 20, 25,  0,  0,  0 }, //2
                                          {  50, 50, 50, 50, 40, 40, 40, 40, 75, 65, 50, 25, 50, 40, 20,  5 }, //3
                                          {  50, 50, 50, 50, 40, 40, 40, 40,100, 95, 75, 50, 75, 65, 25, 10 }, //4
                                          {   0,  0,  0,  0, 10, 10, 10, 10,200,190,150,100,100, 75, 50, 15 }, //5
                                          {   0,  0,  0,  0,  0,  0,  0,  0,300,290,250,200,150,100, 75, 25 }, //6
                                          {   0,  0,  0,  0,  0,  0,  0,  0,400,390,350,300,250,200,150, 75 }, //7
        };
        bool ispaused = false;
        readonly int[] creepadd = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] creepaddcount = { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 5, 5, 5, 5 };
        int baseHP = 10000;
        readonly int maxbaseHP = 10000;
        int lastbasep = 0;
        int moneys = 100;
        int score = 0;
        int level = 0;
        readonly int maxlevel = 7;
        int wave = 0;
        Point mousepos = new();
        int seltower = 1;
        int interval = 8;
        readonly TurretStruct[] ts = { new("Four Gun", "Four guns, medium range, light damage", 0, 300, 10, 4, 1, 0, 100),
                                       new("Heater", "Heater, low range, medium damage", 1, 150, 50, 1, 1, 7, 200),
                                       new("Beamer", "Beamer, medium range, high damage", 2, 300, 100, 1, 1, 0, 500),
                                       new("Cannon", "Cannon, low range, very high damage, armor piercing shells", 4, 150, 250, 1, 1, 0, 2500),
                                       new("Beamer 2", "Beamer 2, high range, light damage", 3, 500, 10, 1, 1, 0, 1500),
                                       new("Orbitter", "Orbitter, medium range, high damage", 0, 300, 100, 2, 1, 0, 2000) };
        readonly CreepStruct[] cs = { new("Zombie Boy", "Slow, stupid, and weak", 100, 100, 1, 1, 10),
                                      new("Zombie Lad", "Still slow, stupid, and weak", 125, 125, 1, 1, 10),
                                      new("Zombie Teen", "Stupid and weak", 125, 125, 5, 1, 10),
                                      new("Zombie Tween", "Stupid and weak but lightly armored", 225, 225, 5, 2, 20),
                                      new("Zombie Adult", "Stupid and a little weak, lightly armored", 350, 350, 5, 2, 20),
                                      new("Zombie Man", "Still very stupid, but less weak, armored", 475, 475, 5, 5, 30),
                                      new("Zombie Woman", "Faster but weaker than Zombie Man", 450, 450, 10, 5, 30),
                                      new("Zombie Elder", "Much stronger", 500, 500, 10, 5, 40),
                                      new("Zombie Private", "Just a soldier in the zombie army", 1250, 1250, 10, 5, 50),
                                      new("Zombie Corporal", "A slightly stronger soldier in the zombie army", 1300, 1300, 10, 5, 50),
                                      new("Zombie Sergeant", "A faster, stronger soldier in the zombie army", 1350, 1350, 15, 5, 70),
                                      new("Zombie M Sergeant", "A stronger, faster, better armored soldier in the zombie army", 1400, 1400, 20, 7, 70),
                                      new("Zombie Leutenant", "Stronger, better armored officer in the zombie army", 1500, 1500, 20, 10, 80),
                                      new("Zombie Captain", "Even stronger officer in the zombie army", 1650, 1650, 20, 10, 80),
                                      new("Zombie Major", "Stronger, better armored officer in the zombie army", 1800, 1800, 20, 15, 90),
                                      new("Zombie General", "OMG!  Is this the army or it's general?", 5000, 5000, 25, 20, 100) };
        //in map and levels, 1 = dirt, 0 = grass
        readonly int[,] map = new int[10, 16];
        readonly int[,,] levels = new int[,,] { { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //0
                                                  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //1
                                                  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //2
                                                  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //3
                                                  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //4
                                                  { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //5
                                                  { 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //6
                                                  { 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                                                { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //7
                                                  { 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0 },
                                                  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
                                                  { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                  { 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
};

        public Form1()
        {
            InitializeComponent(); //windows requirement
        }
        public void LoadCreeps()
        {
            FindDirt(out int x, out int y); //find first dirt space
            for (int creep = 0; creep < creeps.Length; creep++) //for each creep
                creeps[creep] = new Creep(cs[creep].Name, cs[creep].Description, cs[creep].HP, cs[creep].MaxHP,
                    cs[creep].Speed, cs[creep].Armor, x, y, cs[creep].Value, LoadThisCreep(creep)); //set up that creep
            for (int c = 0; c < creeps1.Length; c++) //for rach creep type
            {
                creeps1[c] = new List<Creep>(); //make a new list
                creeps1[c].Clear(); //clear that list
            }
        }
        private void FindDirt(out int x, out int y)
        {
            x = 0; //start at upper left corner
            y = 0;
            for (int i = 0; i < 10; i++) //for each line vertically
                for (int j = 0; j < 16; j++) //for each line horizontally
                    if (CheckIt(i, j)) //if it is dirt
                    {
                        x = j * 100 + 38; //compute x value
                        y = i * 100 + 38; //compute y value
                        return;
                    }
        }
        public static List<Image>[] LoadThisCreep(int creep)
        {
            List<Image>[] creeps = new List<Image>[4]; //set up lists of images for each direction
            creeps[0] = LoadThisDirectionCreep(creep); //load the first direction
            for (int i = 0; i < 3; i++) //for each direction
                creeps[i + 1] = RotateBy90(creeps[i]); //compute the next direction by rotating 90 degrees
            return creeps; //return the lists of images
        }
        public static List<Image> RotateBy90(List<Image> creeps)
        {
            List<Image> images = new(); //list of images we will return
            for (int i = 0; i < 8; i++) //for each image (8 of them)
            {
                Bitmap bmp = new(creeps[i]); //make a new bitmap from an image
                bmp.RotateFlip(RotateFlipType.Rotate90FlipNone); //rotate it 90 degrees
                images.Add(bmp); //add it to the list
            }
            return images; //return the list
        }
        public static List<Image> LoadThisDirectionCreep(int creep)
        {
            List<Image> creepthisdir = new(); //list of images to return
            creepthisdir.Clear(); //clear this list
            for (int picno = 0; picno < 8; picno++) //for each picture (8 of them)
                creepthisdir.Add(LoadCreepPicture(creep, picno)); //add the loaded picture
            return creepthisdir; //return the list
        }
        public static Bitmap LoadCreepPicture(int creep, int picno)
        {
            Bitmap bmp = new(25, 25); //set up a bitmap for the picture
            using Graphics g = Graphics.FromImage(bmp); //using graphics object from bitmap
            {
                Rectangle src = new(picno * 25, creep * 25, 25, 25); //rectangle for source
                Rectangle dst = new(0, 0, 25, 25); //rectangle for destination
                g.DrawImage(Resources._25_16x8_creeps, dst, src, GraphicsUnit.Pixel); //draw the image
            }
            return bmp; //return the bitmap
        }
        public void LoadTowers()
        {
            for (int tower = 0; tower < turrets.Length; tower++) //for each turret
                turrets[tower] = new Turret(ts[tower].Name, ts[tower].Description, ts[tower].FireType, ts[tower].Range,
                    ts[tower].Damage, ts[tower].NumberShots, ts[tower].Speed, ts[tower].TargetType, LoadThisTower(tower)); //load it
        }
        public static List<Image> LoadThisTower(int tower)
        {
            List<Image> towers = new(); //list of images for the turret
            towers.Clear(); //clear the list
            for (int picno = 0; picno < 8; picno++) //for each picture (8)
                towers.Add(LoadTowerPicture(tower, picno)); //add the picture
            return towers; //return the list
        }
        public static Bitmap LoadTowerPicture(int tower, int picno)
        {
            Bitmap bmp = new(100, 100); //bitmap for the picture
            using Graphics g = Graphics.FromImage(bmp); //using graphics object from bitmap
            {
                Rectangle src = new(picno * 100, tower * 100, 100, 100); //rectangle for source
                Rectangle dst = new(0, 0, 100, 100); //rectangle for destination
                g.DrawImage(Resources._100_8x8_towers, dst, src, GraphicsUnit.Pixel); //draw the picture
            }
            return bmp; //return the picture
        }
        private void DrawMap()
        {
            LoadLevel(level); //load the level
            using Graphics g = Graphics.FromImage(background); //using the graphics object from the background image
            {
                for (int x = 0; x < background.Width; x += 100) //for each 100x100 tile horizontally
                    for (int y = 0; y < background.Height; y += 100) //for each 100x100 tile vertically
                        PaintIt(g, 0, x, y); //paint grass everywhere
                for (int i = 0; i < 10; i++) //for each vertical position
                    for (int j = 0; j < 16; j++) //for each horizontal position
                        PaintIt(g, map[i, j], j * 100, i * 100); //paint according to the map
                DrawBase(g); //draw the base
            }
        }
        private void DrawBase(Graphics g)
        {
            int p = 15 - ((baseHP * 16 - 1) / maxbaseHP); //compute the position of the base (for base being eaten with damage)
            if (p != lastbasep) //if base position is not the same as last position
            {
                SolidBrush b = new(Color.FromArgb(p * 16 + 15, Color.Red)); //set up a red brush with opacity by amount of damage
                g.FillRectangle(b, 0, 0, screen.Width, screen.Height); //color the screen with it
                lastbasep = p; //set last position to base position
            }
            Rectangle src = new(p * 100, 0, 100, 100); //rectangle for source
            Rectangle dst = new(1500, 900, 100, 100); //rectangle for destination
            g.DrawImage(Resources._100_16_brainbase, dst, src, GraphicsUnit.Pixel); //draw the base
        }
        private void FindGrassySpot(out int x, out int y)
        {
            x = rand.Next(pictureBox.Width - 100) / 100 * 100; //pick a horizontal spot
            y = rand.Next(pictureBox.Height - 100) / 100 * 100; //pick a vertical spot
            if (map[y / 100, x / 100] == 1) FindGrassySpot(out x, out y); //if it is not grass, do recursive check
        }
        private void LoadLevel(int level)
        {
            for (int i = 0; i < numcreeps.Length; i++) //for each creep
                numcreeps[i] = 0; //set number of them to zero
            for (int i = 0; i < 10; i++) //for each vertical position
                for (int j = 0; j < 16; j++) //for each horizontal position
                    map[i, j] = levels[level, i, j]; //copy the level data to map
        }
        private void OnFormLoad(object sender, EventArgs e)
        {
            Size = new(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); //set to desktop size
            Location = new Point(0, 0); //put in upper left corner of screen
            thissize = ClientSize; //save the client size
            pictureBox.Size = new Size(thissize.Width - 200, thissize.Height);
            pictureBox.Location = new Point(0, 0); //set location of picturebox
            pictureBox.BackColor = Color.Black; //set it to black (for space)
            pictureBox.MouseMove += HandleMouse; //set up handlers for mouse in picturebox
            pictureBox.MouseUp += HandleMouseUp;
            pictureBox.MouseWheel += HandleMouse;
            pictureBox.MouseDown += HandleMouseDown;
            Controls.Add(pictureBox); //add picturebox to form
            infoBox.Location = new Point(thissize.Width - 200, 0); //set up infobox next to picturebox
            infoBox.Size = new Size(200, thissize.Height); //set size
            infoBox.BackColor = Color.Black; //set background color to black
            infoBox.MouseDown += HandleInfoMouse; //set up handler for mouse in infobox
            Controls.Add(infoBox); //add infobox to form
            screen.Dispose(); //get rid of screen
            screen = new(thissize.Width - 200, thissize.Height); //make new screen, size of picturebox
            background.Dispose(); //get rid of background image
            background = new(screen.Width, screen.Height); //make new background image, size of screen image
            infobmp.Dispose(); //get rid of info bitmap
            infobmp = new Bitmap(200, thissize.Height); //set up new info bitmap, size of info box
            timer.Interval = 100; //every tenth of a second
            timer.Tick += Timer_Tick; //function to run
            StartGame(true); //start the game
            sizeset = true; //set flag to show we have set the size
            timer.Start(); //begin timer
        }
        private void HandleMouseDown(object? sender, MouseEventArgs e)
        {
            mousepos = e.Location; //save mouse position
            for (int i = 0; i < turrets1.Count; i++) //for each turret
            {
                if (turrets1[i].AtPosition(mousepos)) //if turret is at mouse position
                {
                    if (ChooseUpgrade(i)) //let user choose an upgrade and if selling
                        turrets1.RemoveAt(i); //get rid of turret
                    return; //and we are done here
                }
            }
            if (moneys >= ts[seltower - 1].Price) //if user has enough money
            {
                timer.Stop(); //stop the timer
                if (GetYesNo("Buy " + ts[seltower - 1].Description + " for $" + ts[seltower - 1].Price)) //if user agrees
                {
                    turrets1.Add(new Turret(turrets[seltower - 1])); //add the new turret
                    turrets1[^1].SetPosition(mousepos); //put it at the mouse position
                    turrets1[^1].SetPrice(ts[seltower - 1].Price); //set it's price
                    moneys -= ts[seltower - 1].Price; //and subtract the price from the user's money
                }
                DoPauseState(); //handle pause status
            }
        }
        private void HandleMouseUp(object? sender, MouseEventArgs e)
        {
            mousepos = e.Location; //save mouse position
        }
        private void HandleInfoMouse(object? sender, MouseEventArgs e)
        {
            int x = e.Location.X, y = e.Location.Y; //save the mouse position in horizontal and vertical
            bool showsel = false; //set a flag to say we are not showing the selection
            if (e.Button == MouseButtons.Left) //if left mouse button was pressed
            {
                if ((x >= 50) && (x <= 150)) //if in the correct horizontal position
                {
                    if ((y >= 10) && (y <= 110)) //if in the first vertical position
                    {
                        seltower = 1; //set tower selection to 1
                        showsel = true; //show selection
                    }
                    if ((y >= 120) && (y <= 220)) //if in the second vertical position
                    {
                        seltower = 2; //set tower selection to 2
                        showsel = true; //show selection
                    }
                    if ((y >= 230) && (y <= 330)) //if in the third vertical position
                    {
                        seltower = 3; //set tower selection to 3
                        showsel = true; //show selection
                    }
                    if ((y >= 340) && (y <= 440)) //if in the fourth vertical position
                    {
                        seltower = 4; //set tower selection to 4
                        showsel = true; //show selection
                    }
                    if ((y >= 450) && (y <= 550)) //if in the fifth vertical position
                    {
                        seltower = 5; //set tower selection to 5
                        showsel = true; //show selection
                    }
                    if ((y >= 560) && (y <= 660)) //if in the sixth vertical position
                    {
                        seltower = 6; //set tower selection to 6
                        showsel = true; //show selection
                    }
                }
            }
            if (showsel) ShowSelection(); //show selection to user
        }
        private void HandleMouse(object? sender, MouseEventArgs e)
        {
            bool showsel = false; //set show selection flag to false
            mousepos = e.Location; //get mouse position
            if (e.Delta > 0) //if mouse wheel this way
            {
                seltower++; //increase tower selection
                if (seltower > 6) seltower = 1; //loop tower selection on out of bounds
                showsel = true; //show selectino
            }
            if (e.Delta < 0) //if mouse wheel the other way
            {
                seltower--; //decrease tower selection
                if (seltower < 1) seltower = 6; //loop tower selection on out of bounds
                showsel = true; //show selection
            }
            if (showsel) ShowSelection(); //show selection to user
        }
        private void ShowSelection()
        {
            timer.Stop();  //stop timer
            MessageBox.Show(ts[seltower - 1].Description + " for $" + ts[seltower - 1].Price.ToString(), ts[seltower - 1].Name, 
                MessageBoxButtons.OK); //show selection info
            DoPauseState(); //handle pause status
        }
        private void StartGame(bool isnew)
        {
            wave = 0; //set wave to 0
            if (!isnew) //if this is not a new game
                foreach (Turret t in turrets1) //for each tower
                    moneys += t.Sell(); //sell it (half price)
            turrets1.Clear(); //clear turrets
            LoadTowers(); //load towers
            LoadCreeps(); //load creeps
            DrawMap(); //draw the map
            for (int c = 0; c < numcreeps.Length; c++) //for each creep type
            {
                numcreeps[c] = 0; //set the number of creep to 0
                maxcreeps[c] = maximumcreeps[level, c]; //set the maximum creeps
            }
            if (isnew) //if this is a new game
            {
                moneys = 100; //set money to 100
                score = 0; //set score to 0
            }
            baseHP = maxbaseHP; //set base HP to maximum
        }
        private void DoPauseState()
        {
            if (ispaused) timer.Stop(); //if paused, stop timer
            else timer.Start(); //otherwise, start timer back up
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Interval = (50 * interval) / (level + 2) + 1; //set timer interval
            if (baseHP < 0) //if base HP all gone
            {
                timer.Stop(); //stop the timer
                if (GetYesNo("You DIED!  Play again?")) //if user wants to play again
                    StartGame(true); //start the game over
                else //otherwise
                    Close(); //close the program
                return; //and we are done
            }
            for (int i = 0; i < creeps1.Length; i++) //for each creep type
                AddCreep(i); //add a creep
            using Graphics g = Graphics.FromImage(screen); //using graphics object from screen bitmap
            {
                g.DrawImage(background, 0, 0); //draw the background image (the playing field)
                for (int i = 0; i < turrets1.Count; i++) //for each turret
                {
                    if (CheckRange(g, turrets1[i])) turrets1[i].Paint(g); //if enemies in range, paint animated turret
                    else turrets1[i].PaintNoAnim(g); //otherwise, paint the static version
                }
                foreach (List<Creep> cr in creeps1) //for each creep type
                {
                    foreach (Creep c in cr) //for each creep in type
                    {
                        c.Paint(g); //paint creep
                        if (!c.InSpot(1538, 938)) //if creep has not reached base
                        {
                            c.Move(); //move creep
                            CheckPath(c); //check creep's path
                        }
                        else //otherwise
                        {
                            baseHP--; //take health from the base
                        }
                    }
                }
                DrawBase(g); //draw the base
            }
            pictureBox.Image = screen; //set picture box to use screen bitmap
            pictureBox.Invalidate(); //tell windows to repaint picture box
            PaintInfo(); //paint the info box
            for (int c = 0; c < creeps1.Length; c++) //for each creep type
            {
                for (int i = 0; i < creeps1[c].Count; i++) //for each creep in type
                {
                    if (creeps1[c][i].HP < 0) //if it has lost all its life
                    {
                        moneys += creeps1[c][i].Value; //add its value to money
                        score += creeps1[c][i].Value; //add its value to score
                        creeps1[c].RemoveAt(i); //get rid of that creep
                        i--; //fix counter for creeps
                    }
                }
            }
            int nc = 0; //set number creeps to 0
            for (int c = 0; c < creeps1.Length; c++) //for each creep type
            {
                if (creeps1[c].Count == 0) //if count of creeps is 0
                {
                    if (numcreeps[c] < maxcreeps[c]) //if all creeps haven't been added
                        nc++; //number creeps gets added to
                }
                else //otherwise (count of creeps not 0)
                    nc++; //number creeps gets added to
            }
            if (nc == 0) //if number creeps is still 0 (all creeps came out and got killed)
            {
                ispaused = true; //pause game
                timer.Stop(); //stop timer
                level++; //increase level
                if (level > maxlevel) level = maxlevel; //don't let level get beyond maximum
                StartGame(false); //start game (not new game)
                return; //we are done here
            }
            if (baseHP < 0) //if base is destroyed
            {
                using Graphics graph = Graphics.FromImage(screen); //using graphics object from screen bitmap
                    Util.DrawOutlinedString(graph, "You DIED!", "Comic Sans MS", 50, 0, 0, screen.Width, screen.Height, 
                        Color.Red, Color.Yellow, 5); //show user they died
                pictureBox.Image = screen; //set picture box to use screen bitmap
                pictureBox.Invalidate(); //tell windows to repaint the picture box
            }
        }
        private static bool GetYesNo(string name) => MessageBox.Show(name, "Yes?No?", MessageBoxButtons.YesNo) == DialogResult.Yes;
        private void PaintInfo()
        {
            Font font = new("Arial Black", 40, FontStyle.Bold); //set up font
            Point p = new(0, 110 * turrets.Length + 10); //set a point for after showing turrets
            using Graphics g = Graphics.FromImage(infobmp); //using graphics object from info bitmap
            {
                g.Clear(Color.Black); //clear bitmap to black
                for (int i = 0; i < turrets.Length; i++) //for each turret
                {
                    Rectangle dst = new(50, 110 * i + 10, 100, 100); //rectangle for destination
                    Rectangle src = new(0, 0, 100, 100); //rectangle for source
                    g.DrawImage(turrets[i].Pictures[0], dst, src, GraphicsUnit.Pixel); //draw the image
                    Util.DrawStringCentered(g, (i + 1).ToString(), dst.Left, dst.Top, dst.Width, dst.Height, false, font, Color.White); //put a number in it
                }
                p = Util.DrawBarGraph(g, "Base HP", p, new Size(200, 40), baseHP, maxbaseHP); //draw bar graph for base hit points
            }
            ImageText it = new(infobmp); //set up new image text
            font.Dispose(); //get rid of font
            font = new("Comic Sans MS", 16, FontStyle.Bold); //set up new font
            it.SetPosition(p); //set position
            it.SetFont(font); //set font
            it.SetForeColor(Color.White); //use white for foreground color
            it.SetBackColor(Color.Black); //use black for background color
            it.Draw("Base HP: " + baseHP.ToString()); //show base hit points
            it.CarriageReturn(); //new line
            it.Draw("Money: $" + moneys.ToString()); //show money
            it.CarriageReturn(); //new line
            it.Draw("Score: " + score.ToString()); //show score
            it.CarriageReturn(); //new line
            it.Draw("Level: " + (level + 1).ToString()); //show level
            it.CarriageReturn(); //new line
            it.Draw("Wave: " + (wave + 1).ToString()); //show wave
            PointF pf = it.GetPosition(); //get position
            SizeF sf = it.GetSize("Wave: " + (wave + 1).ToString()); //get size of wave string
            pf.X += it.GetSize("  ").Width; //add two spaces to width
            sf.Width = 200 - pf.X; //set width we can use
            int maxc = (int)sf.Width; //maximum is width
            int numc = 0; //number is 0
            int kilc = 0; //number killed is 0
            try //try the following
            {
                maxc = 100 * (int)sf.Width / 100; //grey
                numc = numcreeps[wave] * 100 / maxcreeps[wave] * (int)sf.Width / 100; //red
                kilc = (numcreeps[wave] - creeps1[wave].Count) * 100 / maxcreeps[wave] * (int)sf.Width / 100; //green
            }
            catch { } //catch any errors (divide by zero?)
            using Graphics gr = Graphics.FromImage(infobmp); //using graphics object from info bitmap
            {
                SolidBrush bb = new(Color.Gray); //gray brush
                SolidBrush rb = new(Color.Red); //red brush
                SolidBrush gb = new(Color.Green); //green brush
                gr.FillRectangle(bb, pf.X, pf.Y, maxc, sf.Height); //draw grey part
                gr.FillRectangle(rb, pf.X, pf.Y, numc, sf.Height); //draw red part
                gr.FillRectangle(gb, pf.X, pf.Y, kilc, sf.Height); //draw green part
            }
            it.CarriageReturn(); //new line
            it.SetFontSize(12); //change font size
            it.SetForeColor(Color.Yellow); //change text color to yellow
            it.Draw(creeps[wave].Name + ": " + creeps[wave].Description); //show creep information
            it.CarriageReturn(); //new line
            infoBox.Image = infobmp; //set info box to use info bitmap
            infoBox.Invalidate(); //tell windows to repaint info box
        }
        private void AddCreep(int c)
        {
            if ((c == 0) || (numcreeps[c - 1] >= maxcreeps[c - 1] - 1)) //if first type or all of last type have come out
            {
                if (numcreeps[c] < maxcreeps[c]) //if there are more to come out
                    wave = c; //set wave number
                if (creepadd[c] == 0) //if creep add is 0 (ready to add)
                {
                    if (numcreeps[c] < maxcreeps[c]) //if there are more to come out
                        creeps1[c].Add(new Creep(creeps[c])); //add new creep
                    numcreeps[c]++; //add to number of creeps done
                }
                creepadd[c]++; //increase creep add
                if (creepadd[c] > creepaddcount[c]) creepadd[c] = 0; //if creep add ready, set to 0
            }
        }
        private bool CheckRange(Graphics g, Turret t)
        {
            List<Creep> creepsinrange = new(); //make list for creeps in range
            for (int i = 0; i < creeps1.Length; i++) //for each creep type
                foreach (Creep c in creeps1[i]) //for each creep in type
                    if (t.GetRangeCreep(c)) //if creep is in range
                        creepsinrange.Add(c); //add creep to list of creeps in range
            if (creepsinrange.Count == 0) return false; //if no creeps in range, return false
            if (t.TargetType == 0) FireAtCreep(g, t, creepsinrange[0]); //if target type is 0, go ahead and shoot first creep
            else if (t.TargetType == 1) FireAtCreep(g, t, ClosestCreep(creepsinrange, t)); //if target type is 1, shoot closest creep
            else if (t.TargetType == 2) FireAtCreep(g, t, FurthestCreep(creepsinrange, t)); //if target type is 2, shoot furthest creep
            else if (t.TargetType == 3) FireAtCreep(g, t, WeakestCreep(creepsinrange)); //if target type is 3, shoot weakest creep
            else if (t.TargetType == 4) FireAtCreep(g, t, StrongestCreep(creepsinrange)); //if target type is 4, shoot strongest creep
            else if (t.TargetType == 5) FireAtCreep(g, t, LeastArmoredCreep(creepsinrange)); //if target type is 5, shoot least armored creep
            else if (t.TargetType == 6) FireAtCreep(g, t, MostArmoredCreep(creepsinrange)); //if target type is 6, shoot most armored creep
            else if (t.TargetType == 7) FireAtCreeps(g, t, creepsinrange); //if target type is 7 (wave type shot), shoot all creeps in range
                return true; //return true (creep was in range)
        }

        private static Creep ClosestCreep(List<Creep> creeps, Turret t)
        {
            List<float> dist = new(); //make list of distances
            foreach (Creep c in creeps) //for each creep
                dist.Add(t.GetDistance(c)); //add distance for that creep
            float mindist = dist.Min(); //get minimum distance
            int i = dist.IndexOf(mindist); //get index of minimum distance
            return creeps[i]; //return corresponding creep
        }

        private static Creep FurthestCreep(List<Creep> creeps, Turret t)
        {
            List<float> dist = new(); //make list of distances
            foreach (Creep c in creeps) //for each creep
                dist.Add(t.GetDistance(c)); //add distance for that creep
            float maxdist = dist.Max(); //get maximum distance
            int i = dist.IndexOf(maxdist); //get index of maximum distance
            return creeps[i]; //return corresponding creep
        }

        private static Creep WeakestCreep(List<Creep> creeps)
        {
            List<int> hps = new(); //make list of hit points
            foreach (Creep c in creeps) //for each creep
                hps.Add(c.HP); //add hit points for that creep
            int minhps = hps.Min(); //get minimum hit points
            int i = hps.IndexOf(minhps); //get index of minimum hit points
            return creeps[i]; //return corresponding creep
        }

        private static Creep StrongestCreep(List<Creep> creeps)
        {
            List<int> hps = new(); //make list of hit points
            foreach (Creep c in creeps) //for each creep
                hps.Add(c.HP); //add hit points for that creep
            int maxhps = hps.Max(); //get maximum hit points
            int i = hps.IndexOf(maxhps); //get index of maximum hit points
            return creeps[i]; //return corresponding creep
        }

        private static Creep LeastArmoredCreep(List<Creep> creeps)
        {
            List<int> acs = new(); //make list of armors
            foreach (Creep c in creeps) //for each creep
                acs.Add(c.Armor); //add armor for that creep
            int minacs = acs.Min(); //get minimum armor
            int i = acs.IndexOf(minacs); //get index of minimum armor
            return creeps[i]; //return corresponding creep
        }

        private static Creep MostArmoredCreep(List<Creep> creeps)
        {
            List<int> acs = new(); //make list of armors
            foreach (Creep c in creeps) //for each creep
                acs.Add(c.Armor); //add armor for that creep
            int maxacs = acs.Max(); //get maximum armor
            int i = acs.IndexOf(maxacs); //get index of maximum armor
            return creeps[i]; //return corresponding creep
        }

        private static float FireAtCreep(Graphics g, Turret t, Creep c)
        {
            float d = t.FireAtCreep(c); //get damage from firing at creep
            if (d > 0) //if damage is more than none
            {
                Pen p = MakePenByDamage(t.Damage); //make a pen by damage
                int r = (int)Math.Sqrt(Math.Pow((t.GetCenterX() - c.GetCenterX()), 2) + Math.Pow(t.GetCenterY() - c.GetCenterY(), 2)); //radius
                if ((t.FireType == 0) || (t.FireType == 2) || (t.FireType == 4)) //if regular shot
                    g.DrawLine(p, t.GetCenterX(), t.GetCenterY(), c.GetCenterX(), c.GetCenterY()); //make a line
                else if ((t.FireType == 1) || (t.FireType == 3)) //if wave type shot
                    g.DrawEllipse(p, t.GetCenterX() - r, t.GetCenterY() - r, 2 * r, 2 * r); //make a circle
                //no other fire types at the moment
            }
            return d; //return damage
        }

        private static void FireAtCreeps(Graphics g, Turret t, List<Creep> creeps)
        {
            float d = t.FireAtCreeps(creeps); //get total damage from firing at creeps
            if (d > 0) //if damage is more than none
            {
                SolidBrush b = MakeBrushByDamage(t.Damage, creeps.Count); //make a brush by damage
                int r = (int)t.GetDistance(FurthestCreep(creeps, t)); //get distance of furthest creep
                g.FillEllipse(b, t.GetCenterX() - r, t.GetCenterY() - r, 2 * r, 2 * r); //draw filled circle
            }
        }

        private static SolidBrush MakeBrushByDamage(float dam, int numberhit)
        {
            Color c = Color.White; //set color to white
            int a = 200; //set alpha to 200 (fairly opaque)
            if (dam < 1400) c = Color.Yellow; //if damage is less than 1400, color is yellow
            if (dam < 500) c = Color.Orange; //if damage is less than 500, color is orange
            if (dam < 100) c = Color.Red; //if damage is less than 100, color is red
            if (numberhit < 5) a = 150; //if less than 5 zombies hit, alpha is 150 (somewhat opaque)
            if (numberhit < 4) a = 100; //if less than 4 zombies hit, alpha is 100 (not very opaque)
            if (numberhit < 3) a = 50; //if less than 3 zombies hit, alpha is 50 (mostly transparent)
            SolidBrush b = new(Color.FromArgb(a, c)); //make brush from alpha and color
            return b; //return brush
        }

        private static Pen MakePenByDamage(float dam)
        {
            Pen p = new(Color.White) //make a new pen in white
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Solid, //use solid line
                Width = 6 //very wide (6 pixels)
            };
            if (dam < 2800) //if damage below 2800
            {
                p.Width = 5; //quite wide (5 pixels)
                p.Color = Color.Yellow; //yellow
            }
            if (dam < 1400) //if damage below 1400
            {
                p.Width = 4; //wide (4 pixels)
                p.Color = Color.FromArgb(255,127,0); //yellow - orange
            }
            if (dam < 700) //if damage less than 700
            {
                p.Width = 3; //normal width (3 pixels)
                p.Color = Color.FromArgb(255, 63, 0); //orange
            }
            if (dam < 300) //if damage less than 300
            {
                p.Width = 3; //normal width (3 pixels)
                p.Color = Color.FromArgb(255, 31, 0); //orange - red
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash; //use dashed line
            }
            if (dam < 100) //if damage less than 100
            {
                p.Width = 2; //skinny width (2 pixels)
                p.Color = Color.Red; //red
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot; //use dotted line
            }
            return p; //return the pen
        }

        private void CheckPath(Creep c)
        {
            int x = c.x, y = c.y; //get horizontal and vertical position of creep
            int i = y / 100, j = x / 100; //compute horizontal and vertical tile positions
            if (c.direction == 0) //if rightward
            {
                if (x % 100 >= 38) //if at or past midpoint
                    if (!CheckIt(i, j + 1)) //if we can't go right
                        GoUpDown(i, j, c); //go up or down
            }
            else if (c.direction == 1) //if downward
            {
                if (y % 100 >= 38) //if at or past midpoint
                    if (!CheckIt(i + 1, j)) //if we can't go down
                        GoLeftRight(i, j, c); //go left or right
            }
            else if (c.direction == 2) //if leftward
            {
                if (x % 100 <= 38) //at or past midpoint
                    if (!CheckIt(i, j - 1)) //if we can't go left
                        GoUpDown(i, j, c); //go up or down
            }
            else if (c.direction == 3) //if upward
            {
                if (y % 100 <= 38) //if at or past midpoint
                    if (!CheckIt(i - 1, j)) //if we can't go up
                        GoLeftRight(i, j, c); //go left or right
            }
        }

        private bool CheckIt(int i, int j)
        {
            if (i < 0 || i > 9 || j < 0 || j > 15) return false; //if i or j out of bounds, return false
            return (levels[level, i, j] == 1); //otherwise return map spot
        }

        private void GoLeftRight(int i, int j, Creep c)
        {
            if (CheckIt(i, j + 1)) //if we can go right
                c.direction = 0; //go right
            else if (CheckIt(i, j - 1)) //otherwise if we can go left
                c.direction = 2; //go left
            else if (c.direction == 1) //otherwise if we came from top to bottom
                c.direction = 3; //go up
            else //otherwise
                c.direction = 1; //go down
        }

        private void GoUpDown(int i, int j, Creep c)
        {
            if (CheckIt(i + 1, j)) //if we can go down
                c.direction = 1; //go down
            else if (CheckIt(i - 1, j)) //otherwise if we can go up
                c.direction = 3; //go up
            else if (c.direction == 0) //otherwise if we came from left to right
                c.direction = 2; //go left
            else //otherwise
                c.direction = 0; //go right
        }

        private static void PaintIt(Graphics g, int i, int x, int y)
        {
            Rectangle dst = new(x, y, 100, 100); //rectangle for destination
            Rectangle src = new(0, 0, 100, 100); //rectangle for source
            if (i == 1) g.DrawImage(Resources._100dirt, dst, src, GraphicsUnit.Pixel); //if dirt, draw dirt
            else g.DrawImage(Resources._100grass, dst, src, GraphicsUnit.Pixel); //otherwise, draw grass
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (sizeset) //if we have set our size
                ClientSize = thissize; //deny size change
        }

        private void HandleKeyboard(object sender, KeyEventArgs e)
        {
            switch (e.KeyData) //according to what key was pressed
            {
                case Keys.Escape: //Esc key
                    timer.Stop(); //stop the timer
                    if (GetYesNo("Quit the game?  Are you sure?")) Close(); //if user is sure, close program
                    else DoPauseState(); //otherwise, handle pause status
                    break; //continue
                case Keys.Add: //numeric + key
                    if (interval > 1) interval /= 2; //if interval more than 1, half it
                    break; //continue
                case Keys.Subtract: //numeric - key
                    if (interval < 64) interval *= 2; //if interval less than 64, double it
                    break; //continue
                case Keys.I: //I key
                    if (interval == 0) interval = 8; //if interval is 0, set it to 8
                    else interval = 0; //otherwise, set it to 0
                    break; //continue
                case Keys.M: //M key
                    moneys = int.MaxValue / 5; //set money to extreme amount
                    break; //continue
                case Keys.P: //P key
                case Keys.Space: //Space key
                    ispaused = !ispaused; //toggle is paused flag
                    if (ispaused) timer.Stop(); //if we are now paused, stop the timer
                    else timer.Start(); //otherwise, start the timer
                    break; //continue
                case Keys.D1: //1 key
                    seltower = 1; //select turret 1
                    ShowSelection(); //show selection
                    break; //continue
                case Keys.D2: //2 key
                    seltower = 2; //select turret 2
                    ShowSelection(); //show selection
                    break; //continue
                case Keys.D3: //3 key
                    seltower = 3; //select turret 3
                    ShowSelection(); //show selection
                    break; //continue
                case Keys.D4: //4 key
                    seltower = 4; //select turret 4
                    ShowSelection(); //show selection
                    break; //continue
                case Keys.D5: //5 key
                    seltower = 5; //select turret 5
                    ShowSelection(); //show selection
                    break; //continue
                case Keys.D6: //6 key
                    seltower = 6; //select turret 6
                    ShowSelection(); //show selection
                    break; //continue
                default: //any other key
                    return; //we are done
            }
            e.Handled = true; //set flag to show we handled keyboard
        }

        readonly Form upgradeform = new();
        bool issold = false;
        int turretno;
        Label money = new();
        Label range = new();
        Label dam = new();
        Label num = new();
        Label speed = new();
        Label target = new();
        Button addrange = new();
        Button adddamage = new();
        Button addnumshots = new();
        Button addspeed = new();
        Button sellit = new();
        Button changetarget = new();
        Button done = new();

        readonly string[] targetlist = { "Fire at first", "Fire at closest", "Fire at furthest", "Fire at weakest", "Fire at strongest",
                                "Fire at least armored", "Fire at most armored", "Fire at everybody" };

        private bool ChooseUpgrade(int turretnumber)
        {
            timer.Stop(); //stop the timer
            Turret t = turrets1[turretnumber]; //make a copy of turret
            turretno = turretnumber; //save turret number
            upgradeform.Text = t.Name; //set form title to use turret name
            upgradeform.ClientSize = new(530, 495); //client size of form
            Label desc = new() //set up description label
            {
                Text = t.Description, //set text to description
                Size = new(510, 95), //set size of field
                Location = new(10, 10) //set position
            };
            money.Dispose(); //get rid of money label
            money = new() //make new money label
            {
                Text = "You have $" + moneys, //set text
                Location = new(10, 105), //set position
                Size = new(250, 35) //set size
            };
            range.Dispose(); //get rid of range label
            range = new() //make new range label
            {
                Text = "Range: " + t.Range, //set text
                Location = new(10, 170), //set position
                Size = new(250, 35) //set size
            };
            dam.Dispose(); //get rid of damage label
            dam = new() //make new damage label
            {
                Text = "Damage: " + t.Damage, //set text
                Location = new(10, 215), //set position
                Size = new(250, 35) //set size
            };
            num.Dispose(); //get rid of number shots label
            num = new() //make new number shots label
            {
                Text = "Number Shots: " + t.NumberShots, //set text
                Location = new(10, 260), //set position
                Size = new(250, 35) //set size
            };
            speed.Dispose(); //get rid of speed label
            speed = new() //make new speed label
            {
                Text = "Speed: " + t.Speed, //set text
                Location = new(10, 305), //set position
                Size = new(250, 35) //set size
            };
            Label price = new() //make price label
            {
                Text = "Value: " + t.price.ToString(), //set text
                Location = new(10, 350), //set position
                Size = new(250, 35) //set size
            };
            target.Dispose();
            target = new() //make new target label
            {
                Text = targetlist[t.TargetType], //set text
                Location = new(10, 405), //set position
                Size = new(250, 35) //set size
            };
            MakeTheButtons(t); //make the buttons
            upgradeform.Controls.Add(desc); //add desc label to form
            upgradeform.Controls.Add(money); //add money label to form
            upgradeform.Controls.Add(range); //add range label to form
            upgradeform.Controls.Add(dam); //add damage label to form
            upgradeform.Controls.Add(num); //add number shots label to form
            upgradeform.Controls.Add(speed); //add speed label to form
            upgradeform.Controls.Add(price); //add price label to form
            upgradeform.Controls.Add(target); //add target label to form
            upgradeform.ShowDialog(); //show form
            DoPauseState(); //handle pause status
            return issold; //return flag for if turret is sold
        }

        private void MakeTheButtons(Turret t)
        {
            //remove buttons:
            upgradeform.Controls.Remove(addrange);
            upgradeform.Controls.Remove(adddamage);
            upgradeform.Controls.Remove(addnumshots);
            upgradeform.Controls.Remove(addspeed);
            upgradeform.Controls.Remove(sellit);
            upgradeform.Controls.Remove(changetarget);
            upgradeform.Controls.Remove(done);
            //create new buttons:
            addrange = MakeButton(addrange, "$" + GetPrice(t.Range) + " Increase Range", new Point(270, 170), new Size(250, 35), AddRange);
            adddamage = MakeButton(adddamage, "$" + GetPrice((int)t.Damage) + " Increase Damage", new Point(270, 215), new Size(250, 35), AddDamage);
            addnumshots = MakeButton(addnumshots, "$" + GetPrice(t.NumberShots) + " Increase Number Shots", new Point(270, 260), new Size(250, 35), AddNumShots);
            addspeed = MakeButton(addspeed, "$" + GetPrice(t.Speed) + " Increase Speed", new Point(270, 305), new Size(250, 35), AddSpeed);
            sellit = MakeButton(sellit, "Sell It", new Point(270, 350), new Size(250, 35), SellIt);
            changetarget = MakeButton(changetarget, "Change Target Type", new Point(270, 405), new Size(250, 35), ChangeTarget);
            done = MakeButton(done, "Done", new Point(140, 450), new Size(250, 35), Done);
            //add buttons to form:
            upgradeform.Controls.Add(addrange);
            upgradeform.Controls.Add(adddamage);
            upgradeform.Controls.Add(addnumshots);
            upgradeform.Controls.Add(addspeed);
            upgradeform.Controls.Add(sellit);
            upgradeform.Controls.Add(changetarget);
            upgradeform.Controls.Add(done);
        }
        private static Button MakeButton(Button old, string text, Point location, Size size, EventHandler function)
        {
            old.Dispose(); //get rid of old button
            Button button = new() //make a new button
            {
                Text = text, //set text
                Location = location, //set position
                Size = size //set size
            };
            button.Click += function; //set function to run on click
            return button; //return the button
        }

        private void AddRange(object? sender, EventArgs e)
        {
            int val = GetPrice(turrets1[turretno].Range); //set price for range upgrade
            if (moneys < val) return; //if user doesn't have enough money, we are done
            if (IncreaseItYesNo("Range", val)) //if user agrees to upgrade
            {
                turrets1[turretno].Range *= 2; //upgrade range
                turrets1[turretno].AddValue(val); //add price to turret's value
                moneys -= val; //take price from user's money
                range.Text = "Range: " + turrets1[turretno].Range; //update range label text
                money.Text = "You have $" + moneys; //update money label text
                MakeTheButtons(turrets1[turretno]); //make new buttons
            }
        }
        private void AddDamage(object? sender, EventArgs e)
        {
            int val = GetPrice((int)turrets1[turretno].Damage); //set price for damage upgrade
            if (moneys < val) return; //if user doesn't have enough money, we are done
            if (IncreaseItYesNo("Damage", val)) //if user agrees to upgrade
            {
                turrets1[turretno].Damage *= 2; //upgrade damage
                turrets1[turretno].AddValue(val); //add price to turret's value
                moneys -= val; //take price from user's money
                dam.Text = "Damage: " + turrets1[turretno].Damage; //update damage label text
                money.Text = "You have $" + moneys; //update money label text
                MakeTheButtons(turrets1[turretno]); //make new buttons
            }
        }
        private void AddNumShots(object? sender, EventArgs e)
        {
            int val = GetPrice(turrets1[turretno].NumberShots); //set price for number shots upgrade
            if (moneys < val) return; //if user doesn't have enough money, we are done
            if (IncreaseItYesNo("Number Shots", val)) //if user agrees to upgrade
            {
                turrets1[turretno].NumberShots *= 2; //upgrade number shots
                turrets1[turretno].AddValue(val); //add price to turret's value
                moneys -= val; //take price from user's money
                num.Text = "Number Shots: " + turrets1[turretno].NumberShots; //update number shots label text
                money.Text = "You have $" + moneys; //update money label text
                MakeTheButtons(turrets1[turretno]); //make new buttons
            }
        }
        private void AddSpeed(object? sender, EventArgs e)
        {
            int val = GetPrice(turrets1[turretno].Speed); //set price for speed upgrade
            if (moneys < val) return; //if user doesn't have enough money, we are done
            if (IncreaseItYesNo("Speed", val)) //if user agrees to upgrade
            {
                turrets1[turretno].Speed *= 2; //upgrade speed
                turrets1[turretno].AddValue(val); //add price to turret's value
                moneys -= val; //take price from user's money
                speed.Text = "Speed: " + turrets1[turretno].Speed; //update speed label text
                money.Text = "You have $" + moneys; //update money label text
                MakeTheButtons(turrets1[turretno]); //make new buttons
            }
        }
        private void SellIt(object? sender, EventArgs e)
        {
            if (GetYesNo("Sell this turret for $" + turrets1[turretno].Sell() + "?")) //if user agrees to sell turret
            {
                moneys += turrets1[turretno].Sell(); //add sell price to user's money
                issold = true; //set flag to show we sold turret
                upgradeform.Close(); //close the dialog
            }
        }
        private void ChangeTarget(object? sender, EventArgs e)
        {
            if (turrets1[turretno].TargetType == 7) return; //if type is 7 (shoot everybody), we are done
            turrets1[turretno].TargetType++; //increase type
            if (turrets1[turretno].TargetType > 6) turrets1[turretno].TargetType = 0; //loop on out of bounds
            target.Text = targetlist[turrets1[turretno].TargetType]; //update target label text
            MakeTheButtons(turrets1[turretno]); //make new buttons
        }
        private void Done(object? sender, EventArgs e)
        {
            issold = false; //set flag to show we did NOT sell turret
            upgradeform.Close(); //close the dialog
        }
        private static int GetPrice(int amt) => 10 * amt;
        private static bool IncreaseItYesNo(string message, int amount) => GetYesNo("Increase " + message + " for $" + amount + "?");
    }
}