using Castle.DynamicProxy;
using System.Diagnostics;
using System.Reflection;

namespace Neobyte.Cms.Backend.RemoteHosting;

public class HostingConnectorTracingProxy : IInterceptor {

	private readonly ActivitySource _activitySource;

	public HostingConnectorTracingProxy (ActivitySource activitySource) {
		_activitySource = activitySource;
	}

	public void Intercept (IInvocation invocation) {
		var activity = _activitySource.StartActivity("hosting_connection", ActivityKind.Client);
		activity?.SetTag("hosting_connection.protocol", invocation.InvocationTarget.GetType().Name);
		activity?.SetTag("hosting_connection.method", invocation.Method.Name);
		invocation.Proceed();
		if (!IsAsyncMethod(invocation.Method)) {
			activity?.Stop();
		} else {
			Task returnTask = (Task)invocation.ReturnValue;
			invocation.ReturnValue = CreateAfterProceedTask(returnTask, activity);
		}
	}

	private Task CreateAfterProceedTask (Task returnTask, Activity? activity) {
		returnTask.ContinueWith(_ => activity?.Stop(), TaskContinuationOptions.AttachedToParent);
		return returnTask;
	}

	private bool IsAsyncMethod (MethodInfo method) {
		return method.ReturnType.IsSubclassOf(typeof(Task));
	}

}