using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace WayPoint
{
    public partial class MapForm : Form
    {
        public string SelectedCity { get; private set; } = "";
        public string SelectedCountry { get; private set; } = "";

        private string initialQuery = "";
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private Panel pnlHeader;
        private Label lblTitle;
        private System.Windows.Forms.Button btnClose;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public MapForm(string query)
        {
            InitializeComponent();
            initialQuery = query;
            SetupUI();

            webView.CoreWebView2InitializationCompleted += WebView_InitializationCompleted;
            StartWebViewAsync();
        }

        private void MapForm_Load(object sender, EventArgs e)
        {
        }

        private void SetupUI()
        {
            this.Size = new Size(1100, 750);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;

            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(31, 41, 55) };

            lblTitle = new Label
            {
                Text = "⏳ Завантаження карти...",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(60, 15)
            };

            btnClose = new System.Windows.Forms.Button
            {
                Text = "← Повернутися",
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 65, 81),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 35),
                Location = new Point(940, 10),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            Label lblBackIcon = new Label { Text = "🔙", Font = new Font("Segoe UI Emoji", 14), ForeColor = Color.White, Location = new Point(15, 12), AutoSize = true, Cursor = Cursors.Hand };
            lblBackIcon.Click += (s, e) => this.Close();

            pnlHeader.MouseDown += (s, e) => { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; };
            pnlHeader.MouseMove += (s, e) => { if (dragging) { this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); } };
            pnlHeader.MouseUp += (s, e) => dragging = false;

            pnlHeader.Controls.Add(lblBackIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            this.Controls.Add(pnlHeader);

            webView = new Microsoft.Web.WebView2.WinForms.WebView2 { Dock = DockStyle.Fill };
            this.Controls.Add(webView);
            webView.BringToFront();
        }

        private async void StartWebViewAsync()
        {
            try
            {
                string userDataFolder = Path.Combine(Path.GetTempPath(), "WayPointMapCache");
                // Додаємо власне ім'я програми (User-Agent), щоб сервери нас поважали
                var envOptions = new CoreWebView2EnvironmentOptions("--disable-web-security --user-agent=\"WayPoint_CRM/1.0\"");
                var env = await CoreWebView2Environment.CreateAsync(null, userDataFolder, envOptions);

                await webView.EnsureCoreWebView2Async(env);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критична помилка браузера: " + ex.Message);
            }
        }

        private void WebView_InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                MessageBox.Show("Помилка завантаження WebView2: " + e.InitializationException?.Message);
                return;
            }

            lblTitle.Text = "🌍 Оберіть локацію (Клікніть по міссту на карті)";

            string[] htmlLines = new string[]
            {
                "<!DOCTYPE html>",
                "<html>",
                "<head>",
                "    <meta charset='utf-8' />",
                "    <link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.css' />",
                "    <script src='https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/leaflet.js'></script>",
                "    <style>body, html, #map { height: 100%; margin: 0; padding: 0; font-family: 'Segoe UI', sans-serif; }</style>",
                "</head>",
                "<body>",
                "    <div id='map'></div>",
                "    <script>",
                "        var map = L.map('map').setView([48.3794, 31.1656], 5); ",
                
                // ПРОБЛЕМА 2 ВИРІШЕНА: Сучасні тайли CartoDB Voyager без помилок 403
                "        L.tileLayer('https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', {",
                "            attribution: '&copy; OpenStreetMap &copy; CARTO'",
                "        }).addTo(map);",

                "        var marker;",
                "        var currentPolygon;", // Змінна для зберігання кордонів країни
                "        var apiParam = '&email=admin@waypoint.com';",
                "        var initialQuery = 'QUERY_PLACEHOLDER';",

                "        if(initialQuery && initialQuery.length > 2) {",
                // ПРОБЛЕМА 1 ВИРІШЕНА: Додано параметр polygon_geojson=1 для отримання меж
                "            fetch('https://nominatim.openstreetmap.org/search?format=json&q=' + encodeURIComponent(initialQuery) + '&polygon_geojson=1&limit=1' + apiParam)",
                "            .then(res => res.json())",
                "            .then(data => {",
                "                if(data.length > 0) {",
                "                    var place = data[0];",
                "                    var lat = place.lat; var lon = place.lon;",
                "                    if (place.geojson) {",
                // Малюємо красиву обводку кордонів країни
                "                        currentPolygon = L.geoJSON(place.geojson, {",
                "                            style: { color: '#3b82f6', weight: 2, fillOpacity: 0.1 }",
                "                        }).addTo(map);",
                // Автоматично наближаємо камеру так, щоб вся країна влізла в екран
                "                        map.fitBounds(currentPolygon.getBounds());",
                "                    } else {",
                "                        map.setView([lat, lon], 12);",
                "                    }",
                "                    marker = L.marker([lat, lon]).addTo(map);",
                "                }",
                "            });",
                "        }",

                "        map.on('click', function(e) {",
                "            if(marker) map.removeLayer(marker);",
                "            if(currentPolygon) map.removeLayer(currentPolygon);",
                "            marker = L.marker([e.latlng.lat, e.latlng.lng]).addTo(map);",

                "            fetch('https://nominatim.openstreetmap.org/reverse?format=json&lat=' + e.latlng.lat + '&lon=' + e.latlng.lng + '&zoom=18&addressdetails=1' + apiParam)",
                "            .then(res => res.json())",
                "            .then(data => {",
                "                var a = data.address;",
                "                var city = a.city || a.town || a.village || a.suburb || a.hamlet || a.municipality || 'Невідоме місто';",
                "                var country = a.country || '';",

                "                var cleanCity = city.replace(/'/g, \"\\\\'\");",
                "                var cleanCountry = country.replace(/'/g, \"\\\\'\");",

                "                var popupContent = `<div style='text-align:center;'>` +",
                "                                   `<b style='font-size:15px; color:#1f2937;'>${city}</b><br>` +",
                "                                   `<span style='color:#6b7280;'>${country}</span><br>` +",
                "                                   `<button onclick=\"sendToCSharp('${cleanCity}', '${cleanCountry}')\" ` +",
                "                                   `style='margin-top:12px; width:100%; padding:8px; background:#10b981; color:white; border:none; border-radius:4px; cursor:pointer; font-weight:bold;'>` +",
                "                                   `✅ Обрати локацію</button></div>`;",
                "                marker.bindPopup(popupContent).openPopup();",
                "            });",
                "        });",

                "        function sendToCSharp(city, country) {",
                "            window.chrome.webview.postMessage(city + '|' + country);",
                "        }",
                "    </script>",
                "</body>",
                "</html>"
            };

            string html = string.Join("\n", htmlLines);
            html = html.Replace("QUERY_PLACEHOLDER", initialQuery.Replace("'", "\\'"));

            webView.NavigateToString(html);

            webView.WebMessageReceived += (s, args) => {
                string[] res = args.TryGetWebMessageAsString().Split('|');
                if (res.Length == 2)
                {
                    SelectedCity = res[0];
                    SelectedCountry = res[1];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };
        }
    }
}