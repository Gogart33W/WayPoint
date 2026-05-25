using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using WayPoint.Services;

namespace WayPoint
{
    public partial class MainWork : Form
    {
        private DataTable travelTable;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private bool isUpdatingFromGrid = false;
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWork()
        {
            InitializeComponent();

            // Жорсткі ліміти калькулятора
            numMarketPrice.Maximum = 1000000m;
            numBudget.Maximum = 1000000m;
            numNights.Maximum = 365m;
            numAdults.Maximum = 100m;
            numNights.Minimum = 1m;
            numAdults.Minimum = 1m;

            // 🚨 ФІКС КРИТИЧНОЇ ПОМИЛКИ: Ініціалізуємо таблицю ДО будь-яких подій
            SetupDataTableStructure();

            BindEvents();
        }

        private void BindEvents()
        {
            txtCountry.TextChanged += (s, e) => AutoCalculateMarketPrice();
            txtCity.TextChanged += (s, e) => AutoCalculateMarketPrice();
            numAdults.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            numNights.ValueChanged += (s, e) => AutoCalculateMarketPrice();
            cmbBoard.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            cmbTransport.SelectedIndexChanged += (s, e) => AutoCalculateMarketPrice();
            numBudget.ValueChanged += (s, e) => AnalyzeBenefit();

            // Клік на поле конкурента — відкриває таблицю цін
            txtAgency.Click += (s, e) => OpenCompetitorPrices();
            txtAgency.Cursor = Cursors.Hand;

            // Клік на поле готелю — відкриває вибір готелю
            txtHotel.Click += (s, e) => OpenHotelPicker();
            txtHotel.Cursor = Cursors.Hand;
        }

        private void MainWork_Load(object sender, EventArgs e)
        {
            lblTitle.Text = $"WayPoint Business | {Session.Username}";
            btnAdminReturn.Visible = (Session.Role == "Admin");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation(
                "User-Agent", "WayPoint-CRM/1.0 (contact@waypoint.ua)");

            LoadUsersToCombo();
            LoadDataFromDatabase();
            AutoCalculateMarketPrice();
        }

        // ══════════════════════════════════════════
        // ПРАЙС-РУШІЙ
        // ══════════════════════════════════════════
        private void AutoCalculateMarketPrice()
        {
            if (isUpdatingFromGrid) return;
            try
            {
                string loc = (txtCountry.Text + " " + txtCity.Text).Trim().ToLower();

                decimal baseNight = 40m;
                if (loc.Contains("єгипет") || loc.Contains("египет")) baseNight = 45m;
                else if (loc.Contains("туреччина") || loc.Contains("турция")) baseNight = 60m;
                else if (loc.Contains("франція") || loc.Contains("італія") ||
                         loc.Contains("іспанія") || loc.Contains("париж")) baseNight = 100m;
                else if (loc.Contains("оае") || loc.Contains("дубай") ||
                         loc.Contains("мальдіви")) baseNight = 200m;
                else if (loc.Contains("україна") || loc.Contains("украина") ||
                         loc.Contains("київ") || loc.Contains("вінниця") ||
                         loc.Contains("львів") || loc.Contains("одеса") ||
                         loc.Contains("карпати")) baseNight = 20m;
                else if (loc.Contains("польща") || loc.Contains("варшава")) baseNight = 35m;

                decimal boardMult = 1.0m;
                string boardText = cmbBoard.Text ?? "";
                if (boardText.Contains("BB")) boardMult = 1.1m;
                else if (boardText.Contains("HB")) boardMult = 1.3m;
                else if (boardText.Contains("FB")) boardMult = 1.5m;
                else if (boardText.Contains("AI")) boardMult = 1.8m;

                decimal transportPrice = 0m;
                string transportText = cmbTransport.Text ?? "";
                if (transportText.Contains("Літак (Прямий)")) transportPrice = 250m;
                else if (transportText.Contains("Літак (Пересадка)")) transportPrice = 180m;
                else if (transportText.Contains("Автобус")) transportPrice = 70m;
                else if (transportText.Contains("Потяг")) transportPrice = 50m;

                if (baseNight == 20m)
                {
                    if (transportText.Contains("Літак")) transportPrice = 50m;
                    else if (transportText.Contains("Автобус")) transportPrice = 15m;
                    else if (transportText.Contains("Потяг")) transportPrice = 20m;
                }

                decimal nights = numNights.Value < 1 ? 1 : numNights.Value;
                decimal adults = numAdults.Value < 1 ? 1 : numAdults.Value;
                decimal totalCost = (baseNight * boardMult * nights * adults) + (transportPrice * adults);

                string[] agencies = { "Booking.com", "Join UP!", "Coral Travel", "TUI", "Oasis Travel" };
                int rawHash = (loc + nights + adults).GetHashCode();
                int safeHash = rawHash == int.MinValue ? 0 : Math.Abs(rawHash);
                txtAgency.Text = agencies[safeHash % agencies.Length];

                decimal margin = 1.0m + ((5m + (safeHash % 10)) / 100m);
                decimal finalMarketPrice = Math.Round(totalCost * margin);
                if (finalMarketPrice > numMarketPrice.Maximum) finalMarketPrice = numMarketPrice.Maximum;
                numMarketPrice.Value = finalMarketPrice;

                AnalyzeBenefit();
            }
            catch { }
        }

