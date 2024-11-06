using System;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

// This line is not mandatory, but improves loading performances
// 필수 아니지만 로딩 성능 향상 시켜주는 기능
[assembly: ExtensionApplication(typeof(Lab2.MyPlugin))]

namespace Lab2
{
    /// <summary>
    /// Revit 애드인 프로그램 Application.cs와 비슷한 기능 
    /// 해당 MyPlugin 클래스는 AutoCAD 응용 프로그램에서 
    /// 한 번 인스턴스화되고 세션 기간 동안 활성 상태로 유지
    /// 한 번 초기화를 수행하지 않으면 해당 MyPlugin 클래스 제거 필수 
    /// </summary>
    public class MyPlugin : IExtensionApplication
    {
        /// <summary>
        /// 애드인 프로그램(Lab2) 시작(초기화)
        /// Revit 애드인 프로그램 메서드 OnStartup과 비슷한 기능 
        /// </summary>
        public void Initialize()
        {
            // Add one time initialization here
            // One common scenario is to setup a callback function here that 
            // unmanaged code can call. 
            // To do this:
            // 1. Export a function from unmanaged code that takes a function
            //    pointer and stores the passed in value in a global variable.
            // 2. Call this exported function in this function passing delegate.
            // 3. When unmanaged code needs the services of this managed module
            //    you simply call acrxLoadApp() and by the time acrxLoadApp 
            //    returns  global function pointer is initialized to point to
            //    the C# delegate.
            // For more info see: 
            // http://msdn2.microsoft.com/en-US/library/5zwkzwf4(VS.80).aspx
            // http://msdn2.microsoft.com/en-us/library/44ey4b32(VS.80).aspx
            // http://msdn2.microsoft.com/en-US/library/7esfatk4.aspx
            // as well as some of the existing AutoCAD managed apps.

            // Initialize your plug-in application here
        }

        /// <summary>
        /// 애드인 프로그램(Lab2) 종료(정리)
        /// </summary>
        public void Terminate()
        {
            // Do plug-in application clean up here
            // 플러그인 애플리케이션을 여기에서 정리
        }

    }
}
