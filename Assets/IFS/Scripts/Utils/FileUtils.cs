using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class FileUtils
{
	public static string getRandomFile(string path, string _extension )
	{
		ArrayList al = new ArrayList();
		DirectoryInfo di = new DirectoryInfo(path);
		FileInfo[] rgFiles = di.GetFiles("*." + _extension );
		foreach (FileInfo fi in rgFiles)
		{
			al.Add(fi.FullName);
		}

		System.Random r = new System.Random();
		int x = r.Next(0, al.Count);

		return al[x].ToString();
	}

	public static string Unescape(string txt)
	{
		if (string.IsNullOrEmpty(txt)) { return txt; }
		StringBuilder retval = new StringBuilder(txt.Length);
		for (int ix = 0; ix < txt.Length;)
		{
			int jx = txt.IndexOf('\\', ix);
			if (jx < 0 || jx == txt.Length - 1) jx = txt.Length;
			retval.Append(txt, ix, jx - ix);
			if (jx >= txt.Length) break;
			switch (txt[jx + 1])
			{
				case '"': retval.Append('\"'); break;  // Line feed
				case 'n': retval.Append('\n'); break;  // Line feed
				case 'r': retval.Append('\r'); break;  // Carriage return
				case 't': retval.Append('\t'); break;  // Tab
				case '\\': retval.Append('\\'); break; // Don't escape
				default:                                 // Unrecognized, copy as-is
					retval.Append('\\').Append(txt[jx + 1]); break;
			}
			ix = jx + 2;
		}
		return retval.ToString();
	}
}
