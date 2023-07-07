namespace Keyboard.HeatMap.Owner;

public class DbHandle : IDisposable
{
    private readonly DateTime startTime;
    private readonly DateTime endTime;
    private bool disposed = false;

    private Dictionary<string, SQLiteConnection> Connections { get; set; } = new Dictionary<string, SQLiteConnection>();
    public DbHandle() : this(DateTime.Now, DateTime.Now) { }
    public DbHandle(DateTime startTime) : this(startTime, DateTime.Now) { }
    public DbHandle(DateTime startTime, DateTime endTime)
    {
        this.startTime = startTime;
        this.endTime = endTime;
        var dates = GetDates();
        foreach (var date in dates)
        { Connections.Add(date.ToString(), new SQLiteConnection($"Data Source={GetDbName(date)};Version=3;")); }
    }

    ~DbHandle() => Dispose(false);
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                foreach (var item in Connections.Values)
                {
                    try
                    {
                        item.Close();
                        item.Dispose();
                    }
                    finally
                    {

                    }
                }
                Connections.Clear();
            }
            disposed = true;
        }
    }

    /// <summary>
    /// 获取一个可用的数据库的名称
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private string GetDbName(DbDate date = default)
    {
        if (date == default) { date = DateTime.Now; }
        string dbFloder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), WinForms.Application.ProductName, "Database");
        if (!Directory.Exists(dbFloder))
        { Directory.CreateDirectory(dbFloder); }
        string dbName = Path.Combine(dbFloder, date.ToString() + ".db");
        if (!File.Exists(dbName) && date.Month >= DateTime.Now.Month)
        {
            try
            {
                SQLiteConnection.CreateFile(dbName);
                using (SQLiteConnection Connection = new SQLiteConnection($"Data Source={dbName};Version=3;"))
                {
                    string sql = @"create table vk_key (Id integer primary key autoincrement, Code integer(4), Time varchar, Status integer(1) default 2);";
                    Connection.Open();
                    Connection.Execute(sql);
                    Connection.Close();
                }
            }
            catch
            {
                if (File.Exists(dbName))
                { File.Delete(dbName); }
            }
        }
        else
        {
        }
        return dbName;
    }

    /// <summary>
    /// 获取当前指定的日期段 yyyy-MM
    /// </summary>
    /// <returns></returns>
    public List<DbDate> GetDates()
    {
        List<DbDate> dates = new List<DbDate>();
        int toatlMonth = ((endTime.Year - startTime.Year) * 12) + (endTime.Month - startTime.Month);
        string dbFloder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), WinForms.Application.ProductName, "Database");
        if (toatlMonth == 0)
        {
            //string dbName = GetDbName(endTime);
            string dbName = Path.Combine(dbFloder, endTime.ToString("yyyy-MM") + ".db");
            if (File.Exists(dbName))
            { dates.Add(endTime); }
            else if (endTime.Date >= DateTime.Now.Date)
            {
                dbName = GetDbName(endTime);
                if (File.Exists(dbName))
                { dates.Add(endTime); }
            }
        }
        else
        {
            for (int i = 0; i < toatlMonth; i++)
            {
                string _dbName = Path.Combine(dbFloder, endTime.AddMonths(-i).ToString("yyyy-MM") + ".db");
                if (File.Exists(_dbName))
                { dates.Add(endTime.AddMonths(-i)); }
            }
            string dbName = Path.Combine(dbFloder, startTime.ToString("yyyy-MM") + ".db");
            if (File.Exists(dbName))
            { dates.Add(startTime); }
        }
        return dates;
    }

    /// <summary>
    /// 获取指定日期段内的数据库。
    /// <para>请勿释放</para>
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public SQLiteConnection GetConnection(DbDate date = default)
    {
        if (date == default) { date = DbDate.Now; }
        return Connections[date.ToString()];
    }

    /// <summary>
    /// 获取所有本地存在的数据库连接。
    /// <para>在 <see cref="SQLiteConnection"/> 使用完之后请依次手动调用 <see cref="SQLiteConnection.Dispose()"/> 释放。</para>
    /// </summary>
    /// <returns>所有本地存在的数据库连接</returns>
    public static List<SQLiteConnection> GetAllConnections()
    {
        string dbFloder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), WinForms.Application.ProductName, "Database");
        var dbNames = Directory.GetFiles(dbFloder).Where(x => Path.GetExtension(x).EndsWith("db")).ToList();
        List<SQLiteConnection> connections = new List<SQLiteConnection>();
        foreach (var dbName in dbNames)
        {
            connections.Add(new SQLiteConnection($"Data Source={dbName};Version=3;"));
        }
        return connections;
    }

    public static async Task ChangeDb()
    {
        await Task.Run(() =>
        {
            string dbFloder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), WinForms.Application.ProductName, "Database");
            var dbNames = Directory.GetFiles(dbFloder).Where(x => Path.GetExtension(x).EndsWith("db")).ToList();
            foreach (var dbName in dbNames)
            {
                using (SQLiteConnection db = new SQLiteConnection($"Data Source={dbName};Version=3;"))
                {
                    db.Open();
                    string sql = @"select sql from sqlite_master where type='table' and name='vk_key';";
                    if (db.Query<string>(sql).FirstOrDefault() == "CREATE TABLE vk_key (`Id` bigint(8) primary key,`KeyName` varchar,`DateTime` datetime)")
                    {
                        string alertSql = $@"alter table vk_key rename to _vk_key_old;
create table vk_key (Id integer primary key autoincrement, Code integer(4), Time varchar, Status integer(1) default 2);
insert into vk_key (Code, Time) select (case 
when KeyName='Modifiers' then -65536 
when KeyName='None' then 0 
when KeyName='LButton' then 1 
when KeyName='RButton' then 2 
when KeyName='Cancel' then 3 
when KeyName='MButton' then 4 
when KeyName='XButton1' then 5 
when KeyName='XButton2' then 6 
when KeyName='Back' then 8 
when KeyName='Tab' then 9 
when KeyName='LineFeed' then 10 
when KeyName='Clear' then 12 
when KeyName='Return' then 13 
when KeyName='Enter' then 13 
when KeyName='ShiftKey' then 16 
when KeyName='ControlKey' then 17 
when KeyName='Menu' then 18 
when KeyName='Pause' then 19 
when KeyName='Capital' then 20 
when KeyName='CapsLock' then 20 
when KeyName='KanaMode' then 21 
when KeyName='HanguelMode' then 21 
when KeyName='HangulMode' then 21 
when KeyName='JunjaMode' then 23 
when KeyName='FinalMode' then 24 
when KeyName='HanjaMode' then 25 
when KeyName='KanjiMode' then 25 
when KeyName='Escape' then 27 
when KeyName='IMEConvert' then 28 
when KeyName='IMENonconvert' then 29 
when KeyName='IMEAccept' then 30 
when KeyName='IMEAceept' then 30 
when KeyName='IMEModeChange' then 31 
when KeyName='Space' then 32 
when KeyName='Prior' then 33 
when KeyName='PageUp' then 33 
when KeyName='Next' then 34 
when KeyName='PageDown' then 34 
when KeyName='End' then 35 
when KeyName='Home' then 36 
when KeyName='Left' then 37 
when KeyName='Up' then 38 
when KeyName='Right' then 39 
when KeyName='Down' then 40 
when KeyName='Select' then 41 
when KeyName='Print' then 42 
when KeyName='Execute' then 43 
when KeyName='Snapshot' then 44 
when KeyName='PrintScreen' then 44 
when KeyName='Insert' then 45 
when KeyName='Delete' then 46 
when KeyName='Help' then 47 
when KeyName='D0' then 48 
when KeyName='D1' then 49 
when KeyName='D2' then 50 
when KeyName='D3' then 51 
when KeyName='D4' then 52 
when KeyName='D5' then 53 
when KeyName='D6' then 54 
when KeyName='D7' then 55 
when KeyName='D8' then 56 
when KeyName='D9' then 57 
when KeyName='A' then 65 
when KeyName='B' then 66 
when KeyName='C' then 67 
when KeyName='D' then 68 
when KeyName='E' then 69 
when KeyName='F' then 70 
when KeyName='G' then 71 
when KeyName='H' then 72 
when KeyName='I' then 73 
when KeyName='J' then 74 
when KeyName='K' then 75 
when KeyName='L' then 76 
when KeyName='M' then 77 
when KeyName='N' then 78 
when KeyName='O' then 79 
when KeyName='P' then 80 
when KeyName='Q' then 81 
when KeyName='R' then 82 
when KeyName='S' then 83 
when KeyName='T' then 84 
when KeyName='U' then 85 
when KeyName='V' then 86 
when KeyName='W' then 87 
when KeyName='X' then 88 
when KeyName='Y' then 89 
when KeyName='Z' then 90 
when KeyName='LWin' then 91 
when KeyName='RWin' then 92 
when KeyName='Apps' then 93 
when KeyName='Sleep' then 95 
when KeyName='NumPad0' then 96 
when KeyName='NumPad1' then 97 
when KeyName='NumPad2' then 98 
when KeyName='NumPad3' then 99 
when KeyName='NumPad4' then 100 
when KeyName='NumPad5' then 101 
when KeyName='NumPad6' then 102 
when KeyName='NumPad7' then 103 
when KeyName='NumPad8' then 104 
when KeyName='NumPad9' then 105 
when KeyName='Multiply' then 106 
when KeyName='Add' then 107 
when KeyName='Separator' then 108 
when KeyName='Subtract' then 109 
when KeyName='Decimal' then 110 
when KeyName='Divide' then 111 
when KeyName='F1' then 112 
when KeyName='F2' then 113 
when KeyName='F3' then 114 
when KeyName='F4' then 115 
when KeyName='F5' then 116 
when KeyName='F6' then 117 
when KeyName='F7' then 118 
when KeyName='F8' then 119 
when KeyName='F9' then 120 
when KeyName='F10' then 121 
when KeyName='F11' then 122 
when KeyName='F12' then 123 
when KeyName='F13' then 124 
when KeyName='F14' then 125 
when KeyName='F15' then 126 
when KeyName='F16' then 127 
when KeyName='F17' then 128 
when KeyName='F18' then 129 
when KeyName='F19' then 130 
when KeyName='F20' then 131 
when KeyName='F21' then 132 
when KeyName='F22' then 133 
when KeyName='F23' then 134 
when KeyName='F24' then 135 
when KeyName='NumLock' then 144 
when KeyName='Scroll' then 145 
when KeyName='LShiftKey' then 160 
when KeyName='RShiftKey' then 161 
when KeyName='LControlKey' then 162 
when KeyName='RControlKey' then 163 
when KeyName='LMenu' then 164 
when KeyName='RMenu' then 165 
when KeyName='BrowserBack' then 166 
when KeyName='BrowserForward' then 167 
when KeyName='BrowserRefresh' then 168 
when KeyName='BrowserStop' then 169 
when KeyName='BrowserSearch' then 170 
when KeyName='BrowserFavorites' then 171 
when KeyName='BrowserHome' then 172 
when KeyName='VolumeMute' then 173 
when KeyName='VolumeDown' then 174 
when KeyName='VolumeUp' then 175 
when KeyName='MediaNextTrack' then 176 
when KeyName='MediaPreviousTrack' then 177 
when KeyName='MediaStop' then 178 
when KeyName='MediaPlayPause' then 179 
when KeyName='LaunchMail' then 180 
when KeyName='SelectMedia' then 181 
when KeyName='LaunchApplication1' then 182 
when KeyName='LaunchApplication2' then 183 
when KeyName='OemSemicolon' then 186 
when KeyName='Oem1' then 186 
when KeyName='Oemplus' then 187 
when KeyName='Oemcomma' then 188 
when KeyName='OemMinus' then 189 
when KeyName='OemPeriod' then 190 
when KeyName='OemQuestion' then 191 
when KeyName='Oem2' then 191 
when KeyName='Oemtilde' then 192 
when KeyName='Oem3' then 192 
when KeyName='OemOpenBrackets' then 219 
when KeyName='Oem4' then 219 
when KeyName='OemPipe' then 220 
when KeyName='Oem5' then 220 
when KeyName='OemCloseBrackets' then 221 
when KeyName='Oem6' then 221 
when KeyName='OemQuotes' then 222 
when KeyName='Oem7' then 222 
when KeyName='Oem8' then 223 
when KeyName='OemBackslash' then 226 
when KeyName='Oem102' then 226 
when KeyName='ProcessKey' then 229 
when KeyName='Packet' then 231 
when KeyName='Attn' then 246 
when KeyName='Crsel' then 247 
when KeyName='Exsel' then 248 
when KeyName='EraseEof' then 249 
when KeyName='Play' then 250 
when KeyName='Zoom' then 251 
when KeyName='NoName' then 252 
when KeyName='Pa1' then 253 
when KeyName='OemClear' then 254 
when KeyName='KeyCode' then 65535 
when KeyName='Shift' then 65536 
when KeyName='Control' then 131072 
when KeyName='Alt' then 26214 
else 0 end) as code, DateTime from _vk_key_old;";
                        db.Execute(alertSql);
                    }
                    db.Close();
                }
            }
        });
    }
}