        private void AnalyzeBenefit()
        {
            if (isUpdatingFromGrid) return;
            decimal our = numBudget.Value;
            decimal market = numMarketPrice.Value;
            if (market > 0 && our > 0)
            {
                decimal diff = market - our;
                if (diff > 0)
                {
                    lblSidebarTitle.Text = $"Вигода клієнта: +${diff}";
                    lblSidebarTitle.ForeColor = Color.Green;
                }
                else
                {
                    lblSidebarTitle.Text = $"Дорожче за {txtAgency.Text} на ${Math.Abs(diff)}";
                    lblSidebarTitle.ForeColor = Color.Red;
                }
            }
            else
            {
                lblSidebarTitle.Text = "Деталі туру";
                lblSidebarTitle.ForeColor = Color.Black;
            }
        }

        // ══════════════════════════════════════════
        // ВИБІР ГОТЕЛЮ
        // ══════════════════════════════════════════
        private async void OpenHotelPicker()
        {
            string city = txtCity.Text.Trim();
            string country = txtCountry.Text.Trim();

            if (string.IsNullOrEmpty(city) && string.IsNullOrEmpty(country))
            {
                MessageBox.Show("Спочатку введіть місто або країну призначення.",
                    "Не вказано місце", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = string.IsNullOrEmpty(city) ? country : $"{city}, {country}";
            var locationInfo = await ValidateLocationAsync(query);

            if (locationInfo == null)
            {
                MessageBox.Show(
                    $"Не вдалося знайти готелі для \"{query}\".\n\n" +
                    "Можливі причини:\n" +
                    "• Занадто маленький населений пункт\n" +
                    "• Помилка в назві\n\n" +
                    "Спробуйте вказати назву найближчого великого міста.",
                    "Місце не знайдено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (locationInfo.Type == "village" || locationInfo.Type == "hamlet" ||
                locationInfo.Type == "locality" || locationInfo.Population < 5000)
            {
                var result = MessageBox.Show(
                    $"\"{query}\" — це невеликий населений пункт ({locationInfo.DisplayName}).\n\n" +
                    "Там може не бути готелів. Шукати все одно?\n" +
                    "(Можливо краще вказати найближче велике місто)",
                    "Маленький населений пункт",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            var hotels = await SearchHotelsAsync(locationInfo.Lat, locationInfo.Lon, query);

            if (hotels.Count == 0)
            {
                MessageBox.Show(
                    $"Готелі в районі \"{query}\" не знайдені в базі OpenStreetMap.\n\n" +
                    "Це не означає що їх немає — просто вони можуть не бути позначені на карті.\n" +
                    "Введіть назву готелю вручну або спробуйте інше місто.",
                    "Готелі не знайдені", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new HotelPickerForm(hotels, query, (int)numNights.Value, (int)numAdults.Value))
            {
                if (form.ShowDialog() == DialogResult.OK && form.SelectedHotel != null)
                {
                    txtHotel.Text = form.SelectedHotel.Name;
                    txtHotel.ForeColor = Color.Black;

                    if (form.SelectedHotel.PricePerNight > 0)
                    {
                        decimal hotelTotal = form.SelectedHotel.PricePerNight * numNights.Value * numAdults.Value;
                        decimal margin = 1.08m;
                        numMarketPrice.Value = Math.Min(Math.Round(hotelTotal * margin), numMarketPrice.Maximum);
                        AnalyzeBenefit();
                    }
                }
            }
        }

        private async Task<LocationInfo> ValidateLocationAsync(string query)
        {
            try
            {
                string url = $"https://nominatim.openstreetmap.org/search" +
                             $"?format=json&q={Uri.EscapeDataString(query)}" +
                             $"&addressdetails=1&extratags=1&limit=1";
                var response = await httpClient.GetStringAsync(url);
                var doc = JsonDocument.Parse(response);
                var root = doc.RootElement;
                if (root.GetArrayLength() == 0) return null;

                var item = root[0];
                string type = "";
                long population = 99999;

                if (item.TryGetProperty("type", out var typeProp)) type = typeProp.GetString() ?? "";

                if (item.TryGetProperty("extratags", out var extratags))
                {
                    if (extratags.TryGetProperty("population", out var pop))
                        long.TryParse(pop.GetString(), out population);
                }

                string displayName = item.GetProperty("display_name").GetString() ?? query;
                double lat = double.Parse(item.GetProperty("lat").GetString() ?? "0", System.Globalization.CultureInfo.InvariantCulture);
                double lon = double.Parse(item.GetProperty("lon").GetString() ?? "0", System.Globalization.CultureInfo.InvariantCulture);

                return new LocationInfo { DisplayName = displayName, Type = type, Population = population, Lat = lat, Lon = lon };
            }
            catch { return null; }
        }

        private async Task<List<HotelInfo>> SearchHotelsAsync(double lat, double lon, string cityName)
        {
            var hotels = new List<HotelInfo>();
            try
            {
                string overpassQuery = $@"[out:json][timeout:15];
(
  node[""tourism""=""hotel""][""name""](around:10000,{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lon.ToString(System.Globalization.CultureInfo.InvariantCulture)});
  way[""tourism""=""hotel""][""name""](around:10000,{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)},{lon.ToString(System.Globalization.CultureInfo.InvariantCulture)});
);
out body;";

                var content = new StringContent($"data={Uri.EscapeDataString(overpassQuery)}");
                var response = await httpClient.PostAsync("https://overpass-api.de/api/interpreter", content);
                var json = await response.Content.ReadAsStringAsync();
                var doc = JsonDocument.Parse(json);

                foreach (var element in doc.RootElement.GetProperty("elements").EnumerateArray())
                {
                    string name = "";
                    int stars = 0;
                    string address = cityName;

                    if (element.TryGetProperty("tags", out var tags))
                    {
                        if (tags.TryGetProperty("name", out var nameProp)) name = nameProp.GetString() ?? "";
                        if (string.IsNullOrEmpty(name)) continue;
                        if (tags.TryGetProperty("stars", out var starsProp)) int.TryParse(starsProp.GetString(), out stars);
                        if (tags.TryGetProperty("addr:street", out var street)) address = street.GetString() ?? cityName;
                    }

                    decimal basePrice = GetBasePriceForRegion(txtCountry.Text + " " + txtCity.Text);
                    decimal pricePerNight = stars > 0 ? basePrice * (0.5m + stars * 0.3m) : basePrice;

                    hotels.Add(new HotelInfo
                    {
                        Name = name,
                        Stars = stars,
                        PricePerNight = Math.Round(pricePerNight),
                        Address = address
                    });

                    if (hotels.Count >= 20) break;
                }
            }
            catch { }
            return hotels;
        }

        private decimal GetBasePriceForRegion(string loc)
        {
            loc = loc.ToLower();
            if (loc.Contains("єгипет")) return 45m;
            if (loc.Contains("туреччина")) return 60m;
            if (loc.Contains("франція") || loc.Contains("італія") || loc.Contains("іспанія")) return 100m;
            if (loc.Contains("оае") || loc.Contains("дубай")) return 200m;
            if (loc.Contains("україна") || loc.Contains("київ") || loc.Contains("львів") || loc.Contains("одеса")) return 25m;
            if (loc.Contains("польща")) return 35m;
            return 40m;
        }

        // ══════════════════════════════════════════
        // ЦІНИ КОНКУРЕНТІВ
        // ══════════════════════════════════════════
        private void OpenCompetitorPrices()
        {
            int currentTravelId = GetCurrentTravelId();
            using (var form = new CompetitorPricesForm(currentTravelId, numMarketPrice.Value, txtCountry.Text, txtCity.Text, (int)numNights.Value, (int)numAdults.Value))
            {
                form.ShowDialog();
            }
        }

        private int GetCurrentTravelId()
        {
            if (dgvData.SelectedRows.Count > 0)
            {
                var val = dgvData.SelectedRows[0].Cells["ID"].Value;
                if (val != null && val != DBNull.Value) return Convert.ToInt32(val);
            }
            return -1;
        }

        // ══════════════════════════════════════════
        // БД — ЗАВАНТАЖЕННЯ / ЗБЕРІГАННЯ
        // ══════════════════════════════════════════
        private void SetupDataTableStructure()
        {
            travelTable = new DataTable();
            travelTable.Columns.Add("ID", typeof(int));
            travelTable.Columns.Add("User", typeof(string));
            travelTable.Columns.Add("Departure", typeof(string));
            travelTable.Columns.Add("Country", typeof(string));
            travelTable.Columns.Add("City", typeof(string));
            travelTable.Columns.Add("Hotel", typeof(string));
            travelTable.Columns.Add("Transport", typeof(string));
            travelTable.Columns.Add("Adults", typeof(int));
            travelTable.Columns.Add("Nights", typeof(int));
            travelTable.Columns.Add("Budget", typeof(decimal));
            travelTable.Columns.Add("MarketPrice", typeof(decimal));
            travelTable.Columns.Add("Status", typeof(string));

            dgvData.DataSource = travelTable;
            if (dgvData.Columns.Contains("ID")) dgvData.Columns["ID"].Visible = false;

            dgvData.EnableHeadersVisualStyles = false;
            dgvData.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgvData.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvData.ReadOnly = true;
            dgvData.AllowUserToAddRows = false;
            dgvData.SelectionChanged += DgvData_SelectionChanged;
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                travelTable.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT * FROM Travels", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataRow row = travelTable.NewRow();
                            row["ID"] = reader["ID"];
                            row["User"] = reader["User"] == DBNull.Value ? "" : reader["User"];
                            row["Departure"] = reader["DepartureCity"] == DBNull.Value ? "Київ" : reader["DepartureCity"];
                            row["Country"] = reader["Country"] == DBNull.Value ? "" : reader["Country"];
                            row["City"] = reader["City"] == DBNull.Value ? "" : reader["City"];

                            // Захист від відсутності колонки або null
                            try { row["Hotel"] = reader["HotelName"] == DBNull.Value ? "" : reader["HotelName"]; } catch { row["Hotel"] = ""; }

                            row["Transport"] = reader["TransportType"] == DBNull.Value ? "Літак (Прямий)" : reader["TransportType"];

                            int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                            row["Adults"] = dbAdults < 1 ? 2 : dbAdults;

                            int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                            row["Nights"] = dbNights < 1 ? 7 : dbNights;

                            row["Budget"] = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                            row["MarketPrice"] = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                            row["Status"] = reader["Status"] == DBNull.Value ? "Запит" : reader["Status"];
                            travelTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Помилка завантаження БД: " + ex.Message); }
        }

        private void DgvData_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            isUpdatingFromGrid = true;
            try
            {
                int id = Convert.ToInt32(dgvData.SelectedRows[0].Cells["ID"].Value);
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT * FROM Travels WHERE ID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtCountry.Text = reader["Country"]?.ToString() ?? "";
                            txtCity.Text = reader["City"]?.ToString() ?? "";
                            txtDepartureCity.Text = reader["DepartureCity"]?.ToString() ?? "Київ";
                            cmbTransport.Text = reader["TransportType"]?.ToString() ?? "Літак (Прямий)";
                            cmbBoard.Text = reader["BoardType"]?.ToString() ?? "AI (Все вкл.)";
                            cmbStatus.Text = reader["Status"]?.ToString() ?? "Запит";
                            cmbAssignedUser.Text = reader["User"]?.ToString() ?? "";
                            txtAgency.Text = reader["AgencyName"]?.ToString() ?? "";

                            try { txtHotel.Text = reader["HotelName"]?.ToString() ?? ""; } catch { txtHotel.Text = ""; }
                            if (!string.IsNullOrEmpty(txtHotel.Text)) txtHotel.ForeColor = Color.Black;

                            decimal dbBudget = reader["Budget"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["Budget"]);
                            numBudget.Value = Math.Min(dbBudget, numBudget.Maximum);

                            decimal dbMarket = reader["MarketPrice"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["MarketPrice"]);
                            numMarketPrice.Value = Math.Min(dbMarket, numMarketPrice.Maximum);

                            int dbAdults = reader["Adults"] == DBNull.Value ? 2 : Convert.ToInt32(reader["Adults"]);
                            numAdults.Value = dbAdults < 1 ? 2 : Math.Min(dbAdults, 100);

                            int dbNights = reader["Nights"] == DBNull.Value ? 7 : Convert.ToInt32(reader["Nights"]);
                            numNights.Value = dbNights < 1 ? 7 : Math.Min(dbNights, 365);

                            lblSidebarTitle.Text = "Деталі туру";
                            lblSidebarTitle.ForeColor = Color.Black;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                isUpdatingFromGrid = false;
                AutoCalculateMarketPrice();
            }
        }

        private void LoadUsersToCombo()
        {
            try
            {
                cmbAssignedUser.Items.Clear();
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT Username FROM Users", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) cmbAssignedUser.Items.Add(reader["Username"].ToString());
                    }
                }
                if (cmbAssignedUser.Items.Count > 0) cmbAssignedUser.SelectedIndex = 0;
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"INSERT INTO Travels 
                        ([User],Country,City,DepartureCity,TransportType,Budget,MarketPrice,
                         AgencyName,[Status],Adults,Nights,BoardType,HotelName,Rating,Comment)
                        VALUES
                        (@u,@co,@ci,@dep,@tr,@b,@mp,@an,@s,@ad,@ni,@bt,@hn,1,'')";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@an", txtAgency.Text);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.Parameters.AddWithValue("@hn", txtHotel.Text == "Натисніть для вибору готелю..." ? "" : txtHotel.Text);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                btnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Помилка додавання: " + ex.Message); }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var sql = @"UPDATE Travels SET
                        [User]=@u,Country=@co,City=@ci,DepartureCity=@dep,TransportType=@tr,
                        Budget=@b,MarketPrice=@mp,AgencyName=@an,[Status]=@s,
                        Adults=@ad,Nights=@ni,BoardType=@bt,HotelName=@hn
                        WHERE ID=@id";
                    var cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", cmbAssignedUser.Text);
                    cmd.Parameters.AddWithValue("@co", txtCountry.Text);
                    cmd.Parameters.AddWithValue("@ci", txtCity.Text);
                    cmd.Parameters.AddWithValue("@dep", txtDepartureCity.Text);
                    cmd.Parameters.AddWithValue("@tr", cmbTransport.Text);
                    cmd.Parameters.AddWithValue("@b", numBudget.Value);
                    cmd.Parameters.AddWithValue("@mp", numMarketPrice.Value);
                    cmd.Parameters.AddWithValue("@an", txtAgency.Text);
                    cmd.Parameters.AddWithValue("@s", cmbStatus.Text);
                    cmd.Parameters.AddWithValue("@ad", (int)numAdults.Value);
                    cmd.Parameters.AddWithValue("@ni", (int)numNights.Value);
                    cmd.Parameters.AddWithValue("@bt", cmbBoard.Text);
                    cmd.Parameters.AddWithValue("@hn", txtHotel.Text == "Натисніть для вибору готелю..." ? "" : txtHotel.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                LoadDataFromDatabase();
            }
            catch (Exception ex) { MessageBox.Show("Помилка оновлення: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) return;
            int id = (int)dgvData.SelectedRows[0].Cells["ID"].Value;
            if (MessageBox.Show("Видалити тур?", "Підтвердження",
                MessageBoxButtons.YesNo) == DialogResult.No) return;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM Travels WHERE ID={id}", conn).ExecuteNonQuery();
                    new SqlCommand($"DELETE FROM CompetitorPrices WHERE TravelID={id}", conn).ExecuteNonQuery();
                }
                LoadDataFromDatabase();
                btnClear_Click(null, null);
            }
            catch (Exception ex) { MessageBox.Show("Помилка: " + ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            isUpdatingFromGrid = true;
            txtCountry.Clear(); txtCity.Clear();
            txtDepartureCity.Text = "Київ";

            txtHotel.Text = "Натисніть для вибору готелю...";
            txtHotel.ForeColor = Color.Gray;

            cmbTransport.SelectedIndex = 0;
            numBudget.Value = 0; numMarketPrice.Value = 0;
            txtAgency.Clear();
            numAdults.Value = 2; numNights.Value = 7;
            if (cmbBoard.Items.Count > 0) cmbBoard.SelectedIndex = 4;
            dgvData.ClearSelection();
            lblSidebarTitle.Text = "Деталі туру";
            lblSidebarTitle.ForeColor = Color.Black;
            isUpdatingFromGrid = false;
            AutoCalculateMarketPrice();
        }

        private void btnOpenMap_Click(object sender, EventArgs e)
        {
            using (var mf = new MapForm($"{txtCity.Text} {txtCountry.Text}"))
            {
                if (mf.ShowDialog() == DialogResult.OK)
                {
                    txtCity.Text = mf.SelectedCity;
                    txtCountry.Text = mf.SelectedCountry;
                }
            }
        }

        private void btnOpenFeed_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (var feed = new UserFeedForm()) { feed.ShowDialog(); }
            this.Show();
            LoadDataFromDatabase();
        }

        private void btnAdminReturn_Click(object sender, EventArgs e) => this.Close();
        private void pbBack_Click(object sender, EventArgs e) => this.Close();
        private void pbExit_Click(object sender, EventArgs e) => Application.Exit();

        private void pnlHeader_MouseDown(object sender, MouseEventArgs e)
        { dragging = true; dragCursorPoint = Cursor.Position; dragFormPoint = this.Location; }
        private void pnlHeader_MouseMove(object sender, MouseEventArgs e)
        { if (dragging) this.Location = Point.Add(dragFormPoint, new Size(Point.Subtract(Cursor.Position, new Size(dragCursorPoint)))); }
        private void pnlHeader_MouseUp(object sender, MouseEventArgs e) => dragging = false;
    }

    // ══════════════════════════════════════════
    // ДОПОМІЖНІ КЛАСИ
    // ══════════════════════════════════════════
    public class LocationInfo
    {
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public long Population { get; set; } = 99999;
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class HotelInfo
    {
        public string Name { get; set; }
        public int Stars { get; set; }
        public decimal PricePerNight { get; set; }
        public string Address { get; set; }
    }

    // ══════════════════════════════════════════
    // ФОРМА ВИБОРУ ГОТЕЛЮ
    // ══════════════════════════════════════════
    public class HotelPickerForm : Form
    {
        public HotelInfo SelectedHotel { get; private set; }
        private DataGridView dgv;
        private List<HotelInfo> hotels;

        public HotelPickerForm(List<HotelInfo> hotels, string cityName, int nights, int adults)
        {
            this.hotels = hotels;
            this.Text = $"Готелі — {cityName}";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            var lblHeader = new Label
            {
                Text = $"Знайдено {hotels.Count} готелів · {nights} ночей · {adults} дорослих",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            var lblHint = new Label
            {
                Text = "Клікніть двічі на готель щоб обрати його",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(15, 40),
                AutoSize = true
            };

            dgv = new DataGridView
            {
                Location = new Point(15, 65),
                Size = new Size(655, 340),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Готель", FillWeight = 40 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stars", HeaderText = "⭐", FillWeight = 10 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "PriceNight", HeaderText = "$/ніч", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "PriceTotal", HeaderText = $"$ за {nights} ніч.", FillWeight = 20 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", HeaderText = "Адреса", FillWeight = 35 });

            foreach (var h in hotels)
            {
                string stars = h.Stars > 0 ? new string('★', h.Stars) : "—";
                decimal total = h.PricePerNight * nights;
                dgv.Rows.Add(h.Name, stars,
                    h.PricePerNight > 0 ? $"${h.PricePerNight}" : "—",
                    h.PricePerNight > 0 ? $"${total}" : "—",
                    h.Address);
            }

            dgv.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.RowIndex >= hotels.Count) return;
                int stars = hotels[e.RowIndex].Stars;
                dgv.Rows[e.RowIndex].DefaultCellStyle.BackColor = stars >= 4
                    ? Color.FromArgb(240, 253, 244)
                    : stars == 3 ? Color.FromArgb(255, 251, 235) : Color.White;
            };

            dgv.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < hotels.Count)
                {
                    SelectedHotel = hotels[e.RowIndex];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            var btnSelect = new Button
            {
                Text = "✅ Обрати готель",
                Location = new Point(15, 420),
                Size = new Size(160, 38),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSelect.Click += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0)
                {
                    SelectedHotel = hotels[dgv.SelectedRows[0].Index];
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            var btnCancel = new Button
            {
                Text = "Скасувати",
                Location = new Point(185, 420),
                Size = new Size(120, 38),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnCancel.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };

            var btnManual = new Button
            {
                Text = "✏️ Ввести вручну",
                Location = new Point(315, 420),
                Size = new Size(150, 38),
                BackColor = Color.FromArgb(245, 158, 11),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            btnManual.Click += (s, e) =>
            {
                // Кастомне вікно без Visual Basic милиць
                Form prompt = new Form()
                {
                    Width = 400,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = "Введіть назву готелю",
                    StartPosition = FormStartPosition.CenterParent
                };
                TextBox inputBox = new TextBox() { Left = 20, Top = 20, Width = 340, Font = new Font("Segoe UI", 10) };
                Button btnOk = new Button() { Text = "ОК", Left = 260, Top = 60, Width = 100, DialogResult = DialogResult.OK };
                prompt.Controls.Add(inputBox); prompt.Controls.Add(btnOk);
                prompt.AcceptButton = btnOk;

                if (prompt.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(inputBox.Text))
                {
                    SelectedHotel = new HotelInfo { Name = inputBox.Text, Stars = 0, PricePerNight = 0 };
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            };

            this.Controls.AddRange(new Control[] { lblHeader, lblHint, dgv, btnSelect, btnCancel, btnManual });
        }
    }

    // ══════════════════════════════════════════
    // ФОРМА ЦІН КОНКУРЕНТІВ
    // ══════════════════════════════════════════
    public class CompetitorPricesForm : Form
    {
        private int travelId;
        private decimal ourPrice;
        private DataGridView dgv;
        private string country, city;
        private int nights, adults;

        private static readonly string[] AgencyNames =
        {
            "Booking.com", "Join UP!", "Coral Travel", "TUI", "Oasis Travel", "Anex Tour", "Mouzenidis"
        };

        public CompetitorPricesForm(int travelId, decimal ourPrice, string country, string city, int nights, int adults)
        {
            this.travelId = travelId; this.ourPrice = ourPrice;
            this.country = country; this.city = city;
            this.nights = nights; this.adults = adults;

            this.Text = "Порівняння цін конкурентів";
            this.Size = new Size(680, 520);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            BuildUI();
            LoadPrices();
        }

        private void BuildUI()
        {
            var lblHeader = new Label
            {
                Text = $"Ціни на тур: {city}, {country}  ·  {nights} ночей  ·  {adults} дорослих",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 15),
                AutoSize = true
            };

            var pnlOur = new Panel
            {
                Location = new Point(15, 45),
                Size = new Size(635, 40),
                BackColor = Color.FromArgb(220, 252, 231),
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlOur.Controls.Add(new Label
            {
                Text = $"💚  НАША ЦІНА:   ${ourPrice}   ({nights} ніч · {adults} дорослих)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(21, 128, 61),
                Location = new Point(10, 10),
                AutoSize = true
            });

            dgv = new DataGridView
            {
                Location = new Point(15, 95),
                Size = new Size(635, 300),
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EditMode = DataGridViewEditMode.EditOnEnter
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(243, 244, 246);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            var agencyCol = new DataGridViewComboBoxColumn
            { Name = "Agency", HeaderText = "Агентство", FillWeight = 25, FlatStyle = FlatStyle.Flat };
            foreach (var a in AgencyNames) agencyCol.Items.Add(a);
            dgv.Columns.Add(agencyCol);

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Ціна ($)", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = "Available", HeaderText = "Є в наявності", FillWeight = 18 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Link", HeaderText = "Посилання", FillWeight = 30 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Diff", HeaderText = "Різниця", FillWeight = 12, ReadOnly = true });

            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.CellEndEdit += Dgv_CellEndEdit;

            var btnAdd = new Button
            {
                Text = "+ Додати агентство",
                Location = new Point(15, 408),
                Size = new Size(170, 36),
                BackColor = Color.FromArgb(59, 130, 246),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnAdd.Click += (s, e) => dgv.Rows.Add(AgencyNames[0], "", true, "", "");

            var btnSave = new Button
            {
                Text = "💾 Зберегти",
                Location = new Point(200, 408),
                Size = new Size(130, 36),
                BackColor = Color.FromArgb(16, 185, 129),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            var btnDelete = new Button
            {
                Text = "🗑 Видалити рядок",
                Location = new Point(345, 408),
                Size = new Size(150, 36),
                BackColor = Color.FromArgb(239, 68, 68),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnDelete.Click += (s, e) => { if (dgv.SelectedRows.Count > 0) dgv.Rows.RemoveAt(dgv.SelectedRows[0].Index); };

            var btnClose = new Button
            {
                Text = "Закрити",
                Location = new Point(510, 408),
                Size = new Size(100, 36),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9)
            };
            btnClose.Click += (s, e) => this.Close();

            var lblHint = new Label
            {
                Text = "💡 Натисніть на комірку Price і введіть ціну. Різниця від нашої ціни рахується автоматично.",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.Gray,
                Location = new Point(15, 455),
                AutoSize = true
            };

            this.Controls.AddRange(new Control[] { lblHeader, pnlOur, dgv, btnAdd, btnSave, btnDelete, btnClose, lblHint });
        }

        private void Dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgv.Rows.Count) return;
            var row = dgv.Rows[e.RowIndex];
            if (decimal.TryParse(row.Cells["Price"].Value?.ToString(), out decimal price))
            {
                decimal diff = ourPrice - price;
                row.Cells["Diff"].Value = diff >= 0 ? $"-${diff} ✅" : $"+${Math.Abs(diff)} ❌";
            }
        }

        private void Dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != "Diff") return;
            string val = dgv.Rows[e.RowIndex].Cells["Diff"].Value?.ToString() ?? "";
            if (val.StartsWith("-") || val.Contains("✅")) e.CellStyle.ForeColor = Color.FromArgb(21, 128, 61);
            else if (val.StartsWith("+") || val.Contains("❌")) e.CellStyle.ForeColor = Color.FromArgb(185, 28, 28);
        }

        private void LoadPrices()
        {
            if (travelId < 0) return;
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    var cmd = new SqlCommand("SELECT * FROM CompetitorPrices WHERE TravelID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", travelId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            decimal price = reader["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["Price"]);
                            bool avail = reader["Available"] != DBNull.Value && Convert.ToBoolean(reader["Available"]);
                            string agency = reader["Agency"]?.ToString() ?? "";
                            string link = reader["Link"]?.ToString() ?? "";

                            decimal diff = ourPrice - price;
                            string diffStr = price > 0 ? (diff >= 0 ? $"-${diff} ✅" : $"+${Math.Abs(diff)} ❌") : "";
                            dgv.Rows.Add(agency, price > 0 ? price.ToString() : "", avail, link, diffStr);
                        }
                    }
                }
            }
            catch { }

            if (dgv.Rows.Count == 0)
            {
                foreach (var agency in AgencyNames) dgv.Rows.Add(agency, "", true, "", "");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (travelId < 0)
            {
                MessageBox.Show("Спочатку збережіть тур в базі даних (натисніть 'Додати').",
                    "Тур не збережено", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (var conn = DatabaseService.GetConnection())
                {
                    new SqlCommand($"DELETE FROM CompetitorPrices WHERE TravelID={travelId}", conn).ExecuteNonQuery();
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string agency = row.Cells["Agency"].Value?.ToString() ?? "";
                        if (string.IsNullOrWhiteSpace(agency)) continue;

                        decimal price = 0;
                        decimal.TryParse(row.Cells["Price"].Value?.ToString(), out price);
                        bool avail = row.Cells["Available"].Value != null && Convert.ToBoolean(row.Cells["Available"].Value);
                        string link = row.Cells["Link"].Value?.ToString() ?? "";

                        var cmd = new SqlCommand(
                            @"INSERT INTO CompetitorPrices (TravelID,Agency,Price,Available,Link)
                              VALUES (@tid,@ag,@pr,@av,@lk)", conn);
                        cmd.Parameters.AddWithValue("@tid", travelId);
                        cmd.Parameters.AddWithValue("@ag", agency);
                        cmd.Parameters.AddWithValue("@pr", price);
                        cmd.Parameters.AddWithValue("@av", avail);
                        cmd.Parameters.AddWithValue("@lk", link);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження: " + ex.Message);
            }
        }
    }
}