using System;
using System.Drawing;
using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.Windows;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;

// TODO : AutoCAD2025 애드인 프로그램 WpfPalette C# Wpf 기술로 Palette 화면 구현 (Revit 애드인 프로그램 DockableDialogs와 비슷한 화면) (2024.11.12 jbh)
// 참고 URL - https://gilecad.azurewebsites.net/UserInterfaces_en.aspx#modalWPF

namespace WpfPalette
{
    // AutoCAD API 클래스 PaletteSet.cs 역할 - Revit DockableDialogs 클래스 DockablePaneProviderData 와 비슷한 기능 
    // AutoCAD API 클래스 CustomPaletteSet.cs 역할 - Revit DockableDialogs 클래스 DockPageProvider 와 비슷한 기능 

    internal class CustomPaletteSet : PaletteSet
    {
        // static field
        static bool wasVisible;

        /// <summary>
        /// Creates a new instance of CustomPaletteSet.
        /// </summary>
        public CustomPaletteSet()
            : base("Palette WPF", "CMD_PALETTE_WPF", new Guid("{42425FEE-B3FD-4776-8090-DB857E9F7A0E}"))
        {
            Style =
                PaletteSetStyles.ShowAutoHideButton |
                PaletteSetStyles.ShowCloseButton |
                PaletteSetStyles.ShowPropertiesMenu;
            MinimumSize = new Size(250, 150);
            AddVisual("Circle", new PaletteTabView());

            // automatically hide the palette while none document is active (no document state)
            var docs = Application.DocumentManager;
            docs.DocumentBecameCurrent += (s, e) => // 문서 변경 
                Visible = e.Document == null ? false : wasVisible;
            docs.DocumentActivated += (s, e) => Visible = wasVisible;  //문서 활성화
            docs.DocumentCreated += (s, e) => // 문서 새로 생성 
                Visible = wasVisible;
            docs.DocumentCreated += (s, e) =>
                Visible = wasVisible;
            docs.DocumentToBeDeactivated += (s, e) => // 문서 비활성화
                wasVisible = Visible;
            docs.DocumentToBeDestroyed += (s, e) => // 문서 파괴 
            {
                wasVisible = Visible;
                if (docs.Count == 1)
                    Visible = false;
            };
        }
    }
}
