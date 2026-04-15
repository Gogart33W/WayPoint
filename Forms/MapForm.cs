using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WayPoint
{
    public partial class MapForm : Form
    {
        public string SelectedCountry { get; private set; }

        private float zoom = 1.4f;
        private float offsetX = 0f;
        private float offsetY = 50f;
        private bool isPanning = false;
        private Point lastMousePosition;

        private Image hdMapImage;
        private string mapFilePath;

        private const float BASE_MAP_W = 1000f;
        private const float BASE_MAP_H = 452f;

        private class CountryMarker
        {
            public string Name { get; set; }
            public float MapX { get; set; }
            public float MapY { get; set; }
            public bool IsHovered { get; set; }
        }

        private List<CountryMarker> markers = new List<CountryMarker>();

        public MapForm()
        {
            InitializeComponent();

            // Налаштування форми
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(8, 12, 18);
            this.DoubleBuffered = true;

            // Шлях до карти
            mapFilePath = Path.Combine(Application.StartupPath, "world_map_hd.png");
            LoadOrDownloadMapAsync();

            // Хардкодимо маркери країн
            markers.Add(new CountryMarker { Name = "Канада", MapX = 259f, MapY = 101f });
            markers.Add(new CountryMarker { Name = "США", MapX = 262f, MapY = 117f });
            markers.Add(new CountryMarker { Name = "Мексика", MapX = 189f, MapY = 156f });
            markers.Add(new CountryMarker { Name = "Бразилія", MapX = 335f, MapY = 274f });
            markers.Add(new CountryMarker { Name = "Британія", MapX = 470f, MapY = 85f });
            markers.Add(new CountryMarker { Name = "Франція", MapX = 477f, MapY = 96f });
            markers.Add(new CountryMarker { Name = "Німеччина", MapX = 506f, MapY = 87f });
            markers.Add(new CountryMarker { Name = "Україна", MapX = 556f, MapY = 89f });
            markers.Add(new CountryMarker { Name = "Італія", MapX = 504f, MapY = 109f });
            markers.Add(new CountryMarker { Name = "Єгипет", MapX = 556f, MapY = 144f });
            markers.Add(new CountryMarker { Name = "ПАР", MapX = 529f, MapY = 309f });
            markers.Add(new CountryMarker { Name = "Індія", MapX = 684f, MapY = 155f });
            markers.Add(new CountryMarker { Name = "Китай", MapX = 797f, MapY = 140f });
            markers.Add(new CountryMarker { Name = "Японія", MapX = 855f, MapY = 124f });
            markers.Add(new CountryMarker { Name = "Австралія", MapX = 874f, MapY = 310f });

            // Підписка на події миші
            this.MouseWheel += Map_MouseWheel;
            this.MouseDown += Map_MouseDown;
            this.MouseMove += Map_MouseMove;
            this.MouseUp += Map_MouseUp;
            this.MouseClick += Map_MouseClick;
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
        }

        private async void LoadOrDownloadMapAsync()
        {
            if (!File.Exists(mapFilePath))
            {
                try
                {
                    string url = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/World_map_blank_without_borders.svg/1000px-World_map_blank_without_borders.svg.png";
                    await Task.Run(() =>
                    {
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add("User-Agent", "Mozilla/5.0");
                            client.DownloadFile(url, mapFilePath);
                        }
                    });
                }
                catch { }
            }

            if (File.Exists(mapFilePath))
            {
                hdMapImage = Image.FromFile(mapFilePath);
                ClampCamera();
                this.Invalidate();
            }
        }

        private void Map_MouseClick(object sender, MouseEventArgs e)
        {
            // Клік по кнопці "X" (Закрити)
            if (e.X >= this.Width - 45 && e.X <= this.Width - 10 && e.Y >= 10 && e.Y <= 40)
            {
                this.Close();
                return;
            }

            // Клік по маркеру
            foreach (var m in markers)
            {
                if (m.IsHovered)
                {
                    SelectedCountry = m.Name;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
        }

        private void ClampCamera()
        {
            float mapH = BASE_MAP_H * zoom;
            float minY = this.Height - mapH;
            if (offsetY > 50) offsetY = 50;
            if (offsetY < minY) offsetY = minY;

            float mapW = BASE_MAP_W * zoom;
            offsetX = offsetX % mapW;
            if (offsetX > 0) offsetX -= mapW;
        }

        private void Map_MouseWheel(object sender, MouseEventArgs e)
        {
            float factor = e.Delta > 0 ? 1.1f : 0.9f;
            float newZoom = zoom * factor;
            if (newZoom < (this.Height - 50) / BASE_MAP_H) return;
            if (newZoom > 10f) return;

            offsetX = e.X - (e.X - offsetX) * (newZoom / zoom);
            offsetY = e.Y - (e.Y - offsetY) * (newZoom / zoom);
            zoom = newZoom;
            ClampCamera();
            this.Invalidate();
        }

        private void Map_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Y > 50)
            {
                isPanning = true;
                lastMousePosition = e.Location;
                this.Cursor = Cursors.Hand;
            }
        }

        private void Map_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                offsetX += (e.X - lastMousePosition.X);
                offsetY += (e.Y - lastMousePosition.Y);
                lastMousePosition = e.Location;
                ClampCamera();
                this.Invalidate();
            }

            float mapW = BASE_MAP_W * zoom;
            bool changed = false;
            foreach (var m in markers)
            {
                float sx = (m.MapX * zoom) + offsetX;
                float sy = (m.MapY * zoom) + offsetY;
                bool h = IsHit(e.Location, sx, sy) || IsHit(e.Location, sx + mapW, sy);
                if (m.IsHovered != h) { m.IsHovered = h; changed = true; }
            }
            if (changed) this.Invalidate();
        }

        private bool IsHit(Point m, float x, float y) => (m.X - x) * (m.X - x) + (m.Y - y) * (m.Y - y) < 225;

        private void Map_MouseUp(object sender, MouseEventArgs e)
        {
            isPanning = false;
            this.Cursor = Cursors.Default;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float mapW = BASE_MAP_W * zoom;
            float mapH = BASE_MAP_H * zoom;

            if (hdMapImage != null)
            {
                ImageAttributes ia = new ImageAttributes();
                ColorMatrix cm = new ColorMatrix(new float[][] {
                    new float[] { 0, 0, 0, 0, 0 },
                    new float[] { 0, 1, 0, 0, 0 },
                    new float[] { 0, 0, 1, 0, 0 },
                    new float[] { 0, 0, 0, 0.6f, 0 },
                    new float[] { 0, 0.4f, 0.7f, 0, 1 }
                });
                ia.SetColorMatrix(cm);
                g.DrawImage(hdMapImage, new Rectangle((int)offsetX, (int)offsetY, (int)mapW, (int)mapH), 0, 0, hdMapImage.Width, hdMapImage.Height, GraphicsUnit.Pixel, ia);
                g.DrawImage(hdMapImage, new Rectangle((int)(offsetX + mapW), (int)offsetY, (int)mapW, (int)mapH), 0, 0, hdMapImage.Width, hdMapImage.Height, GraphicsUnit.Pixel, ia);
            }

            Font f = new Font("Segoe UI", 9f, FontStyle.Bold);
            foreach (var m in markers)
            {
                float sx = (m.MapX * zoom) + offsetX;
                float sy = (m.MapY * zoom) + offsetY;
                DrawMarker(g, m, sx, sy, f);
                DrawMarker(g, m, sx + mapW, sy, f);
            }

            g.FillRectangle(new SolidBrush(Color.FromArgb(250, 8, 12, 18)), 0, 0, this.Width, 50);
            g.DrawLine(new Pen(Color.Cyan, 2), 0, 50, this.Width, 50);
            g.DrawString("СИСТЕМА ГЛОБАЛЬНОЇ НАВІГАЦІЇ", new Font("Consolas", 14, FontStyle.Bold), Brushes.Cyan, 20, 15);

            g.FillRectangle(Brushes.Crimson, this.Width - 45, 10, 35, 30);
            g.DrawString("X", f, Brushes.White, this.Width - 33, 16);
        }

        private void DrawMarker(Graphics g, CountryMarker m, float x, float y, Font font)
        {
            if (m.IsHovered)
            {
                g.FillEllipse(new SolidBrush(Color.FromArgb(120, 0, 255, 255)), x - 12, y - 12, 24, 24);
                g.FillEllipse(Brushes.White, x - 5, y - 5, 10, 10);
                g.DrawString(m.Name, font, Brushes.Cyan, x + 15, y - 7);
            }
            else
            {
                g.FillEllipse(Brushes.Cyan, x - 3, y - 3, 6, 6);
                g.DrawString(m.Name, font, new SolidBrush(Color.FromArgb(180, 0, 255, 255)), x + 8, y - 7);
            }
        }
    }
}