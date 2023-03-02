using System.Threading.Tasks;
using System.Threading;
using System;

namespace Neobyte.Cms.Backend.Utils;

public class AsyncUtils {

	private readonly SemaphoreSlim _semaphore = new (1, 1);

	public async Task LockAsync (Func<Task> worker) {
		await _semaphore.WaitAsync();
		try {
			await worker();
		} finally {
			_semaphore.Release();
		}
	}

	public async Task<T> LockAsync<T> (Func<Task<T>> worker) {
		await _semaphore.WaitAsync();
		try {
			return await worker();
		} finally {
			_semaphore.Release();
		}
	}
}