namespace Keyboard.HeatMap.Owner;

/// <summary>
/// 键盘按键记录器
/// </summary>
public class KeyBoardHandle : IDisposable
{

    // Field
    /// <summary>
    /// 键盘钩子
    /// </summary>
    KeyBoardHook KeyHook;
    /// <summary>
    /// 记录器状态
    /// </summary>
    private KeyState keyState;

    public delegate void KeyStateEventHandle(object sender, KeyState state);
    /// <summary>
    /// 按键按下事件
    /// </summary>
    public event System.Windows.Forms.KeyEventHandler KeyDown;
    /// <summary>
    /// 按键抬起事件
    /// </summary>
    public event System.Windows.Forms.KeyEventHandler KeyUp;
    /// <summary>
    /// 状态改变时发生
    /// </summary>
    public event KeyStateEventHandle KeyStateChanged;
    DbHandle handle;
    SQLiteConnection db;
    private bool disposed = false;

    // Property
    /// <summary>
    /// 获取当前记录器状态
    /// </summary>
    public KeyState KeyState { get => keyState; }

    // class
    /// <summary>
    /// 初始化
    /// </summary>
    public KeyBoardHandle()
    {
        KeyHook = new KeyBoardHook();
        KeyHook.OnKeyDownEvent += (s, e) =>
        {
            KeyDown?.Invoke(this, e);
        };
        KeyHook.OnKeyUpEvent += (s, e) =>
        {
            KeyUp?.Invoke(this, e);
        };
        keyState = KeyState.Closed;
        handle = new DbHandle();
        db = handle.GetConnection();
    }

    ~KeyBoardHandle()
    {
        Dispose(false);
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            Stop();
            if (disposing)
            {
                try
                {
                    handle.Dispose();
                    KeyHook.Dispose();
                }
                finally
                {

                }
            }
            disposed = true;
        }
    }

    // event handle

    private void Hook_OnKeyDownEvent(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        KeyData keyData = new KeyData()
        {
            Time = DateTime.Now,
            Code = (int)e.KeyCode,
            Status = 1,
        };
        using (var handle = new DbHandle())
        {
            var db = handle.GetConnection();
            db.Open();
            var res = db.Insert(keyData);
            db.Close();
        }
    }
    /// <summary>
    /// 记录击键
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hook_OnKeyUpEvent(object sender, System.Windows.Forms.KeyEventArgs e)
    {
        KeyData keyData = new KeyData()
        {
            Time = DateTime.Now,
            Code = (int)e.KeyCode,
            Status = 2,
        };
        using (var handle = new DbHandle())
        {
            var res = db.Insert(keyData);
        }
    }

    // public
    /// <summary>
    /// 开始记录
    /// </summary>
    public void Start()
    {
        if (KeyState == KeyState.Closed)
        {
            db.Open();
            keyState = KeyState.Open;
            KeyHook.OnKeyDownEvent += Hook_OnKeyDownEvent;
            KeyHook.OnKeyUpEvent += Hook_OnKeyUpEvent;
            KeyStateChanged?.Invoke(this, keyState);
        }
    }

    /// <summary>
    /// 停止记录
    /// </summary>
    public void Stop()
    {
        if (KeyState == KeyState.Open)
        {
            keyState = KeyState.Closed;
            KeyHook.OnKeyDownEvent -= Hook_OnKeyDownEvent;
            KeyHook.OnKeyUpEvent -= Hook_OnKeyUpEvent;
            KeyStateChanged?.Invoke(this, keyState);
            db.Close();
        }
    }


    /// <summary>
    /// 获取指定时间段内，每个键的击键记录数。
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public Dictionary<System.Windows.Forms.Keys, long> Count(DateTime start, DateTime end)
    {
        if (start == end)
        { return new Dictionary<System.Windows.Forms.Keys, long>(); }
        var min = start < end ? start : end;
        var max = start > end ? start : end;
        string sql = $"select Code,count(*) as Count from vk_key where Status=2 and Time between '{start:yyyy-MM-dd HH:mm:ss}' and '{end:yyyy-MM-dd HH:mm:ss}' group by Code;";

        Dictionary<System.Windows.Forms.Keys, long> vals = new Dictionary<System.Windows.Forms.Keys, long>();
        using (DbHandle handle = new DbHandle(start, end))
        {
            foreach (var date in handle.GetDates())
            {
                var _db = handle.GetConnection(date);
                _db.Open();
                var datas = _db.Query<KeyCount>(sql);
                _db.Close();
                foreach (var data in datas)
                {
                    System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)data.Code;

                    if (vals.ContainsKey(key))
                    { vals[key] += data.Count; }
                    else
                    { vals.Add(key, data.Count); }
                }
            }
        }
        return vals;
    }
    /// <summary>
    /// 获取每个键的所有击键记录数。
    /// </summary>
    /// <returns></returns>
    public Dictionary<System.Windows.Forms.Keys, long> Count()
    {
        string sql = $"select Code,count(*) as Count from vk_key where Status=2 group by Code;";

        Dictionary<System.Windows.Forms.Keys, long> vals = new Dictionary<System.Windows.Forms.Keys, long>();
        var dbs = DbHandle.GetAllConnections();
        foreach (var db in dbs)
        {
            db.Open();
            var datas = db.Query<KeyCount>(sql);
            db.Close();
            db.Dispose();
            foreach (var data in datas)
            {
                System.Windows.Forms.Keys key = (System.Windows.Forms.Keys)data.Code;

                if (vals.ContainsKey(key))
                { vals[key] += data.Count; }
                else
                { vals.Add(key, data.Count); }
            }
        }
        return vals;
    }

    /// <summary>
    /// 获取指定时间段内，指定的键的击键记录数。
    /// </summary>
    /// <param name="keys">指定按键</param>
    /// <param name="start">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <returns></returns>
    public long KeyCounts(System.Windows.Forms.Keys keys, DateTime start, DateTime end)
    {
        if (start == end)
        { return 0L; }
        var min = start < end ? start : end;
        var max = start > end ? start : end;
        long count = 0;
        string sql = $"select count(*) from vk_key where Status=2 and Time between '{min:yyyy-MM-dd HH:mm:ss}' and '{max:yyyy-MM-dd HH:mm:ss}' and Code={(int)keys};";
        using (DbHandle handle = new DbHandle(start, end))
        {
            var dates = handle.GetDates();
            foreach (var date in dates)
            {
                var db = handle.GetConnection(date);
                db.Open();
                count = db.QueryFirstOrDefault<long>(sql);
                db.Close();
            }
        }
        return count;
    }
    /// <summary>
    /// 获取指定的键的所有击键记录数。
    /// </summary>
    /// <param name="keys">指定按键</param>
    /// <returns></returns>
    public long KeyCounts(System.Windows.Forms.Keys keys)
    {
        string sql = $"select count(*) as Count from vk_key where Status=2 and Code={(int)keys};";
        long count = 0;
        foreach (var item in DbHandle.GetAllConnections())
        {
            item.Open();
            count += item.QueryFirstOrDefault<long>(sql);
            item.Close();
            item.Dispose();
        }
        return count;
    }
}
