using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace AFKPatcher.Windows;

public class ConfigWindow : Window, IDisposable
{
    private AFKPatcherConfig AFKPatcherConfig; 
    public static readonly Memory.Replacer InstanceKickerPatch = new("F3 ?? 11 43 ?? 0F ?? ?? F3 ?? 58 43", new byte[2] {144, 144});
    public static readonly Memory.Replacer NoviceNetKickerPatch = new("F3 ?? 11 4B ?? F3 ?? 11 43 ?? 45 33", new byte[5] { 144, 144, 144, 144, 144 });

    public ConfigWindow(Plugin plugin) : base(
        "AFK Patcher Config",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.AFKPatcherConfig = plugin.AFKPatcherConfig;
    }

    public void Dispose() { }

    public override void Draw()
    {
        bool first = true;
        ImGui.Text("Select which timers to patch:");
        ImGui.Indent();

        ReplacerCheckbox(ConfigWindow.InstanceKickerPatch, "Instance AFK Patch", (System.Action)(() =>
        {
            AFKPatcherConfig.InstanceKickerBool = !ConfigWindow.InstanceKickerPatch.IsEnabled;
            AFKPatcherConfig.Save();
        }));

        ReplacerCheckbox(ConfigWindow.NoviceNetKickerPatch, "Novice Network AFK Patch", (System.Action)(() =>
        {
            AFKPatcherConfig.NoviceNetKickerBool = !ConfigWindow.NoviceNetKickerPatch.IsEnabled;
            AFKPatcherConfig.Save();
        }));

        ImGui.Unindent();
        ImGui.Text("All settings auto-save upon change.");

        void ReplacerCheckbox(Memory.Replacer rep, string label, System.Action preAction)
        {
            if (!rep.IsValid)
                return;
            if (!first)
                ImGui.NextColumn();
            bool isEnabled = rep.IsEnabled;
            if (ImGui.Checkbox(label, ref isEnabled))
            {
                if (preAction != null)
                    preAction();
                rep.Toggle();
            }
            first = false;
        }
    }
}
