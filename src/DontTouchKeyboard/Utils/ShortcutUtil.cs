using IWshRuntimeLibrary;
using File = System.IO.File;

namespace DontTouchKeyboard.Utils;

public static class ShortcutUtil
{
    /// <summary>
    /// 在指定位置上创建快捷方式
    /// </summary>
    /// <param name="type">快捷方式类型</param>
    /// <param name="productName">快捷方式名称</param>
    /// <param name="executablePath">可执行文件路径</param>
    /// <param name="description">描述</param>
    /// <param name="arguments">参数</param>
    /// <param name="hotKey">快捷键</param>
    /// <returns>成功或失败</returns>
    public static bool CreateShortcut(ShortcutType type, string productName, string executablePath, string? description = null, string? arguments = null, string? hotKey = null)
    {
        if (GetShortcut(type, productName))
        { return true; }
        try
        {
            string linkFileName = Path.Combine(type.ShortcutFolder(), productName.ShortcutFileName());
            if (File.Exists(linkFileName)) { File.Delete(linkFileName); }
            IWshShortcut shortcut = (IWshShortcut)new WshShell().CreateShortcut(linkFileName);
            shortcut.TargetPath = executablePath;
            shortcut.IconLocation = executablePath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(executablePath);
            shortcut.WindowStyle = 1; //普通(1)、最大化(3)、最小化(7)
            shortcut.Description = description ?? productName;
            shortcut.Arguments = arguments ?? "";
            shortcut.Hotkey = hotKey ?? shortcut.Hotkey;
            shortcut.Save();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 获取指定位置上是否已经有快捷方式
    /// </summary>
    /// <param name="type">快捷方式类型</param>
    /// <param name="productName">快捷方式名称</param>
    /// <returns>有或无</returns>
    public static bool GetShortcut(ShortcutType type, string productName) => File.Exists(Path.Combine(type.ShortcutFolder(), productName.ShortcutFileName()));

    /// <summary>
    /// 移除指定位置上的快捷方式
    /// </summary>
    /// <param name="type">快捷方式类型</param>
    /// <param name="productName">快捷方式名称</param>
    /// <returns></returns>
    public static bool RemoveShortcut(ShortcutType type, string productName)
    {
        try
        {
            File.Delete(Path.Combine(type.ShortcutFolder(), productName.ShortcutFileName()));
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string ShortcutFolder(this ShortcutType type) => Environment.GetFolderPath(type switch { ShortcutType.Desktop => Environment.SpecialFolder.Desktop, _ => Environment.SpecialFolder.Startup, });

    private static string ShortcutFileName(this string productName) => productName.EndsWith(".lnk") ? productName : $"{productName}.lnk";
}
