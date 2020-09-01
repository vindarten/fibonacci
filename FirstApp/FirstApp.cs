using EasyNetQ;
using Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FirstApp
{
	class FirstApp : IDisposable
	{
		private IFibonacciContext context = null;

		private IConfig config = null;

		private IBus bus = null;

		private HttpClient httpClient = null;

		private readonly string target = null;

		public FirstApp(IConfig config)
		{
			this.config = config;

			target = "http://" + config.target_address + ":" + config.target_port + "/api/fibonacci";
		}

		public void Start()
		{
			context = new FibonacciContext();

			IBus bus = RabbitHutch.CreateBus("host=localhost");
			
			//bus.Receive<FibonacciData>("fibonacci", HandleOldMessage)

			bus.Subscribe<FibonacciData>("fibonacci", HandleFibonacciMessage);
			Logger.WriteLog("Listening for messages is initialized");

			httpClient = new HttpClient();

			for (int i = 0; i < config.calc_num; ++i)
			{
				InitCalculationAsync(i);
			}
		}

		private async void InitCalculationAsync(int i)
		{
			while (true)
			{
				try
				{
					Logger.WriteLog($"Try init calculation {i + 1}. Send request to {target}");

					await httpClient.PutAsync(target, new FibonacciData(1, i).ToStringContent());

					Logger.WriteLog($"Calculation {i + 1} initialized successfully");
					break;
				}
				catch (Exception ex)
				{
					Logger.WriteLog($"Failed to initialize calculation {i + 1}: " + ex.Message);
					Thread.Sleep(1000);
				}
			}
		}

		private void HandleOldMessage(FibonacciData data)
		{ }

		private void HandleFibonacciMessage(FibonacciData data)
		{
			Task.Run(() =>
			{
				try
				{
					Logger.WriteLog($"Received new value {data.Value} for calculation {data.CalculationId + 1}");

					FibonacciData next_data = context.CalculateNext(data);
					Thread.Sleep(5000);
					Logger.WriteLog($"Сalculated next value {next_data.Value} for calculation {data.CalculationId + 1}");

					httpClient.PutAsync(target, next_data.ToStringContent()).Wait();

					Logger.WriteLog($"Next value {next_data.Value} for calculation {data.CalculationId + 1} has been sent");
				}
				catch (Exception ex)
				{
					Logger.WriteLog($"Calculation {data.CalculationId + 1} stopped.\nError occurred: " + ex.Message);
					return;
				}
			});
		}

		private bool _disposed = false;

		~FirstApp() => Dispose(false);

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			bus?.Dispose();
			httpClient?.Dispose();

			_disposed = true;
		}
	}
}
