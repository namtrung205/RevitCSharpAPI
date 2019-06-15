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

namespace AutoDimLevelOrGrid
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AutoDimLevelOrGrid : IExternalCommand
    {
        // Set GUID [Guid("CF9438F2-87AC-4C8E-A359-9298D7AE1F1F")]
        static AddInId appId = new AddInId(new Guid("CF9438F2-87AC-4C8E-A359-9298D7AE1F1F"));

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
            dimGridsOrLevels(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }


        private void dimGridsOrLevels(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            // Filter by name 

            FilterByNameElementType myFilter = new FilterByNameElementType(new List<string>() { "Grid", "Level" });

            // Select Grids		
            // S1: Pick an element
            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myFilter,
                                                                    "Pick Grids or Levels....") as List<Reference>;

            // Group Type Referent
            List<Reference> listRef_Level = new List<Reference>();
            List<Reference> listRef_Grid = new List<Reference>();

            foreach (Reference myRefChecking in myListRef)
            {
                if (doc.GetElement(myRefChecking) is Level)
                {
                    listRef_Level.Add(myRefChecking);
                }
                else
                {
                    listRef_Grid.Add(myRefChecking);
                }
            }


            if (listRef_Grid.Count >= 2)
            {
                dimGrids_Para(uiDoc, listRef_Grid);
            }
            if (listRef_Level.Count >= 2)
            {
                dimLevels_Para(uiDoc, listRef_Level);
            }

        }



        private void dimLevels_Para(UIDocument uiDoc, List<Reference> myListRef)
        {

            Document doc = uiDoc.Document;

            ReferenceArray ra = new ReferenceArray();

            //List<Reference> myListRef_2 = new List<Reference>();

            foreach (var myRef in myListRef)
            {
                Level myLevel = doc.GetElement(myRef) as Level;
                Reference myLevelRef = new Reference(myLevel);
                ra.Append(myLevelRef);
                //myListRef_2.Add(myLevelRef);

            }

            setCurrentViewAsWorkPlan(uiDoc);

            XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");
            XYZ myDimPoint_2 = new XYZ(myDimPoint_1.X, myDimPoint_1.Y, myDimPoint_1.Z + 5);
            Line dimLine = Line.CreateBound(myDimPoint_1, myDimPoint_2);


            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();
                Dimension myDim = doc.Create.NewDimension(doc.ActiveView, dimLine, ra);
                trans.Commit();
            }

        }



        private bool isListLineParallel(List<XYZ> myListXYZ)
        {
            for (int i = 0; i < myListXYZ.Count; i++)
            {
                List<XYZ> subList = myListXYZ.GetRange(i + 1, myListXYZ.Count - (i + 1));
                foreach (XYZ nextXYZ in subList)
                {
                    double lengthCross = Math.Round(nextXYZ.CrossProduct(myListXYZ[i]).GetLength(), 7);

                    if (lengthCross >= 0.0000001)
                    {
                        return false;
                    }
                }
            }

            return true;
        }



        private void dimGrids_Para(UIDocument uiDoc, List<Reference> myListRef)
        {
            Document doc = uiDoc.Document;

            ReferenceArray ra = new ReferenceArray();

            List<XYZ> myListDirectLine = new List<XYZ>();

            foreach (var myRef in myListRef)
            {
                Grid myGrid = doc.GetElement(myRef) as Grid;
                Reference myGridRef = new Reference(myGrid);
                Line gridAsLine = myGrid.Curve as Line;
                if (gridAsLine == null)
                {
                    TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
                    return;
                }
                myListDirectLine.Add(gridAsLine.Direction);

                ra.Append(myGridRef);
                //myListRef_2.Add(myGridRef);	
            }

            if (!isListLineParallel(myListDirectLine))
            {
                TaskDialog.Show("Error", "Chỉ hỗ trợ Grid dạng Line song song với nhau, hãy chọn cẩn thận...");
                return;
            }

            setCurrentViewAsWorkPlan(uiDoc);

            XYZ myDimPoint_1 = uiDoc.Selection.PickPoint("Pick Point To Place Dimension....");

            //TaskDialog.Show("check", XYZtoString(doc.ActiveView.ViewDirection));

            XYZ viewDirect = doc.ActiveView.ViewDirection;

            XYZ plusXYZ = new XYZ();

            if (Math.Round(viewDirect.Z, 6) == 0)
            {
                plusXYZ = new XYZ(-viewDirect.Y, viewDirect.X, viewDirect.Z);

            }

            else
            {
                Grid firstGrid = doc.GetElement(myListRef[0]) as Grid;
                Line gridLine = firstGrid.Curve as Line;
                XYZ girdLineDirect = gridLine.Direction;
                plusXYZ = new XYZ(-girdLineDirect.Y, girdLineDirect.X, girdLineDirect.Z);
                //plusXYZ = new XYZ(0,1,0);
            }
            XYZ myDimPoint_2 = myDimPoint_1 + plusXYZ;
            Line l = Line.CreateBound(myDimPoint_1, myDimPoint_2);
            using (Transaction trans = new Transaction(doc, "Create linear Dimension"))
            {
                trans.Start();
                Dimension myDim = doc.Create.NewDimension(doc.ActiveView, l, ra);
                trans.Commit();
            }
        }





        public void setCurrentViewAsWorkPlan(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;

            using (Transaction trans = new Transaction(doc, "WorkPlane"))
            {
                trans.Start();

                //Plane plane = new Plane( uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);

                Plane plane = Plane.CreateByNormalAndOrigin(uiDoc.Document.ActiveView.ViewDirection, uiDoc.Document.ActiveView.Origin);


                SketchPlane sp = SketchPlane.Create(doc, plane);
                uiDoc.Document.ActiveView.SketchPlane = sp;
                trans.Commit();
            }
        }



    }


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

}
