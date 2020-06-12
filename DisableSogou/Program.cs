using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace DisableSogou
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			bool createdNew = false;
			Mutex mutex = new Mutex(true, "{43039718-C810-42F4-980E-8C6710422880}", out createdNew);
			if (createdNew)
			{
				bool autoMode = args.Length > 0 && args[0].ToLower() == "-auto";
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new FormMain(autoMode));
			}			
		}		
	}
}
