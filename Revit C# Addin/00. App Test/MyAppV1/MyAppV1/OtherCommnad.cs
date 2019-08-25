using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyAppV1
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Other : IExternalCommand
    {
        // Set GUID [Guid("4A202994-8AA4-40F9-810D-3A0F663A6CA8")]
        static AddInId appId = new AddInId(new Guid("4A202994-8AA4-40F9-810D-3A0F663A6CA8"));

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
                helloWorld(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch { return Autodesk.Revit.UI.Result.Succeeded; }

        }
        public void helloWorld(UIDocument uiDoc)
        {
            TaskDialog.Show("Done", "Hello World");
        }
    }



    /// <summary>
    /// Parameter Tools
    /// </summary>


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class IncreaseParameter : IExternalCommand
    {
        // Set GUID [Guid("4A202994-8AA4-40F9-810D-3A0F663A6CA8")]
        // [Guid("809221F9-D3C8-41DE-8153-2FAFCF848AEB")]
        static AddInId appId = new AddInId(new Guid("809221F9-D3C8-41DE-8153-2FAFCF848AEB"));


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Thưc thi hàm createElevationByRooms
            try
            {
                increaseValueParameter(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            catch
            {
                return Result.Succeeded;
            }
        }




        public void increaseValueParameter(UIDocument uiDoc)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View myView = doc.ActiveView;

            int methodIndex = 0;

            string prefix = "9B";

            int beginNumber = 1;

            string suffix = "";

            bool isDecrease = false;

            int stepNum = 1;

            string parameterName = "INNO_Ten-CK";

            bool notError = true;

            string sortBy = "X";

            using (IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm())

            {
                // Add list parameter to cb
                myFormIncreaseParameter.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myFormIncreaseParameter.Close();
                    return;
                }

                if (myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    isDecrease = myFormIncreaseParameter.isDecreaseChecked;

                    methodIndex = myFormIncreaseParameter.method;

                    parameterName = myFormIncreaseParameter.paraName_Tb.Text;

                    prefix = myFormIncreaseParameter.Pre_Tb.Text;
                    beginNumber = Convert.ToInt32(myFormIncreaseParameter.startNum_Tb.Text);
                    suffix = myFormIncreaseParameter.Suf_Tb.Text;
                    stepNum = Convert.ToInt32(myFormIncreaseParameter.step_numUD.Value);

                    //SortBy
                    if (myFormIncreaseParameter.Y_Rb.Checked == true)
                    {
                        sortBy = "Y";
                    }
                    if (myFormIncreaseParameter.Z_Rb.Checked == true)
                    {
                        sortBy = "Z";
                    }

                    myFormIncreaseParameter.Close();
                }
            }

            string myStatus = "Next Value: " + prefix + beginNumber.ToString() + suffix;
            while (true)

            {
                // Method 1:
                if (methodIndex == 1)
                {
                    Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, myStatus);


                    try
                    {
                        using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
                        {
                            trans.Start();
                            notError = true;
                            Element myElem = doc.GetElement(myRef);
                            Parameter myParameter = myElem.LookupParameter(parameterName);
                            if (myParameter == null)
                            {
                                TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
                                notError = false;
                            }

                            else
                            {
                                myParameter.Set(prefix + beginNumber.ToString() + suffix);
                            }

                            trans.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        notError = false;
                        TaskDialog.Show("Error!!!", e.Message.ToUpper());
                    }

                    finally
                    {

                        if (isDecrease && notError)
                        {
                            beginNumber = beginNumber - stepNum;
                        }
                        if (!isDecrease && notError)
                        {
                            beginNumber = beginNumber + stepNum;
                        }

                        changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                          beginNumber.ToString());
                        myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
                    }
                }

                // Method 2	

                if (methodIndex == 2)
                {
                    List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myStatus) as List<Reference>;



                    using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
                    {
                        trans.Start();
                        notError = true;
                        foreach (Reference myRef in myListRef)
                        {
                            try
                            {
                                Element myElem = doc.GetElement(myRef);
                                Parameter myParameter = myElem.LookupParameter(parameterName);
                                if (myParameter == null)
                                {
                                    TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
                                    notError = false;
                                }

                                else
                                {
                                    myParameter.Set(prefix + beginNumber.ToString() + suffix);
                                }
                            }
                            catch (Exception e)
                            {
                                //								notError = false;
                                continue;
                                TaskDialog.Show("Error!!!", e.Message.ToUpper());
                            }
                        }
                        trans.Commit();
                    }


                    if (isDecrease && notError)
                    {
                        beginNumber = beginNumber - stepNum;
                    }
                    if (!isDecrease && notError)
                    {
                        beginNumber = beginNumber + stepNum;
                    }

                    changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                      beginNumber.ToString());
                    myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
                }

                // Method 3	

                if (methodIndex == 3)
                {
                    List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myStatus) as List<Reference>;

                    List<Element> myListElement = new List<Element>();
                    foreach (Reference myRef in myListRef)
                    {
                        Element myElement = doc.GetElement(myRef);
                        myListElement.Add(myElement);
                    }

                    //Sort by...
                    //SortByX

                    if (sortBy == "X")
                    {
                        myListElement.Sort(new SortElementInView_ByX(myView));
                    }
                    if (sortBy == "Y")
                    {
                        myListElement.Sort(new SortElementInView_ByY(myView));
                    }
                    if (sortBy == "Z")
                    {
                        myListElement.Sort(new SortElementInView_ByZ(myView));
                    }


                    if (isDecrease)
                    {
                        myListElement.Reverse();
                    }


                    using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
                    {
                        trans.Start();
                        foreach (Element myElem in myListElement)
                        {
                            try
                            {
                                notError = true;

                                Parameter myParameter = myElem.LookupParameter(parameterName);
                                if (myParameter == null)
                                {
                                    TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
                                    notError = false;
                                }

                                else
                                {
                                    myParameter.Set(prefix + beginNumber.ToString() + suffix);
                                }
                            }
                            catch (Exception e)
                            {
                                notError = false;
                                TaskDialog.Show("Error!!!", e.Message.ToUpper());
                            }
                            finally
                            {
                                //								if(isDecrease && notError)
                                //								{
                                //									beginNumber = beginNumber-stepNum;
                                //								}
                                //								if(!isDecrease && notError)
                                //								{
                                //					            	beginNumber = beginNumber+stepNum;
                                //								}	

                                if (notError)
                                {
                                    beginNumber = beginNumber + stepNum;
                                }
                            }

                        }
                        trans.Commit();
                    }

                    changeSetting(@"D:\Revit Setting\RevitSetting.set", "INCREASE_PARA_START",
                      beginNumber.ToString());
                    myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
                }
            }
        }



        private string valueOfSetting(string pathSettingFile, string settingName)

        {
            // Open file setting
            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
                            {
                                return mySettingList[1];
                            }

                            continue;
                        }
                        continue;
                    }
                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
                return "";
            }
        }


        private void changeSetting(string pathSettingFile, string settingName, string settingValue)
        {
            //Check file exixst, if Ok ...

            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    Dictionary<string, string> myDicSetting = new Dictionary<string, string>();

                    string[] mySettingList;
                    // Add pair of setting to Dic (transfer setting to Dic)

                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            myDicSetting.Add(mySettingList[0], mySettingList[1]);
                        }
                    }
                    //Change setting
                    if (myDicSetting.Keys.Contains(settingName))
                    {
                        myDicSetting[settingName] = settingValue;
                    }

                    // Clear Setting file
                    using (var fs = new FileStream(pathSettingFile, FileMode.Truncate))
                    {
                    }

                    // ReWrite Setting
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathSettingFile))
                    {
                        foreach (string mySetName in myDicSetting.Keys)
                        {
                            string lineToWrite = mySetName + "|" + myDicSetting[mySetName];
                            file.WriteLine(lineToWrite);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
            }

        }


    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ShowElementByValueParaAutoCom : IExternalCommand
    {
        // Set [Guid("0B9E69F6-2A26-41D1-8561-8059FB19958A")]
        // [Guid("809221F9-D3C8-41DE-8153-2FAFCF848AEB")]
        static AddInId appId = new AddInId(new Guid("0B9E69F6-2A26-41D1-8561-8059FB19958A"));


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Thưc thi hàm createElevationByRooms
            try
            {
                showElementByValueParaAutoCom(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            catch
            {
                return Result.Succeeded;
            }
        }



        /// <summary>
        /// Show element by value of parameter:
        /// </summary>
        /// 
        public void showElementByValueParaAutoCom(UIDocument uiDoc)
        {
            //UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;


            string parameterNameValue = valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "FIND_BY_NAM_NAMEPARA");

            if (parameterNameValue == "" || parameterNameValue == null)
            {
                parameterNameValue = "INNO_Ten-CK";
            }

            string nameParameter = "INNO_Ten-CK";
            string valueParameter = "";

            //TaskDialog.Show("abc", "Name: "+nameParameter+ ", Value: "+valueParameter);
            //Query all element in view
            View myViewToShow = doc.ActiveView;

            FilteredElementCollector myFec = new FilteredElementCollector(doc, myViewToShow.Id).WhereElementIsNotElementType();

            ICollection<ElementId> myAllElement = myFec.ToElementIds();

            //TaskDialog.Show("Number", "number element: " + myAllElement.Count);

            //Loc toan bo parameter của cac phan tu
            List<string> autoList = new List<string>();

            foreach (ElementId myElemId in myAllElement)
            {
                Element myElem = doc.GetElement(myElemId);

                //ParameterSet myParas = e.Parameters;
                Parameter myPara = myElem.GetParameters(parameterNameValue).FirstOrDefault();

                if (myPara != null)
                {
                    if (myPara.StorageType == StorageType.String)
                    {
                        if (myPara.AsString() != null)
                        {
                            if (!autoList.Contains(myPara.AsString()))
                            {
                                autoList.Add(myPara.AsString());

                            }
                        }

                    }

                    else
                    {
                        if (myPara.AsValueString() != null)
                        {
                            if (!autoList.Contains(myPara.AsValueString()))
                            {
                                autoList.Add(myPara.AsValueString());
                            }
                        }
                    }
                }

            }

            //Show dialog Window
            using (ShowElemByParameter_Form myShowForm = new ShowElemByParameter_Form(autoList))
            {
                myShowForm.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myShowForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myShowForm.Close();
                    return;
                }

                if (myShowForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    parameterNameValue = myShowForm.paraName_Tb.Text;
                    valueParameter = myShowForm.paraNameValue_Cb.Text;

                    myShowForm.Close();
                }
            }


            //TaskDialog.Show("Number", "number element: " + myAllElement.Count);

            List<ElementId> myListIdShow = new List<ElementId>();

            foreach (ElementId myElemId in myAllElement)
            {

                Element myElem = doc.GetElement(myElemId);

                //ParameterSet myParas = e.Parameters;



                Parameter myPara = myElem.GetParameters(parameterNameValue).FirstOrDefault();

                if (myPara != null)
                {
                    if (myPara.StorageType == StorageType.String)
                    {
                        if (myPara.AsString() != null)
                        {
                            if (myPara.AsString().ToUpper() == valueParameter.ToUpper())
                            {
                                myListIdShow.Add(myElemId);
                            }
                        }

                    }

                    if (myPara.AsValueString() != null)
                    {
                        if (myPara.AsValueString().ToString().ToUpper() == valueParameter.ToUpper())
                        {
                            myListIdShow.Add(myElemId);

                        }
                    }
                }


            }

            // If listId is empty, show an warnning
            if (myListIdShow.Count < 1)
            {
                TaskDialog.Show("Warning", "Has no element has " + parameterNameValue +
                                    " is: " + valueParameter);
            }
            else
            {
                //Show element in view
                uiDoc.ShowElements(myListIdShow);
                uiDoc.Selection.SetElementIds(myListIdShow);
            }
        }


        private string valueOfSetting(string pathSettingFile, string settingName)

        {
            // Open file setting
            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
                            {
                                return mySettingList[1];
                            }

                            continue;
                        }
                        continue;
                    }
                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
                return "";
            }
        }


        private void changeSetting(string pathSettingFile, string settingName, string settingValue)
        {
            //Check file exixst, if Ok ...

            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    Dictionary<string, string> myDicSetting = new Dictionary<string, string>();

                    string[] mySettingList;
                    // Add pair of setting to Dic (transfer setting to Dic)

                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            myDicSetting.Add(mySettingList[0], mySettingList[1]);
                        }
                    }
                    //Change setting
                    if (myDicSetting.Keys.Contains(settingName))
                    {
                        myDicSetting[settingName] = settingValue;
                    }

                    // Clear Setting file
                    using (var fs = new FileStream(pathSettingFile, FileMode.Truncate))
                    {
                    }

                    // ReWrite Setting
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathSettingFile))
                    {
                        foreach (string mySetName in myDicSetting.Keys)
                        {
                            string lineToWrite = mySetName + "|" + myDicSetting[mySetName];
                            file.WriteLine(lineToWrite);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
            }

        }

    }



    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SelectByValueParameter : IExternalCommand
    {
        // Set [Guid("0B9E69F6-2A26-41D1-8561-8059FB19958A")]
        // [Guid("E7806A72-28E1-458F-9A15-A962CD4E8FCA")]
        static AddInId appId = new AddInId(new Guid("E7806A72-28E1-458F-9A15-A962CD4E8FCA"));

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Thưc thi hàm createElevationByRooms
            try
            {
                selectByParaSpecial(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }

            catch
            {
                return Result.Succeeded;
            }
        }


        public void selectByParaSpecial(UIDocument uiDoc)
        {

            // pick first element to get all parameters

            //UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;

            Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Select an Element...");

            Element myFirstElem = doc.GetElement(myRef);

            // Get parameters of element
            ParameterSet myParaSet = myFirstElem.Parameters;



            List<string> myListParaName = new List<string>();
            List<string> myListParaValueString = new List<string>();

            bool saveSelection = false;

            foreach (Parameter myPara in myParaSet)
            {
                //TaskDialog.Show("abc", myPara.Definition.Name + "Value: " + myPara.AsString());
                myListParaName.Add(myPara.Definition.Name);

                if (myPara.StorageType == StorageType.String)
                {
                    myListParaValueString.Add(myPara.AsString());
                }

                else
                {
                    myListParaValueString.Add(myPara.AsValueString());
                }

            }

            //TaskDialog.Show("abc","num value: " + myListParaName.Count.ToString());
            //Reference myRef2 = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByNameElementType(myListType));

            List<ElementId> myListIdElem = new List<ElementId>();

            string paraNameSelected = "a";
            string paraValueSelected = "b";

            using (FilterForm myFormSelect = new FilterForm(myListParaName, myListParaValueString))

            {
                // Add list parameter to cb
                myFormSelect.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myFormSelect.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myFormSelect.Close();
                    return;
                }

                if (myFormSelect.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    paraNameSelected = myFormSelect.paraName_Cb.Text;
                    paraValueSelected = myFormSelect.value_Tb.Text;
                    saveSelection = myFormSelect.saveSelection;
                    myFormSelect.Close();
                }
            }

            // Select by Filter	


            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                    new FilterByParameterValue(paraNameSelected, paraValueSelected),
                                                                    "Select Element") as List<Reference>;

            foreach (Reference myRefe in myListRef) // Iterate through list of selected elements
            {
                // Add the ElementId of each selected element to the selection filter
                myListIdElem.Add(myRefe.ElementId);
            }

            if (saveSelection)
            {
                using (Transaction t = new Transaction(doc, "Create & Add To Selection Filter"))
                {
                    t.Start(); // Start the transaction

                    // Create a SelectionFilterElement
                    SelectionFilterElement selFilter = SelectionFilterElement.Create(doc, paraNameSelected + " - " + paraValueSelected);



                    foreach (Reference r in myListRef) // Iterate through list of selected elements
                    {
                        // Add the ElementId of each selected element to the selection filter
                        selFilter.AddSingle(r.ElementId);
                    }

                    // Commit the transaction
                    t.Commit();
                }

            }
            uiDoc.Selection.SetElementIds(myListIdElem);

            //TaskDialog.Show("abc", "number element: " + myListRef.Count);

        }


        private string valueOfSetting(string pathSettingFile, string settingName)

        {
            // Open file setting
            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
                            {
                                return mySettingList[1];
                            }

                            continue;
                        }
                        continue;
                    }
                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
                return "";
            }
        }


        private void changeSetting(string pathSettingFile, string settingName, string settingValue)
        {
            //Check file exixst, if Ok ...

            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    Dictionary<string, string> myDicSetting = new Dictionary<string, string>();

                    string[] mySettingList;
                    // Add pair of setting to Dic (transfer setting to Dic)

                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            myDicSetting.Add(mySettingList[0], mySettingList[1]);
                        }
                    }
                    //Change setting
                    if (myDicSetting.Keys.Contains(settingName))
                    {
                        myDicSetting[settingName] = settingValue;
                    }

                    // Clear Setting file
                    using (var fs = new FileStream(pathSettingFile, FileMode.Truncate))
                    {
                    }

                    // ReWrite Setting
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(pathSettingFile))
                    {
                        foreach (string mySetName in myDicSetting.Keys)
                        {
                            string lineToWrite = mySetName + "|" + myDicSetting[mySetName];
                            file.WriteLine(lineToWrite);
                        }
                    }
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
            }

        }

    }


    /// <summary>
    /// Dimension Tools
    /// </summary>
    /// 

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class DimenisonGroupSettingForm : IExternalCommand
    {
        // Set [Guid("A7F26FCB-950F-4FD6-A16C-11C83B2ECF2D")]
        static AddInId appId = new AddInId(new Guid("A7F26FCB-950F-4FD6-A16C-11C83B2ECF2D"));

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Thưc thi hàm createElevationByRooms
                DimGroupSettingForm(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch
            {
                return Autodesk.Revit.UI.Result.Succeeded;

            }

        }



        public void DimGroupSettingForm(UIDocument uiDoc)
        {
            using (DimSettingForm mySettingDim = new DimSettingForm())
            {
                mySettingDim.ShowDialog();
            }
        }


    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class LocateColumnOnPlan : IExternalCommand
    {
        // Set [Guid("0A9EFC00-1DD1-487B-BEE5-772FCD5B7506")]
        static AddInId appId = new AddInId(new Guid("0A9EFC00-1DD1-487B-BEE5-772FCD5B7506"));

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Thưc thi hàm createElevationByRooms
                AutoDimColumn_Multi(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch
            {
                return Autodesk.Revit.UI.Result.Succeeded;

            }

        }



        public void AutoDimColumn_Multi(UIDocument uiDoc)
        {
            Document doc = uiDoc.Document;
            View myView = doc.ActiveView;


            double myScaleView = myView.Scale;
            double offsetXValue = 8 * myScaleView;
            double offsetYValue = 8 * myScaleView;

            double beginMoveAt = -1;

            bool onlyDimCenter = false;
            //Load setting from Text


            try
            {
                offsetXValue = Convert.ToDouble(valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.01_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OX"));
                offsetYValue = Convert.ToDouble(valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.02_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_OY"));
            }
            catch (Exception)
            {
                TaskDialog.Show("Warning!!!", string.Format("Giá trị trong setting file không hợp lệ, sử dụng giá trị OX, OY mặc định bằng {0}", offsetXValue));
            }

            try
            {
                beginMoveAt = Convert.ToDouble(valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.04_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_MA"));
            }
            catch (Exception)
            {
                TaskDialog.Show("Warning!!!", string.Format("Giá trị bắt đầu bẻ dim được set mặc định bằng '2.5*TextHeight*Scale' "));
            }


            try
            {
                if (valueOfSetting(@"D:\Revit Setting\RevitSetting.set", "5.03_OTHER_TAB_AUTO_LOCATE_COLUMN_ON_PLAN_IS_ONLY_CENTER") == "True")
                {
                    onlyDimCenter = true;
                }
            }
            catch (Exception e)
            {
                //				TaskDialog.Show("Error!!!", e.Message.ToString());
                TaskDialog.Show("Warning!!!", string.Format("Thiết đặt onlydim center lỗi!!! "));
            }


            //Convert to feet
            offsetXValue = offsetXValue / 304.8;
            offsetYValue = offsetYValue / 304.8;


            List<int> myFilListId = new List<int>(){(int)BuiltInCategory.OST_StructuralColumns,
                (int)BuiltInCategory.OST_Columns, (int)BuiltInCategory.OST_StructuralFoundation};

            List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, new FilterByIdCategory(myFilListId), "Pick Column...")

                as List<Reference>;

            List<int> listElementId = new List<int>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here

            List<ElementId> noDoneList = new List<ElementId>();

            foreach (Reference myRef in myListRef)
            {
                try
                {
                    Element myElement = doc.GetElement(myRef);

                    if (myElement.Category.Name == "Columns")
                    {
                        onlyDimCenter = false;
                    }


                    AutoDimColumnOnPlane_Para_Setting(uiDoc, myElement, myScaleView, offsetXValue, offsetYValue, beginMoveAt / 304.8, onlyDimCenter);
                    //if (!listElementId.Contains(myElement.Id.IntegerValue))
                    //{
                    //    AutoDimColumnOnPlane_Para_Setting(uiDoc, myElement, myScaleView, offsetXValue, offsetYValue, beginMoveAt / 304.8, onlyDimCenter);
                    //    listElementId.Add(myElement.Id.IntegerValue);
                    //}


                }
                catch (Exception)
                {

                    noDoneList.Add(doc.GetElement(myRef).Id);
                    //TaskDialog.Show("abc", "has error!");
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            TaskDialog.Show("abc", "Dim : " + (myListRef.Count - noDoneList.Count) + " in " + elapsedMs.ToString() + " ms\n"
                            + "Cannot Dim: " + noDoneList.Count + " Columns\n"
                           + "The cannot dim column will be select and show...");
            if (noDoneList.Count > 0)
            {
                uiDoc.ShowElements(noDoneList);
                uiDoc.Selection.SetElementIds(noDoneList);
            }


        }



        public void AutoDimColumnOnPlane_Para_Setting(UIDocument uiDoc, Element myElement, double scaleFactor, double offsetXValue, double offsetYValue, double beginMoveAt, bool onlyCenter)
        {

            //Xem xet them cac thong so tu form
            /*
			 * Scale: ti le ban ve mat bang dinh vi cot(lay tu View hoac nhap tay)
			 * Khoang cach offset cua dim so voi tam cot (X, Y)
			 * Chi dim tam cot tron
			 * 
			 * 
			 * 
			 */
            //UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View myView = doc.ActiveView;

            bool isCylinder = false;

            // ColumnStructure Element dong vai tro nhu familyInstance trong ban ve
            FamilyInstance myElementInstance = myElement as FamilyInstance;

            //Kiem tra tinh dung dan cua column Element
            LocationPoint myElementLp = myElement.Location as LocationPoint;
            LocationCurve myElementLc = myElement.Location as LocationCurve;
            if (myElementLp == null)
            {
                if (myElementLc == null)
                {
                    TaskDialog.Show("Error!", "Doi tuong ban chon 0 phuong");
                    return;
                }
                else
                {
                    TaskDialog.Show("Error!", "Doi tuong ban chon 2 phuong");
                    return;
                }

            }

            //lay gia tri diem tham chieu cua cot (center of family)
            XYZ lcPoint = myElementLp.Point;

            //Xác định cột chỉ có instanceObject hay có cả solid  

            bool onlyInstance = true;
            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);
            if (geometryElement.Count() > 1) { onlyInstance = false; }


            //            TaskDialog.Show("abc", "Only Instance: " + onlyInstance);

            //Xac dinh goc xoay neu co(chi xu dung 2 doc xoay dac biet 90 va 0
            string rotated;
            if (Math.Round(Math.Abs(Math.Cos(myElementLp.Rotation)), 9) == 1)
            {
                rotated = "0";
            }
            else
            {
                if (Math.Round(Math.Abs(Math.Cos(myElementLp.Rotation)), 9) == 0)
                {
                    rotated = "90";
                }
                else
                {
                    rotated = "other";
                }
            }
            //			TaskDialog.Show("abc", "Cos: " + Math.Cos(myElementLp.Rotation));
            //            TaskDialog.Show("abc", "Rotate: " + rotated);

            bool isAbove = false;

            bool isRight = true;


            //get closest grid with column, only ortho grid

            List<Grid> myListGridClosest = getCloseGridWithPoint(uiDoc, lcPoint);

            //Check Left and Up
            XYZ section2Grid = null;
            if (myListGridClosest.Count == 2)
            {
                section2Grid = getIntersecOf2Line(myListGridClosest[0].Curve as Line, myListGridClosest[1].Curve as Line);
            }

            XYZ LcPointSectionPointVector = lcPoint - section2Grid;

            //xac dinh goc phan tu
            //Right
            if (LcPointSectionPointVector.DotProduct(myView.RightDirection) >= 0)
            {
                isRight = true;
            }
            else
            {
                isRight = false;
            }

            if (LcPointSectionPointVector.DotProduct(myView.UpDirection) >= 0)
            {
                isAbove = true;
            }
            else
            {
                isAbove = false;
            }

            double directValueX = 1;
            double directValueY = 1;

            if (!isAbove) { directValueY = -1; }
            if (!isRight) { directValueX = -1; }


            //Phan tich hinh hoc cua cot
            BoundingBoxXYZ myColBB = myElement.get_BoundingBox(null);

            UV oPoint = new UV(0, 0);

            //Cot chu nhat
            List<Face> listSideFace = new List<Face>();
            List<Face> listTopFace = new List<Face>();
            List<Face> listUpFace = new List<Face>();
            List<Face> listRightFace = new List<Face>();

            //Cot tron
            List<CylindricalFace> listCyFace = new List<CylindricalFace>();

            //get solid
            List<Solid> myListSolidCol = getListSolidFromObject(myElement);

            foreach (Solid mySolid in myListSolidCol)
            {
                XYZ myNormVec = new XYZ();
                foreach (Face myFace in mySolid.Faces)
                {
                    if (myFace is CylindricalFace)
                    {
                        CylindricalFace myCyFace = myFace as CylindricalFace;
                        listCyFace.Add(myCyFace);

                        continue;
                    }

                    myNormVec = myFace.ComputeNormal(oPoint);
                    if (Math.Round(myNormVec.DotProduct(myView.ViewDirection), 6) == 0)
                    {
                        if (Math.Abs(Math.Round(myNormVec.DotProduct(myView.UpDirection), 6)) == 1)
                        {
                            listUpFace.Add(myFace);
                        }

                        else
                        {
                            if (Math.Abs(Math.Round(myNormVec.DotProduct(myView.RightDirection), 6)) == 1)
                            {
                                listRightFace.Add(myFace);
                            }
                            else
                            {
                                listSideFace.Add(myFace);
                            }
                        }

                    }
                    if (Math.Abs(Math.Round(myNormVec.DotProduct(myView.ViewDirection), 6)) == 1)
                    {
                        listTopFace.Add(myFace);
                    }
                }
            }

            //			TaskDialog.Show("abc", "Num CyFAce: " + listCyFace.Count);
            //			TaskDialog.Show("abc", "Num SideFace: " + listSideFace.Count);			
            //			TaskDialog.Show("abc", "Num TopFace: " + listTopFace.Count);
            //			TaskDialog.Show("abc", "Num UpFace: " + listUpFace.Count);			
            //			TaskDialog.Show("abc", "Num RightFace: " + listRightFace.Count);

            if (listCyFace.Count > 1)
            {
                isCylinder = true;
            }


            //Tinh toan hinh hoc cua cot chu nhat
            XYZ lcPointOffsetUpdirect = lcPoint - 100000 * myView.UpDirection.Normalize();
            XYZ lcPointOffsetRightdirect = lcPoint - 100000 * myView.RightDirection.Normalize();

            listUpFace.Sort(new SortByDisPointToFace(lcPointOffsetUpdirect));

            listRightFace.Sort(new SortByDisPointToFace(lcPointOffsetRightdirect));

            Utils myUtil = new Utils();
            //Dim Face of rectang column

            Plane myViewPlane = Plane.CreateByNormalAndOrigin(myView.ViewDirection, myView.Origin);
            XYZ lcPointOnView = myUtil.getPointProjectFromPointOntoPlane(lcPoint, myViewPlane);

            double X;
            double Y;
            double D = 1;

            List<Reference> listCenterRefOfIns = new List<Reference>();
            List<Reference> listSideRefOFIns = new List<Reference>();

            // Dim cot Tron
            if (myElementInstance != null)
            {
                listCenterRefOfIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.CenterFrontBack));
                listCenterRefOfIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.CenterLeftRight));

                listSideRefOFIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.Front));
                listSideRefOFIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.Back));
                listSideRefOFIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.Left));
                listSideRefOFIns.AddRange(myElementInstance.GetReferences(FamilyInstanceReferenceType.Right));

                //Get center Line
                Options myOption = new Options();
                myOption.IncludeNonVisibleObjects = true;
                myOption.ComputeReferences = true;


                GeometryElement myGeoElementOfColumn = myElement.get_Geometry(myOption);

                foreach (var myGeo in myGeoElementOfColumn)
                {
                    if (myGeo is Line)
                    {
                        Line centerLineOfCol = myGeo as Line;
                        listCenterRefOfIns.Add(centerLineOfCol.Reference);
                        //						TaskDialog.Show("abc", "Center Line: " + centerLineOfCol.Length);
                    }
                }
            }

            List<Reference> listStrongRefOfIns = new List<Reference>();
            double textSizeDimType;


            #region Rectang colume
            if (!isCylinder)
            {

                X = Math.Abs(Math.Abs(myUtil.getDisFromPointToFace(lcPointOffsetRightdirect, listRightFace[0])) -
                             Math.Abs(myUtil.getDisFromPointToFace(lcPointOffsetRightdirect, listRightFace[listRightFace.Count - 1])));

                Y = Math.Abs(-Math.Abs(myUtil.getDisFromPointToFace(lcPointOffsetUpdirect, listUpFace[0])) +
                             Math.Abs(myUtil.getDisFromPointToFace(lcPointOffsetUpdirect, listUpFace[listUpFace.Count - 1])));

                //				TaskDialog.Show("abc", "X: " + X + "; Y: " + Y);	


                //Right Faces
                ReferenceArray myRefArRight = new ReferenceArray();
                ReferenceArray myRefArUp = new ReferenceArray();


                //			foreach (Face myFaceRight in listRightFace) 
                //			{
                //				myRefArRight.Append(myFaceRight.Reference);
                //			}				

                if (onlyInstance && rotated == "90")
                {
                    myRefArRight.Append(listUpFace[0].Reference);
                    myRefArRight.Append(listUpFace[listUpFace.Count - 1].Reference);
                }

                else
                {
                    myRefArRight.Append(listRightFace[0].Reference);
                    myRefArRight.Append(listRightFace[listRightFace.Count - 1].Reference);
                }

                if (myListGridClosest.Count > 0)
                {
                    myRefArRight.Append(new Reference(myListGridClosest[0]));
                    myRefArUp.Append(new Reference(myListGridClosest[1]));
                }


                Dimension myDimRight = null;
                using (Transaction trans = new Transaction(doc, "Create linear Right"))
                {
                    trans.Start();
                    Line lineDimRight = null;

                    lineDimRight = Line.CreateBound(lcPointOnView + (Y / 2 + offsetYValue) * myView.UpDirection * directValueY,
                                                       lcPointOnView + (Y / 2 + offsetYValue) * myView.UpDirection * directValueY + myView.RightDirection);

                    myDimRight = doc.Create.NewDimension(doc.ActiveView, lineDimRight, myRefArRight);

                    textSizeDimType = myDimRight.DimensionType.GetParameters("Text Size").FirstOrDefault().AsDouble();

                    //Edit dimsegments
                    DimensionSegmentArray myDimSegAr = myDimRight.Segments;
                    Dictionary<double, DimensionSegment> myDic_X_DimSeg = new Dictionary<double, DimensionSegment>();
                    if (myDimSegAr.Size > 1)
                    {
                        foreach (DimensionSegment myDimSeg in myDimSegAr)
                        {
                            if (!myDic_X_DimSeg.Keys.Contains(myDimSeg.TextPosition.X))
                            {
                                myDic_X_DimSeg.Add(myDimSeg.TextPosition.X, myDimSeg);
                            }

                        }

                        //Sort keys
                        List<double> xPositionText = myDic_X_DimSeg.Keys.ToList();
                        xPositionText.Sort();
                        //move text 

                        //						if(myDic_X_DimSeg[xPositionText[0]].Value < 2.5*textSizeDimType*scaleFactor)	

                        if (beginMoveAt < 0)
                        {
                            beginMoveAt = 2.5 * textSizeDimType * scaleFactor;
                        }


                        if (myDic_X_DimSeg[xPositionText[0]].Value < beginMoveAt)
                        {
                            myDic_X_DimSeg[xPositionText[0]].TextPosition = new XYZ(myDic_X_DimSeg[xPositionText[0]].TextPosition.X - 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_X_DimSeg[xPositionText[0]].TextPosition.Y,
                                                                                    myDic_X_DimSeg[xPositionText[0]].TextPosition.Z);

                        }
                        if (myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].Value < beginMoveAt)
                        {
                            myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition = new XYZ(myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.X + 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.Y,
                                                                                    myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.Z);

                        }
                    }

                    trans.Commit();
                }


                //Remove zero dim
                //removeSegmentDim_Para(myDimRight, "  ");

                //Up Faces

                //			foreach (Face myFaceUp in listUpFace) 
                //			{
                //				myRefArUp.Append(myFaceUp.Reference);
                //			}				

                //				myRefArUp.Append(listUpFace[0].Reference);
                //				myRefArUp.Append(listUpFace[listUpFace.Count-1].Reference);			



                //Nguoc
                if (onlyInstance && rotated == "90")
                {
                    myRefArUp.Append(listRightFace[0].Reference);
                    myRefArUp.Append(listRightFace[listRightFace.Count - 1].Reference);
                }

                //Xuoi
                else
                {
                    myRefArUp.Append(listUpFace[0].Reference);
                    myRefArUp.Append(listUpFace[listUpFace.Count - 1].Reference);
                }



                Dimension myDimUp = null;
                using (Transaction trans = new Transaction(doc, "Create linear Right"))
                {
                    trans.Start();
                    Line lineDimUp = null;

                    lineDimUp = Line.CreateBound(lcPointOnView + (X / 2 + offsetXValue) * myView.RightDirection * directValueX,
                                                           lcPointOnView + myView.UpDirection + (X / 2 + offsetXValue) * myView.RightDirection * directValueX);

                    myDimUp = doc.Create.NewDimension(doc.ActiveView, lineDimUp, myRefArUp);

                    textSizeDimType = myDimUp.DimensionType.GetParameters("Text Size").FirstOrDefault().AsDouble();
                    //				 	TaskDialog.Show("abc", "begin break: " + textSizeDimType*2.5*scaleFactor*304.8);
                    //Edit dimsegments
                    DimensionSegmentArray myDimSegAr = myDimUp.Segments;
                    Dictionary<double, DimensionSegment> myDic_Y_DimSeg = new Dictionary<double, DimensionSegment>();
                    if (myDimSegAr.Size > 1)
                    {
                        foreach (DimensionSegment myDimSeg in myDimSegAr)
                        {
                            if (!myDic_Y_DimSeg.Keys.Contains(myDimSeg.TextPosition.Y))
                            {
                                myDic_Y_DimSeg.Add(myDimSeg.TextPosition.Y, myDimSeg);
                            }

                        }

                        //Sort keys
                        List<double> yPositionText = myDic_Y_DimSeg.Keys.ToList();
                        yPositionText.Sort();



                        if (beginMoveAt < 0)
                        {
                            beginMoveAt = 2.5 * textSizeDimType * scaleFactor;
                        }


                        if (myDic_Y_DimSeg[yPositionText[0]].Value < beginMoveAt)
                        {

                            myDic_Y_DimSeg[yPositionText[0]].TextPosition = new XYZ(myDic_Y_DimSeg[yPositionText[0]].TextPosition.X,
                                                                                    myDic_Y_DimSeg[yPositionText[0]].TextPosition.Y - 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_Y_DimSeg[yPositionText[0]].TextPosition.Z);

                        }
                        if (myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].Value < beginMoveAt)
                        {
                            myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition = new XYZ(myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.X,
                                                                                    myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.Y + 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.Z);

                        }
                    }

                    trans.Commit();
                }

                //removeSegmentDim_Para(myDimUp, "  ");
            }
            #endregion


            #region Cylinder Column
            else
            {



                D = listCyFace[0].get_Radius(0).GetLength() * 2;
                //				TaskDialog.Show("abc", "D: " + D);


                //Right Faces
                ReferenceArray myRefArRight = new ReferenceArray();
                ReferenceArray myRefArUp = new ReferenceArray();


                if (onlyCenter == false)
                {

                    if (rotated == "90")
                    {
                        myRefArUp.Append(listCenterRefOfIns[1]);
                        myRefArUp.Append(listSideRefOFIns[2]);
                        myRefArUp.Append(listSideRefOFIns[3]);
                        //				myRefArRight.Append(listCyFace[0].Reference);
                        ////				myRefArRight.Append(listCyFace[1].Reference);


                        myRefArRight.Append(listCenterRefOfIns[0]);
                        myRefArRight.Append(listSideRefOFIns[0]);
                        myRefArRight.Append(listSideRefOFIns[1]);
                        //				myRefArUp.Append(listCyFace[0].Reference);				
                        //						myRefArUp.Append(listCyFace[1].Reference);					

                    }

                    else
                    {
                        myRefArRight.Append(listCenterRefOfIns[1]);
                        myRefArRight.Append(listSideRefOFIns[2]);
                        myRefArRight.Append(listSideRefOFIns[3]);
                        //				myRefArRight.Append(listCyFace[0].Reference);
                        ////				myRefArRight.Append(listCyFace[1].Reference);


                        myRefArUp.Append(listCenterRefOfIns[0]);
                        myRefArUp.Append(listSideRefOFIns[0]);
                        myRefArUp.Append(listSideRefOFIns[1]);
                        //				myRefArUp.Append(listCyFace[0].Reference);				
                        //						myRefArUp.Append(listCyFace[1].Reference);				

                    }

                }

                else
                {
                    myRefArRight.Append(listCenterRefOfIns[2]);

                    //				myRefArRight.Append(listCyFace[0].Reference);
                    ////				myRefArRight.Append(listCyFace[1].Reference);


                    myRefArUp.Append(listCenterRefOfIns[2]);
                    //				myRefArUp.Append(listCyFace[0].Reference);				
                    //				myRefArUp.Append(listCyFace[1].Reference);
                }




                if (!isPointOnGrid(lcPointOnView, myListGridClosest[0]))
                {
                    myRefArRight.Append(new Reference(myListGridClosest[0]));
                }

                if (!isPointOnGrid(lcPointOnView, myListGridClosest[1]))
                {
                    myRefArUp.Append(new Reference(myListGridClosest[1]));
                }


                Dimension myDimRight = null;
                using (Transaction trans = new Transaction(doc, "Create linear Right"))
                {
                    trans.Start();
                    Line lineDimRight = null;

                    lineDimRight = Line.CreateBound(lcPointOnView + (D / 2 + offsetYValue) * myView.UpDirection * directValueY,
                                                    lcPointOnView + (D / 2 + offsetYValue) * myView.UpDirection * directValueY + myView.RightDirection);

                    myDimRight = doc.Create.NewDimension(doc.ActiveView, lineDimRight, myRefArRight);
                    textSizeDimType = myDimRight.DimensionType.GetParameters("Text Size").FirstOrDefault().AsDouble();
                    //Edit dimsegments
                    DimensionSegmentArray myDimSegAr = myDimRight.Segments;
                    Dictionary<double, DimensionSegment> myDic_X_DimSeg = new Dictionary<double, DimensionSegment>();
                    if (myDimSegAr.Size > 1)
                    {
                        foreach (DimensionSegment myDimSeg in myDimSegAr)
                        {
                            if (!myDic_X_DimSeg.Keys.Contains(myDimSeg.TextPosition.X))
                            {
                                myDic_X_DimSeg.Add(myDimSeg.TextPosition.X, myDimSeg);
                            }

                        }

                        //Sort keys
                        List<double> xPositionText = myDic_X_DimSeg.Keys.ToList();
                        xPositionText.Sort();

                        if (beginMoveAt < 0)
                        {
                            beginMoveAt = 2.5 * textSizeDimType * scaleFactor;
                        }


                        if (myDic_X_DimSeg[xPositionText[0]].Value < beginMoveAt)
                        {
                            myDic_X_DimSeg[xPositionText[0]].TextPosition = new XYZ(myDic_X_DimSeg[xPositionText[0]].TextPosition.X - 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_X_DimSeg[xPositionText[0]].TextPosition.Y,
                                                                                    myDic_X_DimSeg[xPositionText[0]].TextPosition.Z);

                        }
                        if (myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].Value < 2.5 * textSizeDimType * scaleFactor)
                        {
                            myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition = new XYZ(myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.X + 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.Y,
                                                                                    myDic_X_DimSeg[xPositionText[xPositionText.Count - 1]].TextPosition.Z);

                        }
                    }

                    trans.Commit();
                }

                //				removeSegmentDim_Para( myDimRight, "  ");




                Dimension myDimUp = null;
                using (Transaction trans = new Transaction(doc, "Create linear Right"))
                {
                    trans.Start();
                    Line lineDimUp = null;

                    lineDimUp = Line.CreateBound(lcPointOnView + (D / 2 + offsetXValue) * myView.RightDirection * directValueX,
                                                 lcPointOnView + myView.UpDirection + (D / 2 + offsetXValue) * myView.RightDirection * directValueX);

                    myDimUp = doc.Create.NewDimension(doc.ActiveView, lineDimUp, myRefArUp);

                    textSizeDimType = myDimUp.DimensionType.GetParameters("Text Size").FirstOrDefault().AsDouble();

                    //Edit dimsegments
                    DimensionSegmentArray myDimSegAr = myDimUp.Segments;
                    Dictionary<double, DimensionSegment> myDic_Y_DimSeg = new Dictionary<double, DimensionSegment>();
                    if (myDimSegAr.Size > 1)
                    {
                        foreach (DimensionSegment myDimSeg in myDimSegAr)
                        {
                            if (!myDic_Y_DimSeg.Keys.Contains(myDimSeg.TextPosition.Y))
                            {
                                myDic_Y_DimSeg.Add(myDimSeg.TextPosition.Y, myDimSeg);
                            }
                        }

                        //Sort keys
                        List<double> yPositionText = myDic_Y_DimSeg.Keys.ToList();
                        yPositionText.Sort();


                        if (beginMoveAt < 0)
                        {
                            beginMoveAt = 2.5 * textSizeDimType * scaleFactor;
                        }


                        if (myDic_Y_DimSeg[yPositionText[0]].Value < beginMoveAt)
                        {
                            myDic_Y_DimSeg[yPositionText[0]].TextPosition = new XYZ(myDic_Y_DimSeg[yPositionText[0]].TextPosition.X,
                                                                                    myDic_Y_DimSeg[yPositionText[0]].TextPosition.Y - 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_Y_DimSeg[yPositionText[0]].TextPosition.Z);

                        }
                        if (myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].Value < beginMoveAt)
                        {
                            myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition = new XYZ(myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.X,
                                                                                    myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.Y + 2 * textSizeDimType * scaleFactor,
                                                                                    myDic_Y_DimSeg[yPositionText[yPositionText.Count - 1]].TextPosition.Z);
                        }
                    }

                    trans.Commit();
                }


                //				removeSegmentDim_Para( myDimUp, "  ");

            }

            #endregion

        }



        private XYZ getIntersecOf2Line(Line line1, Line line2)
        {
            IntersectionResultArray results;

            SetComparisonResult result = line1.Intersect(line2, out results);

            if (result != SetComparisonResult.Overlap) return null;

            if (results == null || results.Size != 1) return null;

            IntersectionResult iResult
              = results.get_Item(0);

            return iResult.XYZPoint;
        }


        private List<Solid> getListSolidFromObject(Element myElement)
        {

            Options myOptions = new Options();
            myOptions.ComputeReferences = true;

            GeometryElement geometryElement = myElement.get_Geometry(myOptions);

            List<Solid> myListSolid = new List<Solid>();

            foreach (GeometryObject myGeometryObject in geometryElement)
            {
                GeometryInstance myGeoInstance = myGeometryObject as GeometryInstance;

                if (myGeoInstance != null)
                {
                    foreach (GeometryObject instObj in myGeoInstance.SymbolGeometry)
                    {
                        Solid solid = instObj as Solid;
                        if (null == solid || solid.Volume < 0.0001)
                        {
                            continue;
                        }

                        myListSolid.Add(solid);

                    }
                }
                else
                {
                    Solid solid = myGeometryObject as Solid;
                    if (null == solid || solid.Volume < 0.0001)
                    {
                        continue;
                    }
                    myListSolid.Add(solid);
                }
            }
            return myListSolid;


        }


        public bool isPointOnGrid(XYZ paraPoint, Grid paraGrid)
        {
            Line lineOfGrid = paraGrid.Curve as Line;
            if (Math.Abs(lineOfGrid.Distance(paraPoint) - Math.Abs(paraPoint.Z)) < 0.00000001)
            {
                return true;
            }
            return false;
        }


        public List<Grid> getCloseGridWithPoint(UIDocument uiDoc, XYZ paraPoint)
        {

            //UIDocument uiDoc = this.ActiveUIDocument;
            Document doc = uiDoc.Document;
            View myView = doc.ActiveView;

            //Phan chia theo 2 list grid_X va grid_Y

            List<Grid> listGrid_X = new List<Grid>();
            List<Grid> listGrid_Y = new List<Grid>();

            //Loc toan bo phan tu trong view la grid
            List<Element> myListAllGrid = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_Grids)
                .OfClass(typeof(Grid)).ToList();


            Dictionary<double, Grid> myDicDisPoint_Grid_X = new Dictionary<double, Grid>();
            Dictionary<double, Grid> myDicDisPoint_Grid_Y = new Dictionary<double, Grid>();


            foreach (Element myElemGrid in myListAllGrid)
            {
                Grid myGrid = myElemGrid as Grid;
                Line lineOfGrid = myGrid.Curve as Line;

                if (Math.Abs(Math.Round(lineOfGrid.Direction.DotProduct(myView.UpDirection), 6)) == 1)
                {
                    //					listGrid_Y.Add(myGrid);
                    if (!myDicDisPoint_Grid_Y.Keys.Contains(lineOfGrid.Distance(paraPoint)))
                    {
                        myDicDisPoint_Grid_Y.Add(lineOfGrid.Distance(paraPoint), myGrid);
                    }
                }

                if (Math.Abs(Math.Round(lineOfGrid.Direction.DotProduct(myView.UpDirection), 6)) == 0)
                {
                    if (!myDicDisPoint_Grid_X.Keys.Contains(lineOfGrid.Distance(paraPoint)))
                    {
                        myDicDisPoint_Grid_X.Add(lineOfGrid.Distance(paraPoint), myGrid);
                    }
                }
            }

            List<double> distancePointToGrid_X = myDicDisPoint_Grid_Y.Keys.ToList();
            distancePointToGrid_X.Sort();


            Grid myClosestGrid_X = myDicDisPoint_Grid_Y[distancePointToGrid_X[0]] as Grid;


            List<double> distancePointToGrid_Y = myDicDisPoint_Grid_X.Keys.ToList();
            distancePointToGrid_Y.Sort();


            Grid myClosestGrid_Y = myDicDisPoint_Grid_X[distancePointToGrid_Y[0]] as Grid;


            List<Grid> myListClosestGrid = new List<Grid>() { myClosestGrid_X, myClosestGrid_Y };

            return myListClosestGrid;

        }



        private string valueOfSetting(string pathSettingFile, string settingName)

        {
            // Open file setting
            try
            {
                if (File.Exists(pathSettingFile))
                {
                    //Read all Line in file
                    string[] myFullSetting = File.ReadAllLines(pathSettingFile);
                    //Create a Dictionay wiht key and 
                    string[] mySettingList;
                    foreach (string pairSetting in myFullSetting)
                    {
                        // if  satisfy conditions, add to Dic{List[0]: List[1],...}

                        if (pairSetting.Count(f => f == '|') == 1)
                        {
                            // Split line to list
                            mySettingList = pairSetting.Split('|');
                            // Add to Dic
                            if (mySettingList[0] == settingName)
                            {
                                return mySettingList[1];
                            }

                            continue;
                        }
                        continue;
                    }
                    return "";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception e)
            {

                //TaskDialog.Show("Error", e.Message);
                return "";
            }


        }


    }


    /// <summary>
    /// Qick dim Level and Grids
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class QuickDimLevelOrGrid : IExternalCommand
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
            try
            {
                // Thưc thi hàm createElevationByRooms
                dimGridsOrLevels(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch
            {
                return Autodesk.Revit.UI.Result.Succeeded;

            }

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


    /// <summary>
    /// Qick dim Level and Grids
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class EditDimensionValue : IExternalCommand
    {
        // Set GUID [Guid("73320858-034C-480B-A859-CF48BABC9C9A")]
        static AddInId appId = new AddInId(new Guid("73320858-034C-480B-A859-CF48BABC9C9A"));

        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // Thưc thi hàm createElevationByRooms
                EditDimensionValueMethod(commandData.Application.ActiveUIDocument);
                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch
            {
                return Autodesk.Revit.UI.Result.Succeeded;

            }

        }


        public void EditDimensionValueMethod(UIDocument uiDoc)
        {

            Document doc = uiDoc.Document;
            View myView = doc.ActiveView;

            string method = "";

            double factorConvert = Utils_Convert.unitConvertLeng_FeetToX(uiDoc);

            //			TaskDialog.Show("abc", "He so chuyen doi: " + Utils_Dimension.unitConvertLeng_FeetToX(uiDoc));

            //Pick select Dimension
            List<string> myListNameElementType = new List<string>() { "Dimension", "AngularDimension" };

            Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, new FilterByNameElementType(myListNameElementType),
                                                                    "Pick dimensions to Correct...");

            Dimension myDim = doc.GetElement(myRef) as Dimension;

            //Danh sach gia tri dimension
            List<string> listDimValue = new List<string>();

            List<string> newListDimValue = new List<string>();
            if (myDim.Segments.Size > 1)
            {
                foreach (DimensionSegment myDimSeg in myDim.Segments)
                {
                    if (myDimSeg.ValueOverride == null || myDimSeg.ValueOverride == "")
                    {
                        listDimValue.Add(myDimSeg.ValueString);
                    }
                    else
                    {
                        listDimValue.Add(myDimSeg.ValueOverride);
                    }
                }
            }
            else
            {
                if (myDim.ValueOverride == null || myDim.ValueOverride == "")
                {
                    listDimValue.Add(myDim.ValueString);
                }
                else
                {
                    listDimValue.Add(myDim.ValueOverride);

                }
            }

            using (EditDimension_Form myEditDimForm = new EditDimension_Form(listDimValue))

            {
                // Add list parameter to cb
                myEditDimForm.ShowDialog();

                //if the user hits cancel just drop out of macro
                if (myEditDimForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    //else do all this :)    
                    myEditDimForm.Close();
                    return;
                }

                if (myEditDimForm.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //else do all this :)    
                    newListDimValue = myEditDimForm.listValue_Tb.Text.Split(';').ToList();

                    //Xac dinh chức năng đã chọn
                    method = myEditDimForm.buttonClicked;
                }


            }

            //Update dim Click

            if (method == "UPDATE")
            {
                //Multiple Dim
                if (myDim.NumberOfSegments > 1)
                {
                    for (int i = 0; i < newListDimValue.Count; i++)
                    {
                        //Kiem tra dau vao co the convert sang so ko?

                        if (Utils_Convert.canConvertStringToDouble(newListDimValue[i]) && newListDimValue[i] != "")
                        {
                            if (Math.Round(Convert.ToDouble(newListDimValue[i]), 3)
                               == Math.Round(Convert.ToDouble(myDim.Segments.get_Item(i).Value) * factorConvert, 3))
                            {
                                Utils_Dimension.overrideSegmentAtIndexDim_Para(uiDoc, myDim, i, null);
                                //								TaskDialog.Show("abc", "Gia tri duoc giu nguyen: " + i);
                            }

                            else
                            {
                                Utils_Dimension.overrideSegmentAtIndexDim_Para(uiDoc, myDim, i, @newListDimValue[i]);
                            }

                        }


                        else
                        {
                            Utils_Dimension.overrideSegmentAtIndexDim_Para(uiDoc, myDim, i, @newListDimValue[i]);
                        }
                    }
                }
                //Single Dim
                else
                {
                    if (newListDimValue[0] == myDim.ValueString)
                    {
                        Utils_Dimension.overrideSegmentAtIndexDim_Para(uiDoc, myDim, 0, null);
                    }
                    else
                    {
                        Utils_Dimension.overrideSegmentAtIndexDim_Para(uiDoc, myDim, 0, @newListDimValue[0]);
                    }
                }
            }

            if (method == "RESET")
            {

                //pick multi dims
                //Pick select Dimension

                List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element,
                                                                new FilterByNameElementType(myListNameElementType),
                                                                "Pick dimensions to Correct...") as List<Reference>;

                using (Transaction trans = new Transaction(doc, "Reset Dim"))
                {
                    trans.Start();


                    foreach (Reference myRef_2 in myListRef)
                    {
                        Dimension myDim_2 = doc.GetElement(myRef_2) as Dimension;

                        //Multi dim
                        if (myDim_2.NumberOfSegments > 0)
                        {
                            foreach (DimensionSegment myDimSeg_2 in myDim_2.Segments)
                            {
                                myDimSeg_2.ValueOverride = null;
                            }
                        }
                        //Single dim
                        else
                        {
                            myDim_2.ValueOverride = null;
                        }
                    }

                    trans.Commit();
                }


                //Clear Override
                using (Transaction t = new Transaction(doc, "Set Element Override"))
                {

                    t.Start();

                    foreach (Reference myRef_3 in myListRef)
                    {
                        Dimension myDim_3 = doc.GetElement(myRef_3) as Dimension;

                        myView.SetElementOverrides(myDim_3.Id, new OverrideGraphicSettings());
                    }
                    t.Commit();
                }
            }
        }
    }








    /// <summary>
    /// Help Tools
    /// </summary>

    //System.Diagnostics.Process.Start("http://www.page.com");

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class GotoWebSite : IExternalCommand
    {
        // Set [Guid("1CC399AD-79C9-45DF-A086-B601086C0921")]
        static AddInId appId = new AddInId(new Guid("1CC399AD-79C9-45DF-A086-B601086C0921"));

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
            gotoWebSite(commandData.Application.ActiveUIDocument);
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        public void gotoWebSite(UIDocument uiDoc)
        {
            System.Diagnostics.Process.Start("http://inaecjsc.vn/");
        }

    }



    public class SortElementInView_ByX : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByX(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
            {
                if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
            }
        }
    }



    public class SortElementInView_ByY : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByY(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.Y, 4) == Math.Round(centerPointElementRhs.Y, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
            }
        }
    }



    public class SortElementInView_ByZ : IComparer<Element>
    {
        View myView = null;
        public SortElementInView_ByZ(View MyView)
        {
            this.myView = MyView;
        }
        public int Compare(Element lhs, Element rhs)
        {

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementLhs_BB = lhs.get_BoundingBox(this.myView);

            XYZ minPointElementLhs = myElementLhs_BB.Min;
            XYZ maxPointElementLhs = myElementLhs_BB.Max;
            XYZ centerPointElementLhs = (minPointElementLhs + maxPointElementLhs) / 2;

            //Get bouding box of rebarSet lHS
            BoundingBoxXYZ myElementRhs_BB = rhs.get_BoundingBox(this.myView);
            XYZ minPointElementRhs = myElementRhs_BB.Min;
            XYZ maxPointElementRhs = myElementRhs_BB.Max;
            XYZ centerPointElementRhs = (minPointElementRhs + maxPointElementRhs) / 2;


            if (Math.Round(centerPointElementLhs.Z, 4) == Math.Round(centerPointElementRhs.Z, 4))
            {
                if (Math.Round(centerPointElementLhs.X, 4) == Math.Round(centerPointElementRhs.X, 4))
                {
                    return Math.Round(centerPointElementLhs.Y, 4).CompareTo(Math.Round(centerPointElementRhs.Y, 4));
                }
                else
                {
                    return Math.Round(centerPointElementLhs.X, 4).CompareTo(Math.Round(centerPointElementRhs.X, 4));
                }
            }
            else
            {
                return Math.Round(centerPointElementLhs.Z, 4).CompareTo(Math.Round(centerPointElementRhs.Z, 4));
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



    public class FilterByParameterValue : ISelectionFilter
    {
        public string prName;
        public string prValue;

        public FilterByParameterValue(string paraName, string paraValue)
        {
            this.prName = paraName;
            this.prValue = paraValue;

        }

        public bool AllowElement(Element e)
        {
            //              ParameterSet myParas = e.Parameters;

            Parameter myPara = e.GetParameters(this.prName).FirstOrDefault();

            if (myPara.StorageType == StorageType.String)
            {
                if (myPara.AsString() == this.prValue)
                {
                    return true;
                }

            }


            if (myPara.AsValueString() == this.prValue)
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
