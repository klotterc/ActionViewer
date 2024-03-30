namespace ActionSnoop.Windows;

// Modified from https://github.com/NotNite/ReSanctuary/blob/main/ReSanctuary/Windows/MainWindowTab.cs
public abstract class MainWindowTab
{
    public readonly string Name;
    public readonly Plugin Plugin;

    protected MainWindowTab(string name, Plugin plugin)
    {
        this.Name = name;
        this.Plugin = plugin;
    }

    public abstract void Draw();
}