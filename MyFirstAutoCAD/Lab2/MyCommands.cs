using System;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Document = Autodesk.AutoCAD.ApplicationServices.Document;

// This line is not mandatory, but improves loading performances
// 필수 아니지만 로딩 성능 향상 시켜주는 기능
[assembly: CommandClass(typeof(Lab2.MyCommands))]

namespace Lab2
{
    /// <summary>
    /// Revit 애드인 프로그램 Command.cs와 비슷한 기능 
    /// This class is instantiated by AutoCAD for each document when
    /// a command is called by the user the first time in the context
    /// of a given document. In other words, non static data in this class
    /// is implicitly per-document! 
    /// </summary>
    class MyCommands
    {
        // The CommandMethod attribute can be applied to any public  member 
        // function of any public class.
        // The function should take no arguments and return nothing.
        // If the method is an intance member then the enclosing class is 
        // intantiated for each document. If the member is a static member then
        // the enclosing class is NOT intantiated.
        //
        // NOTE: CommandMethod has overloads where you can provide helpid and
        // context menu.

        // Revit의 명령(Command) 클래스와 비슷한 기능
        // 다만 차이점은 AutoCAD 응용 프로그램의 경우 
        // 명령(Command)을 할 때, Revit 처럼 버튼을 누르는게 아니라
        // 명령어를 텍스트 입력 및 엔터를 눌러서 명령(Command)을 실행시키는 방식이기 때문에,
        // 아래처럼 인스턴스 메서드 방식으로 명령(Command) 관련 기능을 구현함.

        /// <summary>
        /// Modal Command with localized name
        /// Modal 명령어 인스턴스 메서드 
        /// </summary>
        // TODO : CommandMethod 에 "MyGroup" 넣으면 애드인 프로그램 실행시 Command 명령 실행 안되서 구현 대상 제외
        //       "MyCommandLocal" 필요 없으므로 제외 (2024.11.06 jbh)
        //[CommandMethod("MyGroup", "MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        //[CommandMethod("MyCommand", "MyCommandLocal", CommandFlags.Modal)]
        [CommandMethod("MyCommand", CommandFlags.Modal)]
        public void MyCommand() // This method can have any name
        {
            // Put your command code here
            MessageBox.Show("Modal 명령어");
        }

