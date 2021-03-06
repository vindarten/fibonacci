﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using EasyNetQ;
using Model;

namespace FirstApp
{

	class Program
	{
		static void Main(string[] args)
		{
			ArgParser parser;
			try
			{
				parser = new ArgParser(args);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				return;
			}

			CancellationTokenSource cts = new CancellationTokenSource();
			CancellationToken token = cts.Token;
			using (FirstApp firstApp = new FirstApp(parser))
			{
				firstApp.Start(token);
				Console.ReadLine();
				cts.Cancel();
			}
		}
	}
}