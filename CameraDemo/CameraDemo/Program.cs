using System;

namespace CameraDemo
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			using (CameraDemoGame game = new CameraDemoGame())
			{
				game.Run();
			}
		}
	}
}

