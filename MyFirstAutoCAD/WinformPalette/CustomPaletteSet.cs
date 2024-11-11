
using System;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Windows;

// TODO : AutoCAD2025 애드인 프로그램에 C# Winform 기술로 Palette 화면 구현 (Revit 애드인 프로그램 DockableDialogs와 비슷한 화면) (2024.11.08 jbh)
// 참고 URL - https://gilecad.azurewebsites.net/UserInterfaces_en.aspx#modalWPF

namespace WinformPalette
{
    internal class CustomPaletteSet : PaletteSet
    {
        // static field
        static bool wasVisible;

        /// <summary>
        /// Creates a new instance of CustomPaletteSet.
        /// </summary>
        public CustomPaletteSet()
            : base("Palette", "CMD_PALETTE", new Guid("{1836C7AC-C70E-4CF7-AA05-F6298D275046}"))
        {
            Style =
                PaletteSetStyles.ShowAutoHideButton |
                PaletteSetStyles.ShowCloseButton |
                PaletteSetStyles.ShowPropertiesMenu;
            MinimumSize = new Size(250, 150);
            Add("Circle", new PaletteTab());

            // automatically hide the palette while none document is active (no document state)
            var docs = Application.DocumentManager;
            docs.DocumentBecameCurrent += (s, e) => Visible = e.Document == null ? false : wasVisible;
            docs.DocumentCreated += (s, e) => Visible = wasVisible;
            docs.DocumentToBeDeactivated += (s, e) => wasVisible = Visible;
            docs.DocumentToBeDestroyed += (s, e) =>
            {
                wasVisible = Visible;
                if (docs.Count == 1)
                    Visible = false;
            };
        }
    }
}
