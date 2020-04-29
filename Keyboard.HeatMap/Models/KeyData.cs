using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using View.SQL;

namespace Keyboard.HeatMap.Models
{
    /// <summary>
    /// 按键数据
    /// </summary>
    public class KeyData : IModel
    {
        public string TableName => "vk_key";
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }
        /// <summary>
        /// 按键名
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// 击键时间
        /// </summary>
        public DateTime DateTime { get; set; }
    }
}
