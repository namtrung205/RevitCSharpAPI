using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
// Lib Revit API
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;

namespace MakeGroupRebarByHost
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class MakeGroupRebarByHost : IExternalCommand
    {
        // Set [Guid("25599A30-6E82-4E37-AA55-D67152A9E0D3")]
        static AddInId appId = new AddInId(new Guid("25599A30-6E82-4E37-AA55-D67152A9E0D3"));

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Thưc thi hàm createElevationByRooms

            try
            {
                makeGroupRebarByHost(commandData.Application.ActiveUIDocument);
                return Result.Succeeded;
            }

            catch
            {
                //TaskDialog.Show("Error", "OK ");
                return Result.Succeeded;

            }
        }



        public void makeGroupRebarByHost(UIDocument uiDoc)
        {
            try
            {
                Document doc = uiDoc.Document;

                string nameGroup = "";


                string paraName_1 = "";
                string paraName_2 = "";


                string paraValue_1 = "";
                int paraValue_2 = 1;


                string warning1 = "";
                string warning2 = "";

                // Show form

                using (SettingDialog myInputFormSetting = new SettingDialog())
                {
                    myInputFormSetting.ShowDialog();

                    //if the user hits cancel just drop out of macro
                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.Cancel) return;
                    {
                        //else do all this :)    
                        myInputFormSetting.Close();
                    }

                    if (myInputFormSetting.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        nameGroup = myInputFormSetting.GroupName_Tb1.Text;


                        paraName_1 = myInputFormSetting.Para_1Tb.Text;
                        paraName_2 = myInputFormSetting.Para_2Tb.Text;

                        paraValue_1 = myInputFormSetting.Value1_Tb.Text;

                        paraValue_2 = Convert.ToInt32(myInputFormSetting.Value2_Tb.Text);

                        myInputFormSetting.Close();
                    }
                }



                //List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByNumberRebarHostIn()).ToList();

                Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByNumberRebarHostIn());


                Element myHost = doc.GetElement(myRef);

                RebarHostData myRbHostData = RebarHostData.GetRebarHostData(myHost);

                List<Rebar> myListRebar = myRbHostData.GetRebarsInHost() as List<Rebar>;


                using (Transaction trans_1 = new Transaction(doc, "Change parameter Rebar"))
                {
                    trans_1.Start();



                    //Set parameter
                    foreach (Rebar myRebar in myListRebar)
                    {

                        //Partition
                        Parameter Para1 = myRebar.LookupParameter(paraName_1);

                        if (Para1 == null)
                        {
                            warning1 = string.Format("Rebar {0} has no paramater name: {1}", myRebar.Id.ToString(), paraName_1);
                        }
                        else
                        {
                            Para1.Set(paraValue_1);
                        }


                        //Partition
                        Parameter Para2 = myRebar.LookupParameter(paraName_2);

                        if (Para2 == null)
                        {
                            warning2 = string.Format("Rebar {0} has no paramater name: {1}", myRebar.Id.ToString(), paraName_2);
                        }
                        else
                        {
                            Para2.Set(paraValue_2);
                        }

                    }
                    trans_1.Commit();
                }

                if (warning1 != warning2)
                {
                    TaskDialog.Show("Waring", warning1 + "\n" + warning2);
                }


                //Set parameter

                List<ElementId> myListElementId = new List<ElementId>();
                foreach (Rebar myRebar in myListRebar)
                {
                    myListElementId.Add(myRebar.Id);
                }


                using (Transaction trans = new Transaction(doc, "Make group Rebar"))
                {
                    trans.Start();

                    if (myListElementId.Count > 0)
                    {
                        Group myGroupRebar = doc.Create.NewGroup(myListElementId);
                        if (nameGroup != "")
                        {
                            myGroupRebar.GroupType.Name = nameGroup;
                        }


                    }
                    else
                    {
                        TaskDialog.Show("Warning!", "No rebar was hosted by this element, so no any group was created!");
                    }
                    trans.Commit();
                }

            }
            catch (Exception)
            {
                TaskDialog.Show("Error!!!", "Co loi xay ra, co the tat ca rebar da co trong 1 group");
                throw;
            }
        }


        public class FilterByNumberRebarHostIn : ISelectionFilter
        {


            public bool AllowElement(Element e)
            {
                RebarHostData myRbHostData = RebarHostData.GetRebarHostData(e);

                if (myRbHostData.GetRebarsInHost().Count > 0)
                {
                    return true;
                }

                return false;
            }


            public bool AllowReference(Reference r, XYZ myXYZ)
            {
                return true;
            }

        }



        //By name type of class. Ex: "AngularDimension", Wall

        public class FilterByNameElementType : ISelectionFilter
        {

            //Cac bien thanh vien
            List<string> myListNameFilter = new List<string>();

            // Bo khoi dung
            public FilterByNameElementType(List<string> myListName)
            {
                myListNameFilter = myListName;
            }

            public bool AllowElement(Element e)
            {
                string typeE = e.GetType().Name.ToString();

                if (this.myListNameFilter.Contains(typeE))
                {
                    return true;
                }

                return false;
            }


            public bool AllowReference(Reference r, XYZ myXYZ)
            {
                return true;
            }

        }


        public class FilterByIdCategory : ISelectionFilter
        {
            //Cac bien thanh vien
            List<int> ListIdCategory = new List<int>();


            // Bo khoi dung
            public FilterByIdCategory(List<int> myListIdCategory)
            {
                this.ListIdCategory = myListIdCategory;
            }

            public bool AllowElement(Element e)
            {
                int categoryE = e.Category.Id.IntegerValue;

                if (this.ListIdCategory.Contains(categoryE))
                {
                    return true;
                }
                return false;
            }

            public bool AllowReference(Reference r, XYZ myXYZ)
            {
                return true;
            }

        }


    }
}
