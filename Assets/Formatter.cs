#if UNITY_EDITOR
#endif

namespace Utility
{
#if UNITY_EDITOR
	//public static class Formatter
	//{
	//	public static string UnityInspectorTitle(string name)
	//	{
	//		System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^_|^._");
	//		string fieldName = regex.Replace(name, @"");
	//		fieldName = System.Text.RegularExpressions.Regex.Replace(fieldName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
	//		fieldName = System.Text.RegularExpressions.Regex.Replace(fieldName, @"^ {1,}", "");
	//		return System.Text.RegularExpressions.Regex.Replace(fieldName, @"^\w", m => m.Value.ToUpper());
	//	}
	//}
#endif
}
