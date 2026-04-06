using Sunny.UI;
using System.ComponentModel;
using System.Text;

namespace QHWMTOOL
{
    public partial class FRM_Facebook : Form
    {
        private const string RuntimePrimaryField = "facebook_reaction";
        private const string RuntimeSecondaryField = "facebook_reaction2";

        private readonly ConfigService _configService = new();
        private readonly TdsService _tdsService = new();
        private readonly FacebookService _facebookService = new();

        private string TDS_token = string.Empty;
        private string fb_dtsg = string.Empty;
        private string lsd = string.Empty;
        private string cookie = string.Empty;
        private string userId = string.Empty;
        private string userName = string.Empty;
        private AppConfig _config = new();

        private CancellationTokenSource? _runtimeRunCts;
        private bool _runtimeIsRunning;
        private string _runtimeTaskField = RuntimePrimaryField;
        private int _runtimeTargetTasks;
        private int _runtimeCompletedTasks;
        private int _runtimeFailedTasks;

        public FRM_Facebook()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            StartPosition = FormStartPosition.CenterScreen;
            if (!IsInDesignMode())
            {
                ApplyVisualStyle();
            }
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }

        private void AppendLog(string message, Color color, UIRichTextBox richTextBox)
        {
            richTextBox.SelectionStart = richTextBox.TextLength;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = color;
            richTextBox.AppendText(message + Environment.NewLine);
            richTextBox.SelectionColor = richTextBox.ForeColor;
            richTextBox.ScrollToCaret();
        }

        private void SetRunningState(bool isRunning)
        {
            BTN_BatDau.Enabled = !isRunning;
            BTN_KetThuc.Enabled = isRunning;
            BTN_LuuCauHinh.Enabled = !isRunning;

            foreach (var checkbox in GRB_TheLoai.Controls.OfType<UICheckBox>())
            {
                checkbox.ReadOnly = isRunning;
            }

            foreach (var textBox in GRB_CauHinh.Controls.OfType<UITextBox>())
            {
                textBox.ReadOnly = isRunning;
            }
        }

        private void RTB_Chinh_DoubleClick(object sender, EventArgs e)
        {
            RTB_Chinh.Visible = false;
            RTB_Log.Visible = true;
            RTB_Log.BringToFront();
            GRP_HienThi.Text = "Nhật ký hệ thống";
        }

        private void RTB_Log_DoubleClick(object sender, EventArgs e)
        {
            RTB_Log.Visible = false;
            RTB_Chinh.Visible = true;
            RTB_Chinh.BringToFront();
            GRP_HienThi.Text = "Nhật ký công việc";
        }

        private void ApplyVisualStyle()
        {
            var bodyFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            var headingFont = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            var monoFont = new Font("Consolas", 9.5F, FontStyle.Regular);
            var panelColor = Color.FromArgb(244, 247, 252);
            var cardColor = Color.White;
            var borderColor = Color.FromArgb(214, 222, 235);
            var accentColor = Color.FromArgb(21, 93, 186);

            BackColor = panelColor;
            ForeColor = Color.FromArgb(26, 39, 68);
            Text = "QH-TDS | Bảng điều khiển";

            PNL_Chinh.FillColor = panelColor;
            PNL_Chinh.RectColor = panelColor;
            PNL_Chinh.Padding = new Padding(12);
            GRP_HienThi.Text = "Nhật ký công việc";

            foreach (var groupBox in GetControlsOfType<UIGroupBox>(this))
            {
                groupBox.Font = headingFont;
                groupBox.FillColor = cardColor;
                groupBox.RectColor = borderColor;
                groupBox.ForeColor = ForeColor;
            }

            foreach (var label in GetControlsOfType<UILabel>(this))
            {
                label.Font = bodyFont;
                label.ForeColor = Color.FromArgb(60, 72, 102);
            }

            foreach (var textBox in GetControlsOfType<UITextBox>(this))
            {
                textBox.Font = bodyFont;
                textBox.FillColor = Color.FromArgb(249, 251, 255);
                textBox.RectColor = borderColor;
                textBox.ForeColor = ForeColor;
                textBox.Padding = new Padding(8, 5, 8, 5);
            }

            foreach (var checkBox in GetControlsOfType<UICheckBox>(this))
            {
                checkBox.Font = bodyFont;
                checkBox.ForeColor = ForeColor;
            }

            StyleButton(BTN_BatDau, Color.FromArgb(22, 163, 74), Color.White);
            StyleButton(BTN_KetThuc, Color.FromArgb(220, 38, 38), Color.White);
            StyleButton(BTN_LuuCauHinh, accentColor, Color.White);

            TXB_TongXu.FillColor = Color.FromArgb(235, 248, 239);
            TXB_TongXu.RectColor = Color.FromArgb(134, 188, 158);
            TXB_XuDie.FillColor = Color.FromArgb(255, 245, 228);
            TXB_XuDie.RectColor = Color.FromArgb(231, 179, 87);
            TXB_TienDo.FillColor = Color.FromArgb(236, 243, 255);
            TXB_TienDo.RectColor = accentColor;
            TXB_ThanhCong.FillColor = Color.FromArgb(235, 248, 239);
            TXB_ThanhCong.RectColor = Color.FromArgb(52, 168, 83);
            TXB_ThatBai.FillColor = Color.FromArgb(255, 238, 238);
            TXB_ThatBai.RectColor = Color.FromArgb(220, 38, 38);
            TXB_TiLeThanhCong.FillColor = Color.FromArgb(241, 239, 255);
            TXB_TiLeThanhCong.RectColor = Color.FromArgb(108, 92, 231);
            TXB_FieldDangChay.FillColor = Color.FromArgb(246, 248, 252);

            RTB_Chinh.Font = monoFont;
            RTB_Log.Font = monoFont;
            RTB_Chinh.FillColor = Color.FromArgb(251, 252, 254);
            RTB_Chinh.RectColor = borderColor;
            RTB_Log.FillColor = Color.FromArgb(251, 252, 254);
            RTB_Log.RectColor = borderColor;
            SLT_TenAcc.Font = bodyFont;
            SLT_TenAcc.ForeColor = accentColor;
            TXB_TrangThai.ForeColor = accentColor;
            TXB_TrangThai.RectColor = borderColor;
            TXB_TrangThai.FillColor = Color.FromArgb(237, 244, 255);
        }

        private static void StyleButton(UIButton button, Color fillColor, Color textColor)
        {
            button.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            button.FillColor = fillColor;
            button.FillHoverColor = ControlPaint.Light(fillColor, 0.05f);
            button.FillPressColor = ControlPaint.Dark(fillColor, 0.05f);
            button.RectColor = fillColor;
            button.ForeColor = textColor;
        }

        private static IEnumerable<T> GetControlsOfType<T>(Control root) where T : Control
        {
            foreach (Control child in root.Controls)
            {
                if (child is T typedControl)
                {
                    yield return typedControl;
                }

                foreach (var nestedControl in GetControlsOfType<T>(child))
                {
                    yield return nestedControl;
                }
            }
        }

        private async void Runtime_FRM_Chinh_Load(object sender, EventArgs e)
        {
            if (IsInDesignMode())
            {
                return;
            }

            RuntimeLoadConfigToState();
            RuntimeApplyStateToUi();
            SetRunningState(false);
            RuntimeResetDashboard();
            await RuntimeRefreshProfileAsync();
        }

        private async void Runtime_BTN_LuuCauHinh_Click(object sender, EventArgs e)
        {
            if (!RuntimeTryReadSettingsFromUi(out var settings, out var errorMessage))
            {
                RuntimeShowError(errorMessage);
                return;
            }

            _config = settings;

            if (_config.FacebookSession.Cookie.Length > 50)
            {
                var validation = await _facebookService.ValidateSessionAsync(_config.FacebookSession.Cookie);
                if (!validation.Success)
                {
                    RuntimeShowError(validation.ErrorMessage ?? "Không thể xác thực cookie Facebook.");
                    return;
                }

                _config.FacebookSession = validation.Session!;
            }

            RuntimePersistConfig();
            RuntimeApplyStateToUi();
            await RuntimeRefreshProfileAsync();
            AppendLog(">>> Đã lưu cấu hình thành công!", Color.DodgerBlue, RTB_Log);
        }

