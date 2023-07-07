using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Keyboard.HeatMap.Owner;

public class Shortcut
{
    private string productName;
    private string exePath;
    /// <summary>
    /// 初始化快捷方式构造器
    /// </summary>
    public Shortcut()
    {
        productName = System.Windows.Forms.Application.ProductName;
        exePath = System.Windows.Forms.Application.ExecutablePath;
    }
    /// <summary>
    /// 初始化快捷方式
    /// </summary>
    /// <param name="productName"></param>
    /// <param name="exePath"></param>
    public Shortcut(string productName)
    {
        this.productName = productName;
        exePath = System.Windows.Forms.Application.ExecutablePath;
    }

    public string ProductName { get => productName; }

    /// <summary>
    /// 在指定位置上创建快捷方式
    /// </summary>
    /// <param name="productName">文件的名称</param>
    /// <param name="exePath">EXE的路径</param>
    /// <returns>成功或失败</returns>
    public bool CreateShortcut(ShortcutType type)
    {
        if (GetShortcut(type))
        { return true; }
        try
        {
            string shortCutPath;
            if (type == ShortcutType.Desktop)
            { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"; }
            else
            { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\"; }
            if (File.Exists(shortCutPath + productName + ".lnk"))  //
            {
                File.Delete(shortCutPath + productName + ".lnk");//删除原来的桌面快捷键方式
            }
            WshShell shell = new WshShell();

            //快捷键方式创建的位置、名称
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortCutPath + productName + ".lnk");
            shortcut.TargetPath = exePath; //目标文件
                                           //该属性指定应用程序的工作目录，当用户没有指定一个具体的目录时，快捷方式的目标应用程序将使用该属性所指定的目录来装载或保存文件。
            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath);
            shortcut.WindowStyle = 1; //目标应用程序的窗口状态分为普通、最大化、最小化【1,3,7】
            shortcut.Description = productName; //描述
            shortcut.IconLocation = exePath;  //快捷方式图标
            shortcut.Arguments = "";
            //shortcut.Hotkey = "CTRL+ALT+F11"; // 快捷键
            shortcut.Save(); //必须调用保存快捷才成创建成功
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
    /// <param name="type"></param>
    /// <returns></returns>
    public bool GetShortcut(ShortcutType type)
    {
        string shortCutPath;
        if (type == ShortcutType.Desktop)
        { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"; }
        else
        { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\"; }

        return File.Exists(shortCutPath + productName + ".lnk");
    }

    public bool RemoveShortcut(ShortcutType type)
    {
        string shortCutPath;
        if (type == ShortcutType.Desktop)
        { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\"; }
        else
        { shortCutPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "\\"; }
        try
        {
            File.Delete(shortCutPath + productName + ".lnk");
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public enum ShortcutType
{
    /// <summary>
    /// 桌面快捷方式
    /// </summary>
    Desktop,
    /// <summary>
    /// 自启快捷方式
    /// </summary>
    Startup,
}
