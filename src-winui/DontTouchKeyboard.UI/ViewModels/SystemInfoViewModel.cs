using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;

namespace DontTouchKeyboard.UI.ViewModels;

internal class SystemInfoViewModel : ObservableObject
{

    private ObservableCollection<SystemInfoModel> systemInfos = new();

    public ObservableCollection<SystemInfoModel> SystemInfos
    {
        get => systemInfos; set => SetProperty(ref systemInfos, value);
    }

    public SystemInfoViewModel()
    {
        SystemInfos.CollectionChanged += async (s, e) =>
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems?[0] is SystemInfoModel info)
                {
                    await Task.Delay(3000);
                    if (info.Level != InfoBarSeverity.Error)
                    {
                        SystemInfos.Remove(info);
                    }
                }
            }
        };
    }
}

internal class SystemInfoModel : ObservableObject
{
    private string title;
    private string message;
    private InfoBarSeverity level = InfoBarSeverity.Informational;

    private bool isHandle = false;
    private string detailContent = "详细";
    private Visibility isShowDetail = Visibility.Collapsed;

    public SystemInfoModel(string title, string message, InfoBarSeverity level)
    {
        Title = title;
        Message = message;
        Level = level;
        IsHandle = false;
        IsDetail = false;
    }

    public SystemInfoModel(Exception ex, InfoBarSeverity level = InfoBarSeverity.Error)
        : this("出现一个错误", ex.Message, level)
    {
        Exception = ex;
    }

    public SystemInfoModel(UnhandledExceptionEventArgs args, InfoBarSeverity level = InfoBarSeverity.Error)
        : this(args.Exception, level)
    {
        args.Handled = true;
    }

    public Exception Exception { get; }

    public string Title { get => title; set => SetProperty(ref title, value); }
    public string Message { get => message; set => SetProperty(ref message, value); }
    public InfoBarSeverity Level { get => level; set => SetProperty(ref level, value); }

    public DateTimeOffset Create { get; }

    public bool IsHandle
    {
        get => isHandle; set => SetProperty(ref isHandle, value);
    }

    public bool IsDetail { get; set; }
    public string DetailContent { get => detailContent; set => SetProperty(ref detailContent, value); }
    public Visibility IsShowDetail { get => isShowDetail; set => SetProperty(ref isShowDetail, value); }

    public ICommand HandleCommand => new RelayCommand(() =>
    {
        IsHandle = true;
        Ioc.Default.GetService<SystemInfoViewModel>()?.SystemInfos.Remove(this);
    });

    private void SetInfo()
    {

    }
}