        private async void Runtime_BTN_BatDau_Click(object sender, EventArgs e)
        {
            if (_runtimeIsRunning)
            {
                return;
            }

            if (!RuntimeTryReadSettingsFromUi(out var settings, out var errorMessage))
            {
                RuntimeShowError(errorMessage);
                return;
            }

            _config = settings;

            var validation = await _facebookService.ValidateSessionAsync(_config.FacebookSession.Cookie);
            if (!validation.Success)
            {
                RuntimeShowError(validation.ErrorMessage ?? "Không thể xác thực cookie Facebook.");
                return;
            }

            _config.FacebookSession = validation.Session!;
            RuntimePersistConfig();

            _runtimeTaskField = RuntimePrimaryField;
            RuntimeStartDashboard(_config.TotalTasks);
            _runtimeRunCts = new CancellationTokenSource();
            _runtimeIsRunning = true;
            SetRunningState(true);
            TXB_TokenTDS.PasswordChar = '*';
            RTB_Chinh.Clear();
            RTB_Log.Clear();
            RuntimeSetStatus("Đang khởi động");
            AppendLog(">>> TOOL BẮT ĐẦU CHẠY <<<", Color.DeepSkyBlue, RTB_Chinh);

            try
            {
                await RuntimeRunLoopAsync(_runtimeRunCts.Token);
            }
            catch (OperationCanceledException)
            {
                RuntimeSetStatus("Đã dừng");
                AppendLog(">>> Đã nhận lệnh dừng tool.", Color.DodgerBlue, RTB_Log);
            }
            finally
            {
                _runtimeRunCts?.Dispose();
                _runtimeRunCts = null;
                _runtimeIsRunning = false;
                SetRunningState(false);
                TXB_TokenTDS.PasswordChar = '\0';
            }
        }

        private void Runtime_BTN_KetThuc_Click(object sender, EventArgs e)
        {
            if (!_runtimeIsRunning)
            {
                return;
            }

            RuntimeSetStatus("Đang dừng");
            _runtimeRunCts?.Cancel();
            AppendLog(">>> Đang dừng tool, vui lòng đợi nhiệm vụ hiện tại kết thúc... <<<", Color.DarkOrange, RTB_Chinh);
        }

        private async Task RuntimeRunLoopAsync(CancellationToken cancellationToken)
        {
            var selectedReactions = RuntimeGetSelectedReactionTypes();
            var type = selectedReactions.Count == 6 ? "ALL" : string.Join(", ", selectedReactions);
            var totalTasks = _config.TotalTasks;
            var tasksPerBatch = Math.Min(_config.TasksPerBatch, totalTasks);
            var completedTasks = 0;

            AppendLog($"Loại nhiệm vụ đã chọn: {type}", Color.DodgerBlue, RTB_Log);
            RuntimeSetStatus("Đang lấy nhiệm vụ");

            while (!cancellationToken.IsCancellationRequested && completedTasks < totalTasks)
            {
                RuntimeSetStatus("Đang lấy nhiệm vụ");
                var tasksResult = await _tdsService.GetTasksAsync(_config.TdsToken, _runtimeTaskField, type);
                if (!string.IsNullOrWhiteSpace(tasksResult.ErrorMessage))
                {
                    await RuntimeWaitWithCountdownAsync(tasksResult.CountdownSeconds, $"TDS trả lời: {tasksResult.ErrorMessage}", cancellationToken);
                    continue;
                }

                if (tasksResult.Tasks.Count == 0)
                {
                    RuntimeToggleTaskField();
                    RuntimeSetStatus("Tạm hết nhiệm vụ, đổi luồng khác");
                    await RuntimeWaitWithCountdownAsync(120, "Tạm hết nhiệm vụ, đổi field và chờ đợt mới", cancellationToken);
                    continue;
                }

                AppendLog($"Đã lấy được {tasksResult.Tasks.Count} nhiệm vụ từ TDS.", Color.DodgerBlue, RTB_Log);

                foreach (var task in tasksResult.Tasks)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (completedTasks >= totalTasks)
                    {
                        break;
                    }

                    var postId = RuntimeExtractPostId(task.id);
                    var taskUserId = RuntimeExtractUserId(task.id);
                    RuntimeSetStatus($"Đang xử lý {task.type} | {completedTasks + 1}/{totalTasks}");
                    var reactResult = await _facebookService.ReactAsync(_config.FacebookSession, task, task.type);

                    if (reactResult.IsBlocked)
                    {
                        RuntimeCountFailure("Tài khoản Facebook bị chặn cảm xúc");
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] Tài khoản Facebook đã bị chặn thả cảm xúc.", Color.DarkRed, RTB_Log);
                        RuntimeAppendSpamLog(reactResult.ErrorMessage ?? "Tài khoản Facebook đã bị chặn thả cảm xúc.");
                        _runtimeRunCts?.Cancel();
                        return;
                    }

                    if (!reactResult.Success)
                    {
                        RuntimeCountFailure($"Thả cảm xúc thất bại | {task.type}");
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] FACEBOOK {task.type} | ID POST {postId} thất bại: {reactResult.ErrorMessage}", Color.Red, RTB_Chinh);
                    }
                    else
                    {
                        var claimResult = await _tdsService.ClaimCoinAsync(_config.TdsToken, _runtimeTaskField, task.code);
                        if (claimResult.Data is not null)
                        {
                            completedTasks++;
                            RuntimeCountSuccess($"Đã nhận xu | {completedTasks}/{totalTasks}");
                            TXB_TongXu.Text = claimResult.Data.xu.ToString("N0");
                            AppendLog(
                                $"[{DateTime.Now:HH:mm:ss}] [{completedTasks}/{totalTasks}] ID USER: {taskUserId} | ID POST: {postId} | {claimResult.Data.msg} | TONG XU: {claimResult.Data.xu:N0}",
                                Color.LimeGreen,
                                RTB_Chinh);
                        }
                        else
                        {
                            RuntimeCountFailure("Nhận xu thất bại");
                            AppendLog(
                                $"[{DateTime.Now:HH:mm:ss}] {claimResult.ErrorMessage} | ID POST: {postId} | TYPE: {task.type}",
                                Color.Red,
                                RTB_Chinh);
                        }
                    }

