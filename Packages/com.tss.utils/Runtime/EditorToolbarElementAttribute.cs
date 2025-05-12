using System;
using System.Diagnostics;

namespace TSS.Utils
{
    public enum EToolbarAlign
    {
        Left,
        Right
    }

    public enum EToolbarPanel
    {
        LeftPanel,
        RightPanel
    }

    [Conditional("UNITY_EDITOR")] 
    public class EditorToolbarElementAttribute : Attribute
    {
        public int Order { get; set; }
        public EToolbarPanel ToolbarPanel { get; }
        public EToolbarAlign ToolbarAlign { get; }
        
        public EditorToolbarElementAttribute(EToolbarPanel toolbarPanel, EToolbarAlign toolbarAlign)
        {
            ToolbarPanel = toolbarPanel;
            ToolbarAlign = toolbarAlign;
        }
    }
}