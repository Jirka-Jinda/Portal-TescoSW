using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Models
{
	public class ErrorViewModel
	{
		public string? RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
		public static void Log(string message)
        {
			FileIOModel.WriteLog(message);
        }
    }



	//public static void BrowserConsoleWrite(string s)
	//{
	//	//string script = $"myFunction('{arg1}', '{arg2}');";
	//	//Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", script, true);
	//}
}