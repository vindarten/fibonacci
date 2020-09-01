using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EasyNetQ;

namespace Fibonacci.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FibonacciController : ControllerBase
	{
		private IFibonacciContext context = null;

		public FibonacciController(IFibonacciContext context)
		{
			this.context = context;
		}

		[HttpGet("{id}")]
		public long Get(int id)
		{
			return context.GetCurrent(id - 1).Value;
		}
 
		[HttpPut]
		public OkResult Put(FibonacciData data)
		{
			Logger.WriteLog($"Received new value {data.Value} for calculation {data.CalculationId + 1}");

			Task.Run(() =>
			{
				try
				{
					FibonacciData next_data = context.CalculateNext(data);
					Thread.Sleep(5000);
					Logger.WriteLog($"Сalculated next value {next_data.Value} for calculation {next_data.CalculationId + 1}");

					using (IBus bus = RabbitHutch.CreateBus("host=localhost"))
					{
						bus.Publish(next_data);
					}
						

					Logger.WriteLog($"Sending next value {next_data.Value} for calculation {next_data.CalculationId + 1}");
				}
				catch(Exception ex)
				{
					Logger.WriteLog($"Calculation {data.CalculationId + 1} stopped.\nError occurred: " + ex.Message);
					return;
				}
			});

			Logger.WriteLog($"Sending confirmation of receipt of value {data.Value} for calculation {data.CalculationId + 1}");
			return Ok();
		}
	}
}
