using Keyboard.HeatMap.HookGlobal;
using Keyboard.HeatMap.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using View.SQL;
using View.SQL.Extensions;
using View.SQLite;

namespace Keyboard.HeatMap.Owner
{
    /// <summary>
    /// 键盘按键记录器
    /// </summary>
    public class KeyBoardHandle
    {

        // Field
        /// <summary>
        /// 键盘钩子
        /// </summary>
        KeyBoardHook KeyHook;
        /// <summary>
        /// 数据库连接
        /// </summary>
        SQLiteHandle db;
        /// <summary>
        /// 记录器状态
        /// </summary>
        private KeyState keyState;

        public delegate void KeyStateEventHandle(object sender, KeyState state);
        /// <summary>
        /// 按键按下事件
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// 按键抬起事件
        /// </summary>
        public event KeyEventHandler KeyUp;
        /// <summary>
        /// 状态改变时发生
        /// </summary>
        public event KeyStateEventHandle KeyStateChanged;

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
            db = CreatDB();
            db.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        ~KeyBoardHandle()
        {
            db.Close();
        }

        // private
        /// <summary>
        /// 每月一个数据库
        /// </summary>
        /// <returns></returns>
        private SQLiteHandle CreatDB()
        {
            string dateDir = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\{Application.ProductName}";
            string dbDir = dateDir + "\\Database";
            string dbName = dbDir + $"\\{DateTime.Now:yyyy-MM}.db";
            if (!Directory.Exists(dateDir))
            {
                Directory.CreateDirectory(dateDir);
            }
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            if (!File.Exists(dbName))
            {
                if (new CreateHandle(dbName).CreateDBFile())
                {
                    string sql = Sentence.CreateTable(new KeyData());
                    var db = new SQLiteHandle(dbName);

                    db.Open();
                    using (SQLiteCommand command = new SQLiteCommand(sql, db.Connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    db.Close();
                    return db;
                }
                else throw new Exception();
            }
            else
            {
                return new SQLiteHandle(dbName);
            }
        }

        // event handle
        /// <summary>
        /// 记录击键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hook_OnKeyUpEvent(object sender, KeyEventArgs e)
        {
            KeyData keyData = new KeyData()
            {
                DateTime = DateTime.Now,
                KeyName = $"{e.KeyCode}",
            };
            var res = db.Insert(keyData);
        }

        // public
        /// <summary>
        /// 开始记录
        /// </summary>
        public void Start()
        {
            if (KeyState == KeyState.Closed)
            {
                keyState = KeyState.Open;
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
                KeyHook.OnKeyUpEvent -= Hook_OnKeyUpEvent;
                KeyStateChanged?.Invoke(this, keyState);
            }
        }

        /// <summary>
        /// 获取指定时间段内，每个键的击键记录数。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Dictionary<Keys, long> Count(DateTime start, DateTime end)
        {
            if (start == end)
            { return new Dictionary<Keys, long>(); }
            var min = start < end ? start : end;
            var max = start > end ? start : end;
            string sql = $"select KeyName,count(*) as Count from vk_key where {"DateTime".Between(min, max)} group by KeyName;";
            using (SQLiteCommand command = new SQLiteCommand(sql, db.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    Dictionary<Keys, long> vals = new Dictionary<Keys, long>();
                    while (reader.Read())
                    {
                        vals.Add((Keys)Enum.Parse(typeof(Keys), (string)reader["KeyName"]), (long)reader["Count"]);
                    }
                    return vals;
                }
            }
        }
        /// <summary>
        /// 获取每个键的所有击键记录数。
        /// </summary>
        /// <returns></returns>
        public Dictionary<Keys, long> Count()
        {
            string sql = $"select KeyName,count(*) as Count from vk_key group by KeyName;";
            using (SQLiteCommand command = new SQLiteCommand(sql, db.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    Dictionary<Keys, long> vals = new Dictionary<Keys, long>();
                    while (reader.Read())
                    {
                        vals.Add((Keys)Enum.Parse(typeof(Keys), (string)reader["KeyName"]), (long)reader["Count"]);
                    }
                    return vals;
                }
            }
        }
        /// <summary>
        /// 获取指定时间段内，指定的键的击键记录数。
        /// </summary>
        /// <param name="keys">指定按键</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public long KeyCount(Keys keys, DateTime start, DateTime end)
        {
            if (start == end)
            { return 0L; }
            var min = start < end ? start : end;
            var max = start > end ? start : end;
            string sql = $"select count(*) as Count from vk_key where {"DateTime".Between(min, max).And("KeyName".EQ(keys))};";
            using (SQLiteCommand command = new SQLiteCommand(sql, db.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    long vals = 0L;
                    while (reader.Read())
                    {
                        vals = (long)reader["Count"];
                    }
                    return vals;
                }
            }
        }
        /// <summary>
        /// 获取指定的键的所有击键记录数。
        /// </summary>
        /// <param name="keys">指定按键</param>
        /// <returns></returns>
        public long KeyCount(Keys keys)
        {
            string sql = $"select count(*) as Count from vk_key where {"KeyName".EQ(keys)};";
            using (SQLiteCommand command = new SQLiteCommand(sql, db.Connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    long vals = 0L;
                    while (reader.Read())
                    {
                        vals = (long)reader["Count"];
                    }
                    return vals;
                }
            }
        }
    }
}
