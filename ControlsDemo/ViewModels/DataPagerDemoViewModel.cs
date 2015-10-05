using Padstone.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlsDemo.ViewModels
{
	public class DataPagerDemoViewModel : BindableObject
	{
		private static string GetRandomName(Random rand)
		{
			int charsCount =rand.Next(12);
			List<char> _string = new List<char>();

			for (int i = 0; i < charsCount; i++)
			{
				_string.Add((char)(64 + rand.Next(26)));
			}

			return new string(_string.ToArray());
		}

		public IEnumerable<string> Items { get; private set; }

		public DataPagerDemoViewModel()
		{
			Random rand = new Random(0);

			this.Items = Enumerable.Range(0, 100).Select(i => GetRandomName(rand));
		}
	}
}
