/*
 * Created by SharpDevelop.
 * User: NAMTRUNG205
 * Date: 5/17/2019
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace WorkWithParameters
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("8E50624F-27E2-4E38-B01F-BB1566CE4FF8")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		
		
		//
		public void selectByParaSpecial()
		{
			
			// pick first element to get all parameters
			
			UIDocument uiDoc = this.ActiveUIDocument;
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
				
				if(myPara.StorageType == StorageType.String)
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
			    if(myFormSelect.DialogResult == System.Windows.Forms.DialogResult.Cancel)
			    {
			    	//else do all this :)    
			    	myFormSelect.Close();
			    	return;
			    }
			    
			    if(myFormSelect.DialogResult == System.Windows.Forms.DialogResult.OK)
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
			
	        if(saveSelection)
	        {
				 using (Transaction t = new Transaction(doc,"Create & Add To Selection Filter"))
			    {
			        t.Start(); // Start the transaction
			
			        // Create a SelectionFilterElement
			        SelectionFilterElement selFilter = SelectionFilterElement.Create(doc,paraNameSelected +" - " +paraValueSelected);
			
	
			        
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


		
		
		public void increaseValueParameter_MultipleOneSelect()
		{			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			string prefix = "9B";
			
			int beginNumber = 1;
			
			string suffix = "";
			
			
			string parameterName = "INNO_Ten-CK";
			
			using (IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm()) 
			
			{
				// Add list parameter to cb
				myFormIncreaseParameter.ShowDialog();
				
				//if the user hits cancel just drop out of macro
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
			    {
			    	//else do all this :)    
			    	myFormIncreaseParameter.Close();
			    	return;
			    }
			    
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
			    {
			    	parameterName = myFormIncreaseParameter.paraName_Tb.Text;
			    	
			    	prefix = myFormIncreaseParameter.Pre_Tb.Text;
			    	beginNumber = Convert.ToInt32( myFormIncreaseParameter.startNum_Tb.Text);
			    	suffix = myFormIncreaseParameter.Suf_Tb.Text;
			    	myFormIncreaseParameter.Close();
			    }
				
				
			}

			string myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
			while (true) 
			
			{
				List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myStatus) as List<Reference>;

				using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
				{
					trans.Start();
					
					foreach (Reference myRef in myListRef) 
					{
						Element myElem = doc.GetElement(myRef);
						Parameter myParameter = myElem.LookupParameter(parameterName);
	                    if(myParameter == null)
	                    {
	                        TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
	                    }
	                    
	                    else
	                    {
	                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
	                    
	                    }

					}
					trans.Commit();
				}
            	beginNumber++;
				changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
              	beginNumber.ToString());
            	myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
			}
		
		}
		

		
		public void increaseValueParameter_SingleOneSelect()
		{			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			string prefix = "9B";
			
			int beginNumber = 1;
			
			string suffix = "";
			
			
			string parameterName = "INNO_Ten-CK";
			
			using (IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm()) 
			
			{
				// Add list parameter to cb
				myFormIncreaseParameter.ShowDialog();
				
				//if the user hits cancel just drop out of macro
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
			    {
			    	//else do all this :)    
			    	myFormIncreaseParameter.Close();
			    	return;
			    }
			    
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
			    {
			    	parameterName = myFormIncreaseParameter.paraName_Tb.Text;
			    	
			    	prefix = myFormIncreaseParameter.Pre_Tb.Text;
			    	beginNumber = Convert.ToInt32( myFormIncreaseParameter.startNum_Tb.Text);
			    	suffix = myFormIncreaseParameter.Suf_Tb.Text;
			    	myFormIncreaseParameter.Close();
			    }	
			}

			string myStatus = "Select Element....";
			while (true) 
			
			{
				
				
				Reference myRef  = uiDoc.Selection.PickObject(ObjectType.Element, myStatus);

				using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
				{
					trans.Start();

					Element myElem = doc.GetElement(myRef);
					Parameter myParameter = myElem.LookupParameter(parameterName);
                    if(myParameter == null)
                    {
                        TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
                    }
                    
                    else
                    {
                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
                    
                    }

					trans.Commit();
				}
            	beginNumber++;
				changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
              	beginNumber.ToString());
            	
            	myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
			}
		
		}
		
		

		
		public void increaseValueParameter()
		{			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			int methodIndex = 0;
			
			string prefix = "9B";
			
			int beginNumber = 1;
			
			string suffix = "";
			
			bool isDecrease = false;
			
			string parameterName = "INNO_Ten-CK";
			
			using (IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm()) 
			
			{
				// Add list parameter to cb
				myFormIncreaseParameter.ShowDialog();
				
				//if the user hits cancel just drop out of macro
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
			    {
			    	//else do all this :)    
			    	myFormIncreaseParameter.Close();
			    	return;
			    }
			    
			    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
			    {
			    	isDecrease = myFormIncreaseParameter.isDecreaseChecked;
			    	
			    	methodIndex = myFormIncreaseParameter.method;
			    	
			    	parameterName = myFormIncreaseParameter.paraName_Tb.Text;
			    	
			    	prefix = myFormIncreaseParameter.Pre_Tb.Text;
			    	beginNumber = Convert.ToInt32( myFormIncreaseParameter.startNum_Tb.Text);
			    	suffix = myFormIncreaseParameter.Suf_Tb.Text;
			    	myFormIncreaseParameter.Close();
			    }	
			}

			string myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
			while (true) 
			
			{
				// Method 1:
				if (methodIndex == 1) 
				{
					Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, myStatus);
	
					using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
					{
						trans.Start();

						Element myElem = doc.GetElement(myRef);
						Parameter myParameter = myElem.LookupParameter(parameterName);
	                    if(myParameter == null)
	                    {
	                        TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
	                    }
	                    
	                    else
	                    {
	                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
	                    }

						trans.Commit();
					}
					
					if(isDecrease)
					{
						beginNumber--;
					}
					else
					{
		            	beginNumber++;
					}

					changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
	              	beginNumber.ToString());
	            	myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
				}					
				
				// Method 2	

				if (methodIndex == 2) 
				{
					List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myStatus) as List<Reference>;
	
					using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
					{
						trans.Start();
						foreach (Reference myRef in myListRef) 
						{
							Element myElem = doc.GetElement(myRef);
							Parameter myParameter = myElem.LookupParameter(parameterName);
		                    if(myParameter == null)
		                    {
		                        TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
		                    }
		                    
		                    else
		                    {
		                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
		                    }
						}
						trans.Commit();
					}
					
					
					if(isDecrease)
					{
						beginNumber--;
					}
					else
					{
		            	beginNumber++;
					}
					
					changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
	              	beginNumber.ToString());
	            	myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
				}
				
				// Method 3	

				if (methodIndex == 3) 
				{

					List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, myStatus) as List<Reference>;
	
					using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
					{
						trans.Start();
						foreach (Reference myRef in myListRef) 
						{
							Element myElem = doc.GetElement(myRef);
							Parameter myParameter = myElem.LookupParameter(parameterName);
		                    if(myParameter == null)
		                    {
		                        TaskDialog.Show("Error!!", "Has no parameter name: " + parameterName);
		                    }
		                    
		                    else
		                    {
		                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
		                    	
        						if(isDecrease)
									{
										beginNumber--;
									}
									else
									{
						            	beginNumber++;
									}
		                    }
						}
						trans.Commit();
					}

					changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
	              	beginNumber.ToString());
	            	myStatus = "Next Name of Element: " + prefix + beginNumber.ToString() + suffix;
				}	
			}
		}
		
	
		
		public void increaseValueParameter_Update()
		{			
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;

			string prefix = "9B";
			
			int beginNumber = 1;
			
			string suffix = "";
			
			
			string parameterName = "INNO_Ten-CK";
			

			IncreaseParameterForm myFormIncreaseParameter = new IncreaseParameterForm();
			// Add list parameter to cb
			myFormIncreaseParameter.Show();
			myFormIncreaseParameter.TopMost = true;
			
			//if the user hits cancel just drop out of macro
		    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.Cancel)
		    {
		    	//else do all this :)    
		    	//myFormIncreaseParameter.Close();
		    	return;
		    }
		    
		    if(myFormIncreaseParameter.DialogResult == System.Windows.Forms.DialogResult.OK)
		    {
//		    	parameterName = myFormIncreaseParameter.paraName_Tb.Text;
//		    	
//		    	prefix = myFormIncreaseParameter.Pre_Tb.Text;
//		    	beginNumber = Convert.ToInt32( myFormIncreaseParameter.startNum_Tb.Text);
//		    	suffix = myFormIncreaseParameter.Suf_Tb.Text;
		    	//myFormIncreaseParameter.Close();
		    }
		
			
			
			
			while (true) 
			
			{
				// Load setting
				
				
				parameterName = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_NANE");
				prefix = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_PRE");
				
				
				
				beginNumber = Convert.ToInt32((valueOfSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START")));
				suffix = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_SUF");
					
				
				
				List<Reference> myListRef = uiDoc.Selection.PickObjects(ObjectType.Element, "Select Elements...") as List<Reference>;

				using (Transaction trans = new Transaction(doc, "Increase Parameter..."))
				{
					trans.Start();
					
					foreach (Reference myRef in myListRef) 
					{
						Element myElem = doc.GetElement(myRef);
						Parameter myParameter = myElem.LookupParameter(parameterName);
	                    if(myParameter == null)
	                    {
	                        TaskDialog.Show("Error!!", "Has no parpation name: " + parameterName);
	                    }
	                    
	                    else
	                    {
	                    	myParameter.Set(prefix + beginNumber.ToString() + suffix);
	                    
	                    }

					}
					trans.Commit();
				}
            	beginNumber++;
            	
			changeSetting(@"D:\Revit Setting\RevitSetting.set","INCREASE_PARA_START",
            	              beginNumber.ToString());
			}
		
		}
		

					  	
		private string valueOfSetting(string pathSettingFile, string settingName)
			
			{
			
				// Open file setting
				try 
				{
					if(File.Exists(pathSettingFile))
					{
						//Read all Line in file
						string[] myFullSetting = File.ReadAllLines(pathSettingFile);
						//Create a Dictionay wiht key and 
						string[] mySettingList;
						foreach (string pairSetting in myFullSetting) 
						{
							// if  satisfy conditions, add to Dic{List[0]: List[1],...}
		
							if(pairSetting.Count(f => f == '|') == 1)
							{
								// Split line to list
								mySettingList = pairSetting.Split('|');
								// Add to Dic
								if(mySettingList[0] == settingName)
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
				catch (Exception e) {
	
					//TaskDialog.Show("Error", e.Message);
					return "";
				}
			}
		
				
				
		private void changeSetting(string pathSettingFile, string settingName, string settingValue)
			{
				//Check file exixst, if Ok ...
				
				try 
				{
					if(File.Exists(pathSettingFile))
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
		
							if(pairSetting.Count(f => f == '|') == 1)
							{
								// Split line to list
								mySettingList = pairSetting.Split('|');
								// Add to Dic
								myDicSetting.Add(mySettingList[0], mySettingList[1]);
							}
						}
						//Change setting
						if(myDicSetting.Keys.Contains(settingName))
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
				catch (Exception e) {
					
					//TaskDialog.Show("Error", e.Message);
				}
				
			}
	
		
		
		/// <summary>
		/// Show element by value of parameter:
		/// </summary>
		/// 
		
//		
//		public void showElementByValuePara()
//		{
//			UIDocument uiDoc = this.ActiveUIDocument;
//			Document doc = uiDoc.Document;
//
//			string nameParameter = "INNO_Ten-CK";
//			string valueParameter = "";
//			//Show dialog Window
//			using (ShowElemByParameter_Form myShowForm = new ShowElemByParameter_Form()) 
//			{
//				myShowForm.ShowDialog();
//				
//				//if the user hits cancel just drop out of macro
//			    if(myShowForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
//			    {
//			    	//else do all this :)    
//			    	myShowForm.Close();
//			    	return;
//			    }
//			    
//			    if(myShowForm.DialogResult == System.Windows.Forms.DialogResult.OK)
//			    {
//					nameParameter = myShowForm.paraName_Tb.Text;
//					valueParameter = myShowForm.value_Tb.Text;
//
//			    	myShowForm.Close();
//			    }
//			}
//			
//			
//			//TaskDialog.Show("abc", "Name: "+nameParameter+ ", Value: "+valueParameter);
//			//Query all element in view
//			View myViewToShow = doc.ActiveView;
//			
//			FilteredElementCollector myFec = new FilteredElementCollector(doc,myViewToShow.Id).WhereElementIsNotElementType();
//			
//			 ICollection<ElementId> myAllElement = myFec.ToElementIds();
//			 
//			 //TaskDialog.Show("Number", "number element: " + myAllElement.Count);
//			 
//			 List<ElementId> myListIdShow = new List<ElementId>();
//			 
//			 foreach (ElementId myElemId in myAllElement) 
//			 {
//			 	
//			 	Element myElem = doc.GetElement(myElemId);
//			 	
//				//ParameterSet myParas = e.Parameters;
//				
//				
//				
//				Parameter myPara = myElem.GetParameters(nameParameter).FirstOrDefault();
//				
//				if (myPara!=null) 
//				{
//					if(myPara.StorageType == StorageType.String)
//					{
//						if(myPara.AsString() !=null)
//						{
//							if (myPara.AsString().ToUpper() == valueParameter.ToUpper())
//			                {
//								myListIdShow.Add(myElemId);
//			                }						
//						}
//
//					}
//					
//					if(myPara.AsValueString() != null)
//					{
//						if (myPara.AsValueString().ToString().ToUpper() == valueParameter.ToUpper())
//		                {
//		                    myListIdShow.Add(myElemId);
//	
//		                }					
//					}
//				}
//				
//
//			 }			 
//			 
//// If listId is empty, show an warnning
//			 if(myListIdShow.Count <1)
//			 {
//			 TaskDialog.Show("Warning", "Has no element has " + nameParameter + 
//			 	                " is: " + valueParameter); 
//			 }
//			 else
//			 {
//				 //Show element in view
//				 uiDoc.ShowElements(myListIdShow);
//                uiDoc.Selection.SetElementIds(myListIdShow);
//			 }
//		}

		
	/// <summary>
	/// Show element by value of parameter:
	/// </summary>
	/// 
	public void showElementByValueParaAutoCom()
	{
		UIDocument uiDoc = this.ActiveUIDocument;
		Document doc = uiDoc.Document;
					

		string parameterNameValue = valueOfSetting(@"D:\Revit Setting\RevitSetting.set","FIND_BY_NAM_NAMEPARA");
		
		if(parameterNameValue == "" || parameterNameValue == null)
		{
			parameterNameValue = "INNO_Ten-CK";
		}
		
		string nameParameter = "INNO_Ten-CK";
		string valueParameter = "";
					
		//TaskDialog.Show("abc", "Name: "+nameParameter+ ", Value: "+valueParameter);
		//Query all element in view
		View myViewToShow = doc.ActiveView;
		
		FilteredElementCollector myFec = new FilteredElementCollector(doc,myViewToShow.Id).WhereElementIsNotElementType();
		
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
				if(myPara.StorageType == StorageType.String)
				{
					if(myPara.AsString() !=null)
					{
						if(!autoList.Contains(myPara.AsString()))
						{
							autoList.Add(myPara.AsString());
						
						}
					}

				}
				
				else
				{
					if(myPara.AsValueString() != null)
					{
						if(!autoList.Contains(myPara.AsValueString()))
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
		    if(myShowForm.DialogResult == System.Windows.Forms.DialogResult.Cancel)
		    {
		    	//else do all this :)    
		    	myShowForm.Close();
		    	return;
		    }
		    
		    if(myShowForm.DialogResult == System.Windows.Forms.DialogResult.OK)
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
			
			if (myPara!=null) 
			{
				if(myPara.StorageType == StorageType.String)
				{
					if(myPara.AsString() !=null)
					{
						if (myPara.AsString().ToUpper() == valueParameter.ToUpper())
		                {
							myListIdShow.Add(myElemId);
		                }						
					}

				}
				
				if(myPara.AsValueString() != null)
				{
					if (myPara.AsValueString().ToString().ToUpper() == valueParameter.ToUpper())
	                {
	                    myListIdShow.Add(myElemId);

	                }					
				}
			}
			

		 }			 
		 
		// If listId is empty, show an warnning
		 if(myListIdShow.Count <1)
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
				
				if(myPara.StorageType == StorageType.String)
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

	
	public class FilterByNameElementType : ISelectionFilter
	{
		
		//Cac bien thanh vien
		List<string> myListNameFilter = new List<string>();
		
		// Bo khoi dung
		public FilterByNameElementType(List<string> myListName){
		myListNameFilter = myListName;
		}
		
		public bool AllowElement(Element e)
		{
			string typeE = e.GetType().Name.ToString();

			if(this.myListNameFilter.Contains(typeE))
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
		public FilterByIdCategory(List<int> myListIdCategory){
		this.ListIdCategory = myListIdCategory;
		}
		
		public bool AllowElement(Element e)
		{
			int categoryE = e.Category.Id.IntegerValue;

			if(this.ListIdCategory.Contains(categoryE))
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