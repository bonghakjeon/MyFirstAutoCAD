using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Document = Autodesk.AutoCAD.ApplicationServices.Document;

[assembly: CommandClass(typeof(WinformPalette.Commands))]

// TODO : AutoCAD2025 애드인 프로그램 WinformPalette C# Winform 기술로 Palette 화면 구현 (Revit 애드인 프로그램 DockableDialogs와 비슷한 화면) (2024.11.08 jbh)
// 참고 URL - https://gilecad.azurewebsites.net/UserInterfaces_en.aspx#modalWPF

namespace WinformPalette
{
    public class Commands
    {
        // static field
        static CustomPaletteSet palette;

        // instance fields (default values)
        double radius = 10.0;
        string layer;

        /// <summary>
        /// 테스트용 Palette 화면 출력 Command 
        /// Command to show the palette.
        /// </summary>
        [CommandMethod("CMD_PALETTE")]
        public void ShowPaletteSet()
        {
            // creation of the palette at the first call of the command
            if(palette == null)
            {
                palette = new CustomPaletteSet();
                palette.Size = new Size(300, 768);

                // TODO : 테스트용 Palette 화면 출력시 AutoCAD 도면 파일(확장자 - .dwg) 우측에 Docking 처리 되도록 구현 (2024.11.11 jbh)
                // 참고 URL - https://forums.autodesk.com/t5/net/how-to-dock-a-new-palette-to-the-right-of-the-properties-palette/m-p/6451680
                palette.Dock = DockSides.Right;

                // TODO : 아래 주석친 테스트 코드 필요시 참고 (2024.11.11 jbh)
                // 참고 URL - https://forums.autodesk.com/t5/net/how-to-dock-a-new-palette-to-the-right-of-the-properties-palette/m-p/6451680
                // palette.Dock = DockSides.Right;
                // palette.RolledUp = true;
                // palette.RolledUp = false;
                // palette.Dock = DockSides.Left;
                // palette.RolledUp = false;
            }
            palette.Visible = true;
        }

        #region 팔레트 만들기

        // TODO : 필요시 팔레트 만들기 Command 메서드 "CreatePaletteReqeust" 참고 (2024.11.11 jbh)
        //[CommandMethod("CB_CREPALREQ", CommandFlags.Session)]
        //public void CreatePaletteReqeust()
        //{
        //    ucRequest page1 = new ucRequest();
        //    bool paladd = true;
        //    if (PS == null)
        //    {
        //        PS = new PaletteSet("CADBOX");

        //        PS.Add("요청사항", page1);

        //        PS.Size = new System.Drawing.Size(300, 768);
        //        PS.Visible = true;
        //        PS.Dock = DockSides.Right;
        //    }
        //    else
        //    {
        //        for(int i = 0; i < PS.Count; i++)
        //        {
        //            if(PS[i].Name == "요청사항")
        //            {
        //                paladd = false;
        //                PS.Activate(i);
        //            }
        //        }

        //        if (paladd == true)
        //        {
        //            PS.Add("요청사항", page1);
        //            for (int i = 0; i < PS.Count; i++)
        //            {
        //                if (PS[i].Name == "요청사항")
        //                {
        //                    PS.Activate(i);
        //                }
        //            }
        //        }
        //        PS.Visible = true;
        //    }
        //}

        #endregion 팔레트 만들기

        /// <summary>
        /// Command to draw the circle
        /// </summary>
        [CommandMethod("CMD_CIRCLE_PALETTE", CommandFlags.Modal)]
        public void DrawCircleCmd()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;

            // choosethe layer
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
                // drawing the circle in the current space
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    var curSpace =
                        (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                    var ucs = ed.CurrentUserCoordinateSystem;
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
