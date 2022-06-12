using Polly;
using Polly.Retry;

namespace FileDownloader;

public class FileDownloaderConfiguration
{
    /*
     * initialRetryDelayInSeconds doubles each retryAttempt
     */
    public FileDownloaderConfiguration(
        int retryAttempts = 15,
        int initialRetryDelayInSeconds = 2,
        Action<string>? loggingMethod = null)
    {
        RetryPolicy = GetRetryPolicy(retryAttempts, initialRetryDelayInSeconds);
        if (loggingMethod is not null)
            LoggingMethod = loggingMethod;
    }

    public RetryPolicy RetryPolicy { get; }
    public Action<string>? LoggingMethod { get; }

    private static RetryPolicy GetRetryPolicy(int retryAttempts, int initialRetryDelayInSeconds)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetry(retryAttempts,
                retryAttempt =>
                {
                    var retryDelayInSeconds = retryAttempt * initialRetryDelayInSeconds;
                    return TimeSpan.FromSeconds(retryDelayInSeconds);
                });
    }
}