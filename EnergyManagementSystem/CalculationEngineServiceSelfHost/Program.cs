﻿//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="EMS-Team">
//     Company copyright tag.
// </copyright>
//-----------------------------------------------------------------------

namespace EMS.Services.CalculationEngineService
{
	using System;
	using EMS.Common;

	/// <summary>
	/// Class for Main method
	/// </summary>
	public class Program
	{
		/// <summary>
		/// Main method
		/// </summary>
		/// <param name="args">arguments for method</param>
		private static void Main(string[] args)
		{
			try
			{
				string message = "Starting Calculation Engine Service...";
				CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
				Console.WriteLine("\n{0}\n", message);

				using (CalculationEngineService ces = new CalculationEngineService())
				{
					ces.Start();

					message = "Press <Enter> to stop the service.";
					CommonTrace.WriteTrace(CommonTrace.TraceInfo, message);
					Console.WriteLine(message);
					Console.ReadLine();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine("CalculationEngineService failed.");
				Console.WriteLine(ex.StackTrace);
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.Message);
				CommonTrace.WriteTrace(CommonTrace.TraceError, "CalculationEngineService failed.");
				CommonTrace.WriteTrace(CommonTrace.TraceError, ex.StackTrace);
				Console.ReadLine();
			}
		}
	}
}