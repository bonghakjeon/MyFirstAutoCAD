using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Document = Autodesk.AutoCAD.ApplicationServices.Document;

// TODO : AutoCAD2025 애드인 프로그램 WpfPalette C# Wpf 기술로 Palette 화면 구현 (Revit 애드인 프로그램 DockableDialogs와 비슷한 화면) (2024.11.12 jbh)
// 참고 URL - https://gilecad.azurewebsites.net/UserInterfaces_en.aspx#modalWPF

[assembly: CommandClass(typeof(WpfPalette.Commands))]

namespace WpfPalette
{
    public class Commands
    {
        // static field
        static CustomPaletteSet palette;

        // instance fields (default values)
        double radius = 10.0;
        string layer;

        /// <summary>
        /// Command to show the palette.
        /// </summary>
        [CommandMethod("CMD_PALETTE_WPF")]
        public void ShowPaletteSetWpf()
        {
            if(palette == null)
            {
                palette = new CustomPaletteSet();

                // TODO : 테스트용 Palette 화면 출력시 AutoCAD 도면 파일(확장자 - .dwg) 우측에 Docking 처리 되도록 구현 (2024.11.11 jbh)
                // 참고 URL - https://forums.autodesk.com/t5/net/how-to-dock-a-new-palette-to-the-right-of-the-properties-palette/m-p/6451680
                palette.Dock = DockSides.Right;
                // palette.RolledUp = true;
                // palette.RolledUp = false;
                // palette.AutoRollUp = false;
                // palette.AutoRollUp = true;
            }
            // palette.Visible = true; 처리하고 나서 palette.RolledUp = false; 처리해야 Pallete 화면이 숨김 처리(RolledUp) 처리 되지 않고 정상 출력됨.
            palette.Visible = true;
            palette.RolledUp = false;
        }

        /// <summary>
        /// Command to draw the circle.
        /// </summary>
        [CommandMethod("CMD_CIRCLE_WPF")]
        public void DrawCircleCmd()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            // choose the layer
            if (string.IsNullOrEmpty(layer))
                layer = (string)Application.GetSystemVariable("clayer");
            var strOptions = new PromptStringOptions("\nLayer name: ");
            strOptions.DefaultValue = layer;
            strOptions.UseDefaultValue = true;
            var strResult = ed.GetString(strOptions);
            if (strResult.Status != PromptStatus.OK)
                return;
            layer = strResult.StringResult;

            // specify the radius
            var distOptions = new PromptDistanceOptions("\nSpecify the radius: ");
            distOptions.DefaultValue = radius;
            distOptions.UseDefaultValue = true;
            var distResult = ed.GetDistance(distOptions);
            if (distResult.Status != PromptStatus.OK)
                return;
            radius = distResult.Value;

            // specify the center
            var ppr = ed.GetPoint("\nSpecify the center: ");
            if (ppr.Status == PromptStatus.OK)
            {
                // draw the circle in current space
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var curSpace =
                        (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    using (var circle = new Circle(ppr.Value, Vector3d.ZAxis, distResult.Value))
                    {
                        circle.TransformBy(ed.CurrentUserCoordinateSystem);
                        circle.Layer = strResult.StringResult;
                        curSpace.AppendEntity(circle);
                        tr.AddNewlyCreatedDBObject(circle, true);
                    }
                    tr.Commit();
                }
            }
        }
    }
}
