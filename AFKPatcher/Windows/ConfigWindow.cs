using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace AFKPatcher.Windows;

public class ConfigWindow : Window, IDisposable
{
    private AFKPatcherConfig AFKPatcherConfig;

    public ConfigWindow(Plugin plugin) : base(
        "A Wonderful AFKPatcherConfig Window",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Size = new Vector2(232, 75);
        this.SizeCondition = ImGuiCond.Always;

        this.AFKPatcherConfig = plugin.AFKPatcherConfig;
    }

    public void Dispose() { }

    public override void Draw()
    {
        // can't ref a property, so use a local copy
        var configValue = this.AFKPatcherConfig.SomePropertyToBeSavedAndWithADefault;
        if (ImGui.Checkbox("Random Config Bool", ref configValue))
        {
            this.AFKPatcherConfig.SomePropertyToBeSavedAndWithADefault = configValue;
            // can save immediately on change, if you don't want to provide a "Save and Close" button
            this.AFKPatcherConfig.Save();
        }
    }
}
