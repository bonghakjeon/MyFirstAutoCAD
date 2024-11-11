using System;
using System.Drawing;
using System.Windows.Forms;

using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Windows.Data;

using AcAp = Autodesk.AutoCAD.ApplicationServices.Application;

// TODO : AutoCAD2025 애드인 프로그램에 C# Winform 기술로 Palette 화면 구현 (Revit 애드인 프로그램 DockableDialogs와 비슷한 화면) (2024.11.08 jbh)
// 참고 URL - https://gilecad.azurewebsites.net/UserInterfaces_en.aspx#modalWPF

namespace WinformPalette
{
    /// <summary>
    /// Describes the tab (palette) of a palette set (PaletteSet)
    /// </summary>
    public partial class PaletteTab : UserControl
    {
        // instance fields
        double radius;
        DataItemCollection layers; // layer data collection

        /// <summary>
        /// Creates a new instance of PaletteTab.
        /// Defines the data bindings for the ComboBox control.
        /// </summary>
        public PaletteTab()
        {
            InitializeComponent();

            layers = layers = AcAp.UIBindings.Collections.Layers; ;

            // binds the layer data to the ComboBox control
            BindData();

            // updates the ComboBox control when the layer data collection changes
            layers.CollectionChanged += (s, e) => BindData();

            // radius default value
            // txtRadius.Text = "10";
        }

        /// <summary>
        /// Handles the 'TextChanged' event of the 'Radius' TextBox.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private void txtRadius_TextChanged(object sender, EventArgs e)
        {
            // OK button is 'disable' if the text does not represent a valid number
            // the radius field is updated accordingly
            // btnOk.Enabled = double.TryParse(txtRadius.Text, out radius);
        }

        // <summary>
        /// Handles the 'Click' event of the 'Radius' button.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private void btnRadius_Click(object sender, EventArgs e)
        {
            // set the focus to AutoCAD editor
            // before AutoCAD 2015, use : Autodesk.AutoCAD.Internal.Utils.SetFocusToDwgView();
            AcAp.MainWindow.Focus();

            // prompt the user to specify a distance
            var ed = AcAp.DocumentManager.MdiActiveDocument.Editor;
            var opts = new PromptDistanceOptions("\nSpecify the radius: ");
            opts.AllowNegative = false;
            opts.AllowZero = false;
            var pdr = ed.GetDistance(opts);
            //if (pdr.Status == PromptStatus.OK) /
                // txtRadius.Text = pdr.Value.ToString();
        }

        /// <summary>
        /// Handles the 'Click' event of the 'OK' button.
        /// </summary>
        /// <param name="sender">Event source.</param>
        /// <param name="e">Event data.</param>
        private async void btnOk_Click(object sender, EventArgs e)
        {
            var docs = AcAp.DocumentManager;
            var ed = docs.MdiActiveDocument.Editor;
            await docs.ExecuteInCommandContextAsync(
                async (ojb) =>
                {
                    // calling the 'CMD_CIRCLE_PALETTE' command with the layer and radius
                    // await ed.CommandAsync("CMD_CIRCLE_PALETTE", ((INamedValue)cbxLayer.SelectedItem).Name, radius);
                },
                null);
        }
        // With versions prior to AutoCAD 2016, use Document.SendStringToExecute.
        //private void btnOk_Click(object sender, EventArgs e)
        //{
        //    // calling the 'CMD_CIRCLE_PALETTE' command with the layer and radius
        //    AcAp.DocumentManager.MdiActiveDocument?.SendStringToExecute(
        //       $"CMD_CIRCLE_PALETTE \"{((INamedValue)cbxLayer.SelectedItem).Name}\" {radius} ", false, false, false);
        //}

        /// <summary>
        /// Defines the data bindings of the Combobox control.
        /// </summary>
        private void BindData()
        {
            // binding to data source
            // cbxLayer.DataSource = new BindingSource(layers, null);

            // definition of the drop down lsit items appearance
            //cbxLayer.DrawMode = DrawMode.OwnerDrawFixed;
            //cbxLayer.DrawItem += (_, e) =>
            //{
            //    if (e.Index > -1)
            //    {
            //        // get the item and its properties
            //        var item = layers[e.Index];
            //        var properties = item.GetProperties();

            //        // draw a square with the layer color
            //        var color = (Autodesk.AutoCAD.Colors.Color)properties["Color"].GetValue(item);
            //        var bounds = e.Bounds;
            //        int height = bounds.Height;
            //        Graphics graphics = e.Graphics;
            //        e.DrawBackground();
            //        var rect = new Rectangle(bounds.Left + 4, bounds.Top + 4, height - 8, height - 8);
            //        graphics.FillRectangle(new SolidBrush(color.ColorValue), rect);
            //        graphics.DrawRectangle(new Pen(Color.Black), rect);

            //        // write the layer name
            //        graphics.DrawString(
            //            (string)properties["Name"].GetValue(item),
            //            e.Font,
            //            new SolidBrush(e.ForeColor), bounds.Left + height, bounds.Top + 1);
            //        e.DrawFocusRectangle();
            //    }
            //};

            //// select the current layer
            //cbxLayer.SelectedItem = layers.CurrentItem;
        }
    }
}
