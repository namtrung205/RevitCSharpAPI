		

		public void createALinearDimensionProject()
		{
			UIDocument uiDoc = this.ActiveUIDocument;
			Document doc = uiDoc.Document;
			
			using(Transaction trans = new Transaction(doc, "Create linear Dimension"))
			{
				trans.Start();

				
				// S1: Pick an element
				Reference myRef = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a foundation");
				Element myE = doc.GetElement(myRef);
				
				Options geomOp = Application.Create.NewGeometryOptions();
				geomOp.ComputeReferences = true;
				
				
				GeometryElement geometryElement = myE.get_Geometry(geomOp);
				
				
				// S2: Get Reference of face parallel
				// refArray to create dimension
				ReferenceArray ra = new ReferenceArray();
				
				List<Face> myListFaces = new List<Face>();

				
				List<Reference> myListRef = new List<Reference>();
				foreach (GeometryObject geometryObject in geometryElement) 
				{
					UV myPoint = new UV();
					
					if(geometryObject is Solid)
					{
						Solid solid = geometryObject as Solid;
						
						XYZ myNormFace = new XYZ();
						foreach (Face myFace in solid.Faces ) 
						{
							myNormFace = myFace.ComputeNormal(myPoint);
							
							if (Math.Abs(Math.Round(myNormFace.Z, 1)) == 1.0)
							{
								myListFaces.Add(myFace);
								
								myListRef.Add(myFace.Reference);
								ra.Append(myFace.Reference);
							}						
						}
					}
				}
				

				// Pick a dimension to get line from it
		
				Reference myRefDim = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a dimension....");
				Dimension myDimExits = doc.GetElement(myRefDim) as Dimension;
				
				Line lineDim = myDimExits.Curve as Line;
		
				Line line2 = lineDim.CreateOffset(1, new XYZ(1,1,1)) as Line;
					
				
				
				
				Dimension myDim = doc.Create.NewDimension(doc.ActiveView, line2, ra);

				trans.Commit();
			
		}
	}
	
	