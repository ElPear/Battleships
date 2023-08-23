using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace Battleships
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ClientSize = new Size(40 * 20 + 50, 40 * 10 + 40);

        }
        public partial class Tile : Button
        {
            private bool occupied = false;
            private bool hit = false;
            public bool Occupied   // property
            {
                get { return occupied; }   // get method
                set { occupied = value; }  // set method
            }
            public bool Hit   // property
            {
                get { return hit; }   // get method
                set { hit = value; }  // set method
            }
        }

        int arenaSize = 10;
        int tileSize = 40;
        Tile[,] playerArena;
        Tile[,] enemyArena;

        string[] boats = { "hLong", "hMid", "hShort", "vLong", "vMid", "vShort", "T", "done" };
        int bIndex;
        private void startOrReset(object sender, EventArgs e)
        {
            bIndex = 0;

            try { foreach (Tile item in playerArena) { Controls.Remove(item); } } catch { }
            try { foreach (Tile item in enemyArena) { Controls.Remove(item); } } catch { }
            playerArena = new Tile[arenaSize, arenaSize];
            enemyArena = new Tile[arenaSize, arenaSize];

            for (int x = 0; x < arenaSize; x++)
            {
                for (int y = 0; y < arenaSize; y++)
                {
                    playerArena[x, y] = new Tile()
                    {
                        Name = $"Tile{x}-{y}",
                        Width = tileSize,
                        Height = tileSize,
                        Location = new Point(10 + tileSize * x, 10 + tileSize * y),
                        BackColor = Color.LightSkyBlue,
                        Occupied = false,
                        Hit = false



                    };
                    Controls.Add(playerArena[x, y]);
                    playerArena[x, y].Click += new EventHandler(this.place);
                    playerArena[x, y].MouseEnter += new EventHandler(this.preview);
                    playerArena[x, y].MouseLeave += new EventHandler(this.clear);
                }
            }
            for (int x = 0; x < arenaSize; x++)
            {
                for (int y = 0; y < arenaSize; y++)
                {
                    enemyArena[x, y] = new Tile()
                    {
                        Name = $"EnemyTile{x}-{y}",
                        Width = tileSize,
                        Height = tileSize,
                        Location = new Point(440 + tileSize * x, 10 + tileSize * y),
                        BackColor = Color.LightSkyBlue,
                        Occupied = false,
                        Hit = false

                    };
                    enemyArena[x, y].Click += new EventHandler(this.playerAttack);
                    Controls.Add(enemyArena[x, y]);
                }
            }

        }
        private void place(object sender, EventArgs e)
        {
            string shape = boats[bIndex];
            Tile btn = (Tile)sender;
            int btnX, btnY;
            string coords = btn.Name.Replace("Tile", "");
            btnX = int.Parse(coords.Split("-")[0]);
            btnY = int.Parse(coords.Split("-")[1]);

            boardInteract(shape, btnX, btnY, Color.Gray, false, playerArena);
            if (boats[bIndex] == "done")
            {
                bIndex = 0;
                while (boats[bIndex] != "done")
                {
                    AutoPlace();
                }
            }
        }
        private void preview(object sender, EventArgs e)
        {
            string shape = boats[bIndex];
            Tile btn = (Tile)sender;
            int btnX, btnY;
            string coords = btn.Name.Replace("Tile", "");
            btnX = int.Parse(coords.Split("-")[0]);
            btnY = int.Parse(coords.Split("-")[1]);

            boardInteract(shape, btnX, btnY, Color.LightGray, true, playerArena);
        }
        private void boardInteract(string shape, int btnX, int btnY, Color color, bool preview, Tile[,] arena)
        {
            try
            {
                switch (shape)
                {
                    case "hLong":
                        if (!arena[btnX - 1, btnY].Occupied && !arena[btnX, btnY].Occupied && !arena[btnX + 1, btnY].Occupied && !arena[btnX + 2, btnY].Occupied)
                        {
                            arena[btnX - 1, btnY].BackColor = color;
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX + 1, btnY].BackColor = color;
                            arena[btnX + 2, btnY].BackColor = color;


                            if (!preview)
                            {
                                arena[btnX - 1, btnY].Occupied = true;
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX + 1, btnY].Occupied = true;
                                arena[btnX + 2, btnY].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    case "hMid":
                        if (!arena[btnX - 1, btnY].Occupied && !arena[btnX, btnY].Occupied && !arena[btnX + 1, btnY].Occupied)
                        {
                            arena[btnX - 1, btnY].BackColor = color;
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX + 1, btnY].BackColor = color;

                            if (!preview)
                            {
                                arena[btnX - 1, btnY].Occupied = true;
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX + 1, btnY].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;

                    case "hShort":
                        if (!arena[btnX, btnY].Occupied && !arena[btnX + 1, btnY].Occupied)
                        {
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX + 1, btnY].BackColor = color;
                            if (!preview)
                            {
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX + 1, btnY].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    case "vLong":
                        if (!arena[btnX, btnY - 1].Occupied && !arena[btnX, btnY].Occupied && !arena[btnX, btnY + 1].Occupied && !arena[btnX, btnY + 2].Occupied)
                        {
                            arena[btnX, btnY - 1].BackColor = color;
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX, btnY + 1].BackColor = color;
                            arena[btnX, btnY + 2].BackColor = color;
                            if (!preview)
                            {
                                arena[btnX, btnY - 1].Occupied = true;
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX, btnY + 1].Occupied = true;
                                arena[btnX, btnY + 2].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    case "vMid":
                        if (!arena[btnX, btnY - 1].Occupied && !arena[btnX, btnY].Occupied && !arena[btnX, btnY + 1].Occupied)
                        {
                            arena[btnX, btnY - 1].BackColor = color;
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX, btnY + 1].BackColor = color;
                            if (!preview)
                            {
                                arena[btnX, btnY - 1].Occupied = true;
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX, btnY + 1].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    case "vShort":
                        if (!arena[btnX, btnY].Occupied && !arena[btnX, btnY - 1].Occupied)
                        {
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX, btnY - 1].BackColor = color;
                            if (!preview)
                            {
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX, btnY - 1].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    case "T":
                        if (!arena[btnX - 1, btnY].Occupied && !arena[btnX, btnY].Occupied && !arena[btnX + 1, btnY].Occupied && !arena[btnX, btnY + 1].Occupied)
                        {
                            arena[btnX - 1, btnY].BackColor = color;
                            arena[btnX, btnY].BackColor = color;
                            arena[btnX + 1, btnY].BackColor = color;
                            arena[btnX, btnY + 1].BackColor = color;
                            if (!preview)
                            {
                                arena[btnX - 1, btnY].Occupied = true;
                                arena[btnX, btnY].Occupied = true;
                                arena[btnX + 1, btnY].Occupied = true;
                                arena[btnX, btnY + 1].Occupied = true;
                                bIndex++;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        private void clear(object sender, EventArgs e)
        {
            foreach (Tile item in Controls.OfType<Tile>())
            {
                if (item.BackColor == Color.LightGray)
                { item.BackColor = Color.LightSkyBlue; }
            }
        }
        private bool gameOver()
        {
            bool isOver = true;
            foreach (Tile item in playerArena)
            {
                if (!item.Hit && item.Occupied)
                    isOver = false;
            }
            return isOver;

        }
        private bool gameWin()
        {
            bool isOver = true;
            foreach (Tile item in enemyArena)
            {
                if (!item.Hit && item.Occupied)
                    isOver = false;
            }
            return isOver;

        }
        private void enemyAttack()
        {
            Random rng = new Random();
            int xAttack, yAttack;
            xAttack = rng.Next(0, arenaSize);
            yAttack = rng.Next(0, arenaSize);

            if (boats[bIndex] == "done" && !gameOver())
            {
                if (playerArena[xAttack, yAttack].Occupied && !playerArena[xAttack, yAttack].Hit)
                {
                    playerArena[xAttack, yAttack].BackColor = Color.IndianRed;
                    playerArena[xAttack, yAttack].Hit = true;
                }
                else if (playerArena[xAttack, yAttack].Hit)
                {
                    enemyAttack();
                }
                else { playerArena[xAttack, yAttack].BackColor = Color.WhiteSmoke; }
            }
            if (gameOver())
            {
                MessageBox.Show("You Lose :(");
            }
        }
        private void playerAttack(object sender, EventArgs e)
        {
            Random rng = new Random();
            Tile btn = (Tile)sender;
            int btnX, btnY;
            string coords = btn.Name.Replace("EnemyTile", "");
            btnX = int.Parse(coords.Split("-")[0]);
            btnY = int.Parse(coords.Split("-")[1]);

            if (boats[bIndex] == "done" && !gameWin())
            {
                if (enemyArena[btnX, btnY].Occupied && !enemyArena[btnX, btnY].Hit)
                {
                    enemyArena[btnX, btnY].BackColor = Color.IndianRed;
                    enemyArena[btnX, btnY].Hit = true;
                    //MessageBox.Show("Hit");
                    enemyAttack();
                }
                else
                {
                    enemyArena[btnX, btnY].BackColor = Color.WhiteSmoke;
                    //MessageBox.Show("miss");
                    enemyAttack();
                }
                foreach (Tile item in enemyArena)
                    if (item.Hit)
                        item.Click -= new EventHandler(this.playerAttack);
            }
            if (gameWin())
            {
                MessageBox.Show("You win!!!");
            }

        }
        private void AutoPlace()
        {
            Random rng = new Random();
            int btnX, btnY;
            btnX = rng.Next(0, arenaSize);
            btnY = rng.Next(0, arenaSize);
            string shape = boats[bIndex];

            boardInteract(shape, btnX, btnY, Color.LightSkyBlue, false, enemyArena);
        }
    }
}