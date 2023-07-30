using CalculatorService.Interfaces.Infrastructure;
using System.Collections.Concurrent;
using System.Text.Json;

namespace CalculatorService.Tracker
{
	public class InMemoryTrackerService<T> : ITrackerService<T>
	{
		// TODO: Read from app settings
		public string HeaderKey => "X-Evi-Tracking-Id";

		private ConcurrentDictionary<string, ConcurrentBag<string>> _operations;

		public InMemoryTrackerService()
		{
			_operations = new ConcurrentDictionary<string, ConcurrentBag<string>>();
		}

		public async Task<List<T>> GetOperationsByTracker(string trackerId)
		{
			// Since interface defines method as async it must run a Task
			// Other implementations would require I/O operations where async makes more sense
			return await Task.Run(() =>
			{
				List<T> result = new List<T>();

				if (_operations.TryGetValue(trackerId, out var trackerValues))
				{
					result = trackerValues.ToList().Select(x => JsonSerializer.Deserialize<T>(x)).ToList();
				}

				return result;
			});			
		}

		public async Task SaveOperation(string trackerId, T value)
		{			
			await Task.Run(() =>
			{
				string json = JsonSerializer.Serialize(value);
				if (_operations.TryGetValue(trackerId, out var trackerValues))
					trackerValues.Add(json);
				else
					_operations.TryAdd(trackerId, new ConcurrentBag<string> { json });
			});
		}
	}
}