        /// <summary>
        /// Modal Command with pickfirst selection
        /// 선택한 객체 정보를 가져오는 명령어 인스턴스 메서드 
        /// </summary>
        // TODO : CommandMethod 에 "MyGroup" 넣으면 애드인 프로그램 실행시 Command 명령 실행 안되서 구현 대상 제외
        //        "MyPickFirstLocal" 필요 없으므로 제외 (2024.11.06 jbh)
        //[CommandMethod("MyGroup", "MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        //[CommandMethod("MyPickFirst", "MyPickFirstLocal", CommandFlags.Modal | CommandFlags.UsePickSet)]
        [CommandMethod("MyPickFirst", CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void MyPickFirst() // This method can have any name
        {
            PromptSelectionResult result = Application.DocumentManager.MdiActiveDocument.Editor.GetSelection();

            // 사용자가 선택한 객체가 존재하는 경우 
            if (result.Status == PromptStatus.OK)
            {
                // There are selected entities
                // Put your command using pickfirst set code here
            }
            // 사용자가 선택한 객체가 없는 경우 
            else
            {
                // There are no selected entities
                // Put your command code here
            }
        }

        /// <summary>
        /// Application Session Command with localized name
        /// 애플리케이션 세션(Application Session) 명령어 인스턴스 메서드
        /// </summary>
        // TODO : CommandMethod 에 "MyGroup" 넣으면 애드인 프로그램 실행시 Command 명령 실행 안되서 구현 대상 제외
        //        "MySessionCmd" 필요 없으므로 제외 (2024.11.06 jbh)
        //[CommandMethod("MyGroup", "MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        //[CommandMethod("MySessionCmd", "MySessionCmdLocal", CommandFlags.Modal | CommandFlags.Session)]
        [CommandMethod("MySessionCmd", CommandFlags.Modal | CommandFlags.Session)]
        public void MySessionCmd() // This method can have any name
        {
            // Put your command code here
        }

        // Start of Lab2 
        // 1. Add a command named addAnEnt.
        // Use the CommandMethod attribute and a Public void function. 
        // Note: put the closing curley brace after step 21.

        /// <summary>
        /// addAnEnt 명령어 인스턴스 메서드 
        /// </summary>
        [CommandMethod("addAnEnt")]
        public void AddAnEnt()
        {
            // 2. Editor 변수 ed 선언 및 값 할당 
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            // 3. PromptKeywordOptions 클래스 객체 getWhichEntityOptions 선언 및 생성 
            //    객체 생성시 문자열 추가 "Which entity do you want to create? [Circle/Block] : ", "Circle Block"
            PromptKeywordOptions getWhichEntityOptions = new PromptKeywordOptions("Which entity do you want to create? [Circle/Block] : ", "Circle Block");

            // 4. PromptResult 클래스 객체 getWhichEntityResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetKeywords 호출 결과 리턴된 값 할당 
            PromptResult getWhichEntityResult = ed.GetKeywords(getWhichEntityOptions);

            // 5. PromptResult 클래스 객체 getWhichEntityResult 상태값이 OK인 경우 (PromptStatus.OK)
            if(getWhichEntityResult.Status == PromptStatus.OK)
            {
                // 6. PromptResult 클래스 객체 getWhichEntityResult에 속한 속성 "StringResult" 사용하여 switch - case문 실행 
                switch(getWhichEntityResult.StringResult)
                {
                    // 7. getWhichEntityResult.StringResult가 "Circle"인 경우
                    case "Circle":
                        // 8. 사용자가 원의 중심 입력하기  
                        PromptPointOptions getPointOptions = new PromptPointOptions("Pick Center Point : ");

                        // 9. PromptPointResult 클래스 객체 getPointResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetPoint 호출 결과 리턴된 값 할당 
                        PromptPointResult getPointResult = ed.GetPoint(getPointOptions);

                        // 10. PromptPointResult 클래스 객체 getPointResult 상태값이 OK인 경우 (PromptStatus.OK)
                        if(getPointResult.Status == PromptStatus.OK)
                        {
                            // 11. 사용자가 입력한 원의 중심 기준으로 반지름 입력하기  
                            // PromptDistanceOptions 클래스 객체 getRadiusOptions 선언 및 생성
                            // 해당 객체 생성시 생성자에 매개변수로 문자열 "Pick Radius : " 전달 
                            PromptDistanceOptions getRadiusOptions = new PromptDistanceOptions("Pick Radius : ");

                            // 12. 9번에서 선택한 지점(getPointResult)을 GetDistance 호출의 기준점으로 사용
                            // 이를 위해 11번에서 만든 PromptDistanceOptions 클래스 객체 "getRadiusOptions"의 속성 BasePoint 사용
                            // 속성 BasePoint(getRadiusOptions.BasePoint)에 getPointResult.Value; 값 할당 
                            getRadiusOptions.BasePoint = getPointResult.Value;

                            // 13. PromptDistanceOptions 클래스 객체 "getRadiusOptions"의 속성 BasePoint 에 할당된 값을 기준점으로 설정 
                            getRadiusOptions.UseBasePoint = true;

                            // 14. 원의 반지름 가져오기 
                            // 원의 반지름 가져오기 위해 PromptDoubleResult 클래스 객체 getRadiusResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetDistance 호출 결과 리턴된 값 할당
                            // 11번에서 만든 PromptDistanceOptions 클래스 객체 getRadiusOptions를 인스턴스 메서드 GetDistance에 인자값으로 전달
                            PromptDoubleResult getRadiusResult = ed.GetDistance(getRadiusOptions);
                        }
                        break;   // 15. case "Circle": 레이블 종료

                    // 16. getWhichEntityResult.StringResult가 "Block"인 경우
                    case "Block":
                        // 17. 사용자가 Block 이름 입력하기  
                        // PromptStringOptions 클래스 객체 blockNameOptions 선언 및 생성 
                        // 해당 객체 blockNameOptions 생성시 생성자에 인자값으로 문자열 "Enter name of the Block to create : " 전달 
                        PromptStringOptions blockNameOptions = new PromptStringOptions("Enter name of the Block to create : ");

                        // 18. 사용자가 입력한 Block 이름에 공백(' ')이 허용되지 않도록 비활성화 처리 
                        blockNameOptions.AllowSpaces = false;

                        // 19. 사용자가 입력한 Block 이름 가져오기 
                        // Block 이름 가져오기 위해 PromptResult 클래스 객체 blockNameResult 선언 및 Editor 변수 ed의 인스턴스 메서드 GetString 호출 결과 리턴된 값 할당
                        // 17번에서 만든 PromptStringOptions 클래스 객체 blockNameOptions를 인스턴스 메서드 GetString에 인자값으로 전달
                        PromptResult blockNameResult = ed.GetString(blockNameOptions);

                        break;   // 20. case "Block": 레이블 종료

                    // 21. 프로젝트 파일 "Lab2" 빌드 및 컴파일 실행 -> AutoCAD 응용 프로그램 실행 -> AutoCAD 도면 열고 명령어 "NETLOAD" 입력 및 엔터 
                    // 애드인 프로그램 dll 파일 "Lab2.dll" 연결 -> AutoCAD 도면에 명령어 "AddAnEnt" 입력 및 엔터 -> addAnEnt 명령어 인스턴스 메서드 AddAnEnt 실행
                }
            }
        }

        // LispFunction is similar to CommandMethod but it creates a lisp 
        // callable function. Many return types are supported not just string or integer.
        // LispFunction은 CommandMethod와 비슷하지만 lisp 호출 가능한 함수 생성.
        // 문자열이나 정수뿐만 아니라 많은 반환 유형이 지원

        /// <summary>
        /// 스크립트 언어 lisp 호출 가능한 MyLispFunction 인스턴스 메서드 
        /// </summary>
        [LispFunction("MyLispFunction", "MyLispFunctionLocal")]
        public int MyLispFunction(ResultBuffer args) // This method can have any name
        {
            // Put your command code here

            // Return a value to the AutoCAD Lisp Interpreter
            return 1;
        }
    }
}