                    if (completedTasks >= totalTasks)
                    {
                        break;
                    }

                    var delaySeconds = Random.Shared.Next(_config.DelayMin, _config.DelayMax + 1);
                    var message = $"[{completedTasks + 1}/{totalTasks}] Chờ nghỉ";

                    if (completedTasks > 0 && completedTasks % tasksPerBatch == 0)
                    {
                        delaySeconds = _config.RestSeconds;
                        message = $"Đã hoàn thành {completedTasks} nhiệm vụ, nghỉ {_config.RestSeconds}s trước khi lấy đợt mới";
                    }

                    await RuntimeWaitWithCountdownAsync(delaySeconds, message, cancellationToken);
                }

                if (completedTasks < totalTasks)
                {
                    await RuntimeWaitWithCountdownAsync(120, "Đợi lấy nhiệm vụ đợt mới", cancellationToken);
                }
            }

            RuntimeSetStatus("Hoàn tất");
            AppendLog($"Đã hoàn thành {completedTasks} nhiệm vụ, tạm biệt và hẹn gặp lại.", Color.DeepSkyBlue, RTB_Chinh);
        }

        private async Task RuntimeRefreshProfileAsync()
        {
            if (string.IsNullOrWhiteSpace(_config.TdsToken))
            {
                TXB_TongXu.Text = "0";
                TXB_XuDie.Text = "0";
                SLT_TenAcc.Text = "Chưa có token TDS";
                return;
            }

            var profile = await _tdsService.GetProfileAsync(_config.TdsToken);
            if (profile is null)
            {
                TXB_TongXu.Text = "0";
                TXB_XuDie.Text = "0";
                SLT_TenAcc.Text = "Không tải được hồ sơ TDS";
                return;
            }

            TXB_TongXu.Text = long.TryParse(profile.xu, out var xu) ? xu.ToString("N0") : "0";
            TXB_XuDie.Text = long.TryParse(profile.xudie, out var xuDie) ? xuDie.ToString("N0") : "0";
            SLT_TenAcc.Text = string.IsNullOrWhiteSpace(_config.FacebookSession.UserName)
                ? $"Xin chào: {profile.user}"
                : $"Xin chào: {profile.user} | FB: {_config.FacebookSession.UserName}";
        }

        private bool RuntimeTryReadSettingsFromUi(out AppConfig config, out string errorMessage)
        {
            config = _config;
            errorMessage = string.Empty;

            var token = TXB_TokenTDS.Text.Trim();
            var cookieValue = TXB_Cookie.Text.Trim();

            if (string.IsNullOrWhiteSpace(token))
            {
                errorMessage = "Vui lòng nhập token TDS.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(cookieValue))
            {
                errorMessage = "Vui lòng nhập cookie Facebook.";
                return false;
            }

            if (!RuntimeTryReadPositiveInt(TXB_DelayMin.Text, out var delayMin))
            {
                errorMessage = "Delay tối thiểu phải là số nguyên dương.";
                return false;
            }

            if (!RuntimeTryReadPositiveInt(TXB_DelayMax.Text, out var delayMax))
            {
                errorMessage = "Delay tối đa phải là số nguyên dương.";
                return false;
            }

            if (delayMin > delayMax)
            {
                errorMessage = "Delay tối thiểu phải nhỏ hơn hoặc bằng delay tối đa.";
                return false;
            }

            if (!RuntimeTryReadPositiveInt(TXB_TongNhiemVu.Text, out var totalTasks))
            {
                errorMessage = "Tổng nhiệm vụ phải là số nguyên dương.";
                return false;
            }

            if (!RuntimeTryReadPositiveInt(TXB_SoNghiemVuMoiSet.Text, out var tasksPerBatch))
            {
                errorMessage = "Số nhiệm vụ mỗi đợt phải là số nguyên dương.";
                return false;
            }

            if (!RuntimeTryReadPositiveInt(TXB_SoDayNghi.Text, out var restSeconds))
            {
                errorMessage = "Số giây nghỉ phải là số nguyên dương.";
                return false;
            }

            if (RuntimeGetSelectedReactionTypes().Count == 0)
            {
                errorMessage = "Hãy chọn ít nhất một loại cảm xúc.";
                return false;
            }

            config = new AppConfig
            {
                TdsToken = token,
                DelayMin = delayMin,
                DelayMax = delayMax,
                TotalTasks = totalTasks,
                TasksPerBatch = tasksPerBatch,
                RestSeconds = restSeconds,
                FacebookSession = new FacebookSession
                {
                    Cookie = cookieValue,
                    FbDtsg = _config.FacebookSession.FbDtsg,
                    Lsd = _config.FacebookSession.Lsd,
                    UserId = _config.FacebookSession.UserId,
                    UserName = _config.FacebookSession.UserName
                }
            };

            return true;
        }

        private void RuntimeLoadConfigToState()
        {
            _config = _configService.Load();
            RuntimeSyncLegacyFields();
        }

        private void RuntimeApplyStateToUi()
        {
            TXB_TokenTDS.Text = _config.TdsToken;
            TXB_DelayMin.Text = _config.DelayMin.ToString();
            TXB_DelayMax.Text = _config.DelayMax.ToString();
            TXB_TongNhiemVu.Text = _config.TotalTasks.ToString();
            TXB_SoNghiemVuMoiSet.Text = _config.TasksPerBatch.ToString();
            TXB_SoDayNghi.Text = _config.RestSeconds.ToString();
            TXB_Cookie.Text = _config.FacebookSession.Cookie;
        }

        private void RuntimePersistConfig()
        {
            RuntimeSyncLegacyFields();
            _configService.Save(_config);
        }

        private void RuntimeSyncLegacyFields()
        {
            TDS_token = _config.TdsToken;
            cookie = _config.FacebookSession.Cookie;
            fb_dtsg = _config.FacebookSession.FbDtsg;
            lsd = _config.FacebookSession.Lsd;
            userId = _config.FacebookSession.UserId;
            userName = _config.FacebookSession.UserName;
        }

        private async Task RuntimeWaitWithCountdownAsync(int seconds, string message, CancellationToken cancellationToken)
        {
            RTB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}: ");
            var startIndex = RTB_Log.TextLength;

            for (var remaining = seconds; remaining >= 0; remaining--)
            {
                cancellationToken.ThrowIfCancellationRequested();
                RuntimeSetStatus($"{message} ({remaining}s)");
                RTB_Log.Select(startIndex, RTB_Log.TextLength - startIndex);
                RTB_Log.SelectedText = $"{remaining}s...";
                await Task.Delay(1000, cancellationToken);
            }

            RTB_Log.AppendText(Environment.NewLine);
        }

        private void RuntimeAppendSpamLog(string message)
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "Log_Spam.txt");
            File.AppendAllText(logPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}", Encoding.UTF8);
        }

        private List<string> RuntimeGetSelectedReactionTypes()
        {
            return GRB_TheLoai.Controls
                .OfType<UICheckBox>()
                .Where(static checkbox => checkbox.Checked)
                .Select(static checkbox => checkbox.Text?.Trim() ?? string.Empty)
                .Where(static text => !string.IsNullOrWhiteSpace(text))
                .ToList();
        }

        private void RuntimeToggleTaskField()
        {
            _runtimeTaskField = _runtimeTaskField == RuntimePrimaryField ? RuntimeSecondaryField : RuntimePrimaryField;
            RuntimeRefreshDashboard();
        }

        private static bool RuntimeTryReadPositiveInt(string input, out int value)
        {
            return int.TryParse(input, out value) && value > 0;
        }

        private static string RuntimeExtractPostId(string rawId)
        {
            return rawId.Contains('_') ? rawId.Split('_')[1] : rawId;
        }

        private static string RuntimeExtractUserId(string rawId)
        {
            return rawId.Contains('_') ? rawId.Split('_')[0] : string.Empty;
        }

        private void RuntimeShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void RuntimeResetDashboard()
        {
            _runtimeTargetTasks = 0;
            _runtimeCompletedTasks = 0;
            _runtimeFailedTasks = 0;
            RuntimeUpdateDashboard("Sẵn sàng");
        }

        private void RuntimeStartDashboard(int totalTasks)
        {
            _runtimeTaskField = RuntimePrimaryField;
            _runtimeTargetTasks = totalTasks;
            _runtimeCompletedTasks = 0;
            _runtimeFailedTasks = 0;
            RuntimeUpdateDashboard("Đang khởi động");
        }

        private void RuntimeCountSuccess(string status)
        {
            _runtimeCompletedTasks++;
            RuntimeUpdateDashboard(status);
        }

        private void RuntimeCountFailure(string status)
        {
            _runtimeFailedTasks++;
            RuntimeUpdateDashboard(status);
        }

        private void RuntimeSetStatus(string status)
        {
            RuntimeUpdateDashboard(status);
        }

        private void RuntimeRefreshDashboard()
        {
            RuntimeUpdateDashboard(TXB_TrangThai.Text);
        }

        private void RuntimeUpdateDashboard(string status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => RuntimeUpdateDashboard(status)));
                return;
            }

            var attempts = _runtimeCompletedTasks + _runtimeFailedTasks;
            TXB_TrangThai.Text = status;
            TXB_TienDo.Text = _runtimeTargetTasks > 0
                ? $"{_runtimeCompletedTasks}/{_runtimeTargetTasks}"
                : "0/0";
            TXB_ThanhCong.Text = _runtimeCompletedTasks.ToString();
            TXB_ThatBai.Text = _runtimeFailedTasks.ToString();
            TXB_TiLeThanhCong.Text = attempts == 0
                ? "0%"
                : $"{((double)_runtimeCompletedTasks / attempts) * 100:0.#}%";
            TXB_FieldDangChay.Text = _runtimeTaskField == RuntimePrimaryField ? "Luồng chính" : "Luồng dự phòng";
            TXB_TrangThai.ForeColor = ResolveStatusColor(status);
        }

        private static Color ResolveStatusColor(string status)
        {
            if (status.Contains("Hoàn tất", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(22, 163, 74);
            }

            if (status.Contains("thất bại", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("chặn", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("dừng", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(220, 38, 38);
            }

            if (status.Contains("đợi", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("chờ", StringComparison.OrdinalIgnoreCase) ||
                status.Contains("tạm", StringComparison.OrdinalIgnoreCase))
            {
                return Color.FromArgb(217, 119, 6);
            }

            return Color.FromArgb(21, 93, 186);
        }
    }
}
