namespace Keyboard.HeatMap.Models;

/// <summary>
/// 按键数据
/// </summary>
[Table("vk_key")]
public class KeyData
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 键码
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// 击键时间
    /// </summary>
    public DateTime Time { get; set; }
    /// <summary>
    /// 击键状态
    /// <para>1:按下 , 2:抬起</para>
    /// </summary>
    public int Status { get; set; }
}

public class KeyDataOld
{
    /// <summary>
    /// 主键
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 按键名
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 键码
    /// </summary>
    public int Code { get; set; }
    /// <summary>
    /// 击键时间
    /// </summary>
    public DateTime Time { get; set; }
    /// <summary>
    /// 击键状态
    /// <para>1:按下 , 2:抬起</para>
    /// </summary>
    public int Status { get; set; }
}
