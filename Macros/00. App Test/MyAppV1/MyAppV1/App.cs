#region Namespaces
using Autodesk.Revit.UI;
using System;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.IO;
#endregion

namespace MyAppV1
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication uiConApp)
        {
            // Method to add Tab and Panel 
            RibbonPanel structure_Panel = null;
            RibbonPanel achitecture_Panel = null;
            RibbonPanel other_Panel = null;

            //Get all ribbonPanel in the tab

            List<RibbonPanel> myAllRibPanel = getRibbonPanels(uiConApp);
            foreach (RibbonPanel myRibPanel in myAllRibPanel)
            {
                if (myRibPanel.Name == "Achitecture")
                {
                    achitecture_Panel = myRibPanel;
                }

                if (myRibPanel.Name == "Structure")
                {
                    structure_Panel = myRibPanel;
                }

                if (myRibPanel.Name == "Other")
                {
                    other_Panel = myRibPanel;
                }

            }
            //Location
            string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;


            //Add my Commands to Panel



            //Other
            //Command 1.1 (name: Increase Value Of parameter)
            PushButtonData increaseParaForm_Button_Data_01 = new PushButtonData("increaseParaForm_Button_Data_01", "(-)Para(+)", thisAssemblyPath, "MyAppV1.IncreaseParameter");
            increaseParaForm_Button_Data_01.ToolTip = "Tăng hoặc giảm dần giá trị parameter ...";
            increaseParaForm_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Increase_Parameter.Size16.png");
            increaseParaForm_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Increase_Parameter.Size32.png");

            //add
            //other_Panel.AddItem(increaseParaForm_Button_Data_01);


            //Command 1.2 (name: Show Element By Value Parameter)
            PushButtonData showElementForm_Button_Data_01 = new PushButtonData("showElementForm_Button_Data_01", "Show by\n Parameter", thisAssemblyPath, "MyAppV1.ShowElementByValueParaAutoCom");
            showElementForm_Button_Data_01.ToolTip = "Show element bằng giá trị parameter...";
            showElementForm_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Show_Element.Show-Size-16.png");
            showElementForm_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Show_Element.Show-Size-32.png");

            //add
            //other_Panel.AddItem(showElementForm_Button_Data_01);



            //Command 1.3 (name: Show Element By Value Parameter)
            PushButtonData selectElementByParaValueForm_Button_Data_01 = new PushButtonData("selectElementByParaValueForm_Button_Data_01", "Select by\n Parameter", thisAssemblyPath, "MyAppV1.SelectByValueParameter");
            selectElementByParaValueForm_Button_Data_01.ToolTip = "Select các element bằng giá trị parameter...";
            selectElementByParaValueForm_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Select_Element.Select-Size-16.png");
            selectElementByParaValueForm_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Select_Element.Select-Size-32.png");

            //add
            //other_Panel.AddItem(selectElementByParaValueForm_Button_Data_01);



            SplitButtonData sb1 = new SplitButtonData("Parameter Tool", "Para Tool");
            SplitButton sb_1 = other_Panel.AddItem(sb1) as SplitButton;
            sb_1.AddPushButton(increaseParaForm_Button_Data_01);
            sb_1.AddPushButton(showElementForm_Button_Data_01);
            sb_1.AddPushButton(selectElementByParaValueForm_Button_Data_01);


            //Add separator
            other_Panel.AddSeparator();


            //Command 2.1 (name: Show Element By Value Parameter)
            PushButtonData quickDimGridOrLevel_Button_Data_01 = new PushButtonData("quickDimGridOrLevel_Button_Data_01", "Quick Dim \n Grids/Level", thisAssemblyPath, "MyAppV1.QuickDimLevelOrGrid");
            quickDimGridOrLevel_Button_Data_01.ToolTip = "Dim nhanh lưới mặt bằng và cao độ";
            quickDimGridOrLevel_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Dimensions.QuickDimGrid-Size-32.png");
            quickDimGridOrLevel_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Dimensions.QuickDimGrid-Size-32.png");

            //add
            //other_Panel.AddItem(quickDimGridOrLevel_Button_Data_01);





            //Command 2.1 (name: Show Element By Value Parameter)
            PushButtonData quickDimGeneral_Button_Data_01 = new PushButtonData("quickDimGeneral_Button_Data_01", "Quick Dim\nby Catergory", thisAssemblyPath, "MyAppV1.QuickDimGeneral");
            quickDimGeneral_Button_Data_01.ToolTip = "Dim nhanh bằng cách chọn các đường vuông góc với Views";
            quickDimGeneral_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Dimensions.Quick-Dim-Size-16.png");
            quickDimGeneral_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Dimensions.Quick-Dim-Size-32.png");

            //add
            //other_Panel.AddItem(quickDimGeneral_Button_Data_01);




            //Command 2.2 (name: LocateColumn)
            PushButtonData quickLocateColumn_Button_Data_01 = new PushButtonData("quickLocateColumn_Button_Data_01", "Locate\nColumn", thisAssemblyPath, "MyAppV1.LocateColumnOnPlan");
            quickLocateColumn_Button_Data_01.ToolTip = "Định vị cột X,Y trên mặt bằng (Dim theo phương X, Y và các lưới) ";
            quickLocateColumn_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Dimensions.Dim-Columns-Size-32.png");
            quickLocateColumn_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Dimensions.Dim-Columns-Size-32.png");

            //add
            //other_Panel.AddItem(quickLocateColumn_Button_Data_01);



            SplitButtonData sb2 = new SplitButtonData("splitButtonDim", "Quick Dim Tool");
            SplitButton sb_2 = other_Panel.AddItem(sb2) as SplitButton;
            sb_2.AddPushButton(quickDimGridOrLevel_Button_Data_01);
            sb_2.AddPushButton(quickDimGeneral_Button_Data_01);
            sb_2.AddPushButton(quickLocateColumn_Button_Data_01);


            //Command 2.3 (name: Dim setting Form)
            PushButtonData dimettingForm_Button_Data_01 = new PushButtonData("dimettingForm_Button_Data_01", "Dim setting", thisAssemblyPath, "MyAppV1.DimenisonGroupSettingForm");
            dimettingForm_Button_Data_01.ToolTip = "Chỉnh sửa thông số của dimension tools ";
            dimettingForm_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Dimensions.Dim-Setting-Form-32.png");
            dimettingForm_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Dimensions.Dim-Setting-Form-32.png");

            //add
            other_Panel.AddItem(dimettingForm_Button_Data_01);



            //Command 2.4 (name: Edit dimension Value Form)
            PushButtonData editDimValue_Button_Data_01 = new PushButtonData("editDimValue_Button_Data_01", "Edit/Reset \n Dimensions", thisAssemblyPath, "MyAppV1.EditDimensionValue");
            editDimValue_Button_Data_01.ToolTip = "Chỉnh sửa giá trị hiển thị của Dimension, AngularDimension";
            editDimValue_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Dimensions.Edit-Dim-Size-32.png");
            editDimValue_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Dimensions.Edit-Dim-Size-32.png");

            //add
            other_Panel.AddItem(editDimValue_Button_Data_01);



            //Add separator
            other_Panel.AddSeparator();

            //Command 3.1 (name: Logo Link)
            PushButtonData logoLink_Button_Data_01 = new PushButtonData("logoLink_Button_Data_01", @"INAECJSC", thisAssemblyPath, "MyAppV1.GotoWebSite");
            logoLink_Button_Data_01.ToolTip = "Go to INAECJSC.VN";
            logoLink_Button_Data_01.Image = PngImageSource("MyAppV1.Resources.Logo_INAEC.logo-Size-16.png");
            logoLink_Button_Data_01.LargeImage = PngImageSource("MyAppV1.Resources.Logo_INAEC.logo-Size-32.png");

            
            //add
            other_Panel.AddItem(logoLink_Button_Data_01);

            //App
            uiConApp.ApplicationClosing += a_ApplicationClosing;
            //Set Application to Idling
            uiConApp.Idling += a_Idling;

            return Result.Succeeded;
        }

        //*****************************a_Idling()*****************************
        void a_Idling(object sender, Autodesk.Revit.UI.Events.IdlingEventArgs e)
        {

        }

        //*****************************a_ApplicationClosing()*****************************
        void a_ApplicationClosing(object sender, Autodesk.Revit.UI.Events.ApplicationClosingEventArgs e)
        {
            throw new NotImplementedException();
        }


        //*****************************Get all ribbonPanel()*****************************[Guid("C0A2E940-6C2C-41C3-8141-407A098CC19F")]
        public List<RibbonPanel> getRibbonPanels(UIControlledApplication myAddin)
        {
            string INAEC_tab = "INAEC"; // Tab name
            // Empty ribbon panel 

            // Try to create ribbon tab. 
            try
            {
                myAddin.CreateRibbonTab(INAEC_tab);
            }
            catch { }
            // Try to create ribbon panel.
            try
            {
                RibbonPanel architecture_Panel = myAddin.CreateRibbonPanel(INAEC_tab, "Achitecture");
                RibbonPanel structure_Panel = myAddin.CreateRibbonPanel(INAEC_tab, "Structure");
                RibbonPanel other_Panel = myAddin.CreateRibbonPanel(INAEC_tab, "Other");
            }
            catch { }
            // Search existing tab for your panel.
            List<RibbonPanel> myListRibbonPanels = myAddin.GetRibbonPanels(INAEC_tab);
            //return panel 
            return myListRibbonPanels;
        }




        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }



        private System.Windows.Media.ImageSource PngImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new System.Windows.Media.Imaging.PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return decoder.Frames[0];
        }


        private System.Windows.Media.ImageSource BmpImageSource(string embeddedPath)
        {
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(embeddedPath);
            var decoder = new System.Windows.Media.Imaging.BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            return decoder.Frames[0];
        }


    }
}