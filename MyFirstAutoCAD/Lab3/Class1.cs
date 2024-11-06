using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Exception = System.Exception;

namespace Lab3
{
    //public class Class1
    //{

    //}

    public class adskClass
    {
        [CommandMethod("addAnEnt")]
        public void AddAnEnt()
        {
            // Editor 클래스 변수 ed 선언 및 값 할당
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // PromptKeywordOptions 클래스 객체 getWhichEntityOptions 선언 및 생성 
            // 객체 생성시 문자열 추가 "Which entity do you want to create? [Circle/Block] : ", "Circle Block"
            PromptKeywordOptions getWhichEntityOptions = new PromptKeywordOptions("Which entity do you want to create? [Circle/Block] : ", "Circle Block");

            // PromptResult 클래스 객체 getWhichEntityResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetKeywords 호출 결과 리턴된 값 할당 
            PromptResult getWhichEntityResult = ed.GetKeywords(getWhichEntityOptions);

            // PromptResult 클래스 객체 getWhichEntityResult 상태값이 OK인 경우 (PromptStatus.OK)
            if(getWhichEntityResult.Status == PromptStatus.OK)
            {
                // PromptResult 클래스 객체 getWhichEntityResult에 속한 속성 "StringResult" 사용하여 switch - case문 실행 
                switch(getWhichEntityResult.StringResult)
                {
                    case "Circle":
                        // 사용자가 원의 중심 입력하기  
                        PromptPointOptions getPointOptions = new PromptPointOptions("Pick Center Point : ");

                        // PromptPointResult 클래스 객체 getPointResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetPoint 호출 결과 리턴된 값 할당 
                        PromptPointResult getPointResult = ed.GetPoint(getPointOptions);

                        // PromptPointResult 클래스 객체 getPointResult 상태값이 OK인 경우(PromptStatus.OK)
                        if(getPointResult.Status == PromptStatus.OK)
                        {
                            // 사용자가 입력한 원의 중심 기준으로 반지름 입력하기  
                            // PromptDistanceOptions 클래스 객체 getRadiusOptions 선언 및 생성
                            // 해당 객체 생성시 생성자에 매개변수로 문자열 "Pick Radius : " 전달 
                            PromptDistanceOptions getRadiusOptions = new PromptDistanceOptions("Pick Radius : ");

                            // 사용자가 선택한 지점(getPointResult)을 GetDistance 호출의 기준점으로 사용
                            // 위에서 만든 PromptDistanceOptions 클래스 객체 "getRadiusOptions"의 속성 BasePoint 사용
                            // 속성 BasePoint(getRadiusOptions.BasePoint)에 getPointResult.Value; 값 할당 
                            getRadiusOptions.BasePoint = getPointResult.Value;

                            // PromptDistanceOptions 클래스 객체 "getRadiusOptions"의 속성 BasePoint 에 할당된 값을 기준점으로 설정 
                            getRadiusOptions.UseBasePoint = true;

                            // 원의 반지름 가져오기 
                            // 원의 반지름 가져오기 위해 PromptDoubleResult 클래스 객체 getRadiusResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetDistance 호출 결과 리턴된 값 할당
                            // 위에서 만든 PromptDistanceOptions 클래스 객체 getRadiusOptions를 인스턴스 메서드 GetDistance에 인자값으로 전달
                            PromptDoubleResult getRadiusResult = ed.GetDistance(getRadiusOptions);

                            // PromptDoubleResult 클래스 객체 getRadiusResult 상태값이 OK인 경우(PromptStatus.OK)
                            if(getRadiusResult.Status == PromptStatus.OK)
                            {
                                // 1. Database 클래스 변수 dwg 선언 및 Editor 변수 ed 속성 Database 값 할당 
                                Database dwg = ed.Document.Database;

                                // 2. Transaction 클래스 객체 trans를 선언하고
                                //    1번에서 만든 Database 클래스 객체 dwg의 인스턴스 메서드 사용하여 리턴된 값 할당 
                                Transaction trans = dwg.TransactionManager.StartTransaction();

                                // 3. try ~ catch() 블록 구현 
                                try
                                {
                                    // Circle 클래스 객체 circle 선언 및 생성
                                    Circle circle = new Circle(getPointResult.Value, Vector3d.ZAxis, getRadiusResult.Value);

                                    // BlockTableRecord 클래스 객체 btr 선언 및 Transaction 클래스 객체 trans의
                                    // GetObject 인스턴스 메서드 사용하여 리턴된 값 할당 
                                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);

                                    // BlockTableRecord 클래스 객체 btr에 Circle 클래스 객체 circle 추가 
                                    btr.AppendEntity(circle);

                                    // Transaction 클래스 객체 trans 인스턴스 메서드 AddNewlyCreatedDBObject 호출
                                    // Circle 클래스 객체 circle 인자로 전달하여 자동으로 닫을 수 있도록 함.
                                    trans.AddNewlyCreatedDBObject(circle, true);

                                    // Transaction 클래스 객체 trans 커밋
                                    trans.Commit();
                                }
                                catch(Exception ex)
                                {
                                    // 오류 발생할 경우 
                                    // Editor 클래스 변수 ed 인스턴스 메서드 "WriteMessage" 호출 
                                    ed.WriteMessage("problem due to " + ex.Message);
                                }
                                finally
                                {
                                    // Transaction 클래스 객체 trans 리소스 해제
                                    trans.Dispose();
                                }
                            }
                        }
                        break;
                    case "Block":
                        // 사용자가 Block 이름 입력하기
                        // PromptStringOptions 클래스 객체 blockNameOptions 선언 및 생성 
                        // 해당 객체 blockNameOptions 생성시 생성자에 인자값으로 문자열 "Enter name of the Block to create : " 전달 
                        PromptStringOptions blockNameOptions = new PromptStringOptions("Enter name of the Block to create : ");

                        // 사용자가 입력한 Block 이름에 공백(' ')이 허용되지 않도록 비활성화 처리 
                        blockNameOptions.AllowSpaces = false;

                        // 사용자가 입력한 Block 이름 가져오기 
                        // Block 이름 가져오기 위해 PromptResult 클래스 객체 blockNameResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetString 호출 결과 리턴된 값 할당
                        // 위에서 만든 PromptStringOptions 클래스 객체 blockNameOptions를 인스턴스 메서드 GetString에 인자값으로 전달
                        PromptResult blockNameResult = ed.GetString(blockNameOptions);

                        // PromptResult 클래스 객체 blockNameResult 상태값이 OK인 경우(PromptStatus.OK)
                        if(blockNameResult.Status == PromptStatus.OK)
                        {
                            // Database 클래스 변수 dwg 선언 및 Editor 변수 ed 속성 Database 값 할당
                            Database dwg = ed.Document.Database;

                            // Transaction 클래스 객체 trans를 선언하고
                            // 위에서 만든 Database 클래스 객체 dwg의 인스턴스 메서드 사용하여 리턴된 값 할당 
                            Transaction trans = dwg.TransactionManager.StartTransaction();

                            // try ~ catch() 블록 구현 
                            try
                            {
                                // BlockTableRecord 클래스 객체 newBlockDef 선언 및 생성 
                                BlockTableRecord newBlockDef = new BlockTableRecord();

                                // BlockTableRecord 클래스 객체 newBlockDef 이름 속성 Name에 값 할당
                                newBlockDef.Name = blockNameResult.StringResult;

                                // BlockTable 클래스 객체 blockTable 선언 및 Transaction 클래스 객체 trans의
                                // GetObject 인스턴스 메서드 사용하여 리턴된 값 할당 
                                BlockTable blockTable = (BlockTable)trans.GetObject(dwg.BlockTableId, OpenMode.ForRead);

                                // BlockTable 클래스 객체 blockTable에 이름(blockNameResult.StringResult)을 가진 블록이 없는 경우 
                                if(blockTable.Has(blockNameResult.StringResult) == false)
                                {
                                    // 이름(blockNameResult.StringResult)이 없으므로 블록 새로 추가 
                                    blockTable.UpgradeOpen();

                                    // BlockTable 클래스 객체 blockTable 인스턴스 메서드 Add 사용 및
                                    // BlockTableRecord 클래스 객체 newBlockDef 인자로 전달하여 데이터 추가 
                                    blockTable.Add(newBlockDef);

                                    // Transaction 클래스 객체 trans 인스턴스 메서드 AddNewlyCreatedDBObject 호출
                                    // BlockTableRecord 클래스 객체 newBlockDef 인자로 전달하여 자동으로 닫을 수 있도록 함.
                                    trans.AddNewlyCreatedDBObject(newBlockDef, true);

                                    // Circle 클래스 객체 circle1 선언 및 생성
                                    Circle circle1 = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, 10);

                                    // BlockTableRecord 클래스 객체 newBlockDef에 Circle 클래스 객체 circle1 추가 
                                    newBlockDef.AppendEntity(circle1);

                                    // Circle 클래스 객체 circle2 선언 및 생성
                                    Circle circle2 = new Circle(new Point3d(20, 10, 0), Vector3d.ZAxis, 10);

                                    // BlockTableRecord 클래스 객체 newBlockDef에 Circle 클래스 객체 circle2 추가
                                    newBlockDef.AppendEntity(circle2);

                                    // Transaction 클래스 객체 trans 인스턴스 메서드 AddNewlyCreatedDBObject 호출
                                    // Circle 클래스 객체 circle1, circle2 둘자 인자로 전달하여 자동으로 닫을 수 있도록 함.
                                    trans.AddNewlyCreatedDBObject(circle1, true);
                                    trans.AddNewlyCreatedDBObject(circle2, true);

                                    // PromptPointOptions 클래스 객체 blockRefPointOptions 선언 및 생성
                                    PromptPointOptions blockRefPointOptions = new PromptPointOptions("Pick insertion point of BlockRef : ");

                                    // PromptPointResult 클래스 객체 blockRefPointResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetPoint 호출 결과 리턴된 값 할당 
                                    PromptPointResult blockRefPointResult = ed.GetPoint(blockRefPointOptions);

                                    // PromptPointResult 클래스 객체 blockRefPointResult 상태값이 OK인 경우(PromptStatus.OK)
                                    if(blockRefPointResult.Status != PromptStatus.OK)
                                    {
                                        // Transaction 클래스 객체 trans 리소스 해제
                                        trans.Dispose();

                                        return; // 메서드 종료 
                                    }

                                    // BlockReference 클래스 객체 blockRef 선언 및 생성
                                    BlockReference blockRef = new BlockReference(blockRefPointResult.Value, newBlockDef.ObjectId);

                                    // BlockTableRecord 클래스 객체 curSpace 선언 및 Transaction 클래스 객체 trans의
                                    // GetObject 인스턴스 메서드 사용하여 리턴된 값 할당 
                                    BlockTableRecord curSpace = (BlockTableRecord)trans.GetObject(dwg.CurrentSpaceId, OpenMode.ForWrite);

                                    // BlockTableRecord 클래스 객체 curSpace에 BlockReference 클래스 객체 blockRef 추가 
                                    curSpace.AppendEntity(blockRef);

                                    // Transaction 클래스 객체 trans 인스턴스 메서드 AddNewlyCreatedDBObject 호출
                                    // BlockReference 클래스 객체 blockRef 인자로 전달하여 자동으로 닫을 수 있도록 함.
                                    trans.AddNewlyCreatedDBObject(blockRef, true);

                                    // Transaction 클래스 객체 trans 커밋
                                    trans.Commit();
                                }
                            }
                            catch(Exception ex)
                            {
                                // 오류 발생할 경우 
                                // Editor 클래스 변수 ed 인스턴스 메서드 "WriteMessage" 호출 
                                ed.WriteMessage("a problem occured because " + ex.Message);
                            }
                            finally
                            {
                                // Transaction 클래스 객체 trans 리소스 해제
                                trans.Dispose();
                            }
                        }

                        break;
                }
            }
        }
    }
}
