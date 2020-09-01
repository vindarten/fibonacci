using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
	public static class Logger
	{
		public static void WriteLog(string mes)
		{
			Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.f") + " " + mes);
		}
	}
}
