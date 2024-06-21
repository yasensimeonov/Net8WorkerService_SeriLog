using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Options;
using Serilog;
using System.ServiceProcess;

namespace Medium_Net8WorkerService_SeriLog
{
    public sealed class SessionTimeMonitor :  WindowsServiceLifetime
    {
        private readonly ILogger<SessionTimeMonitor> _logger;
        private Guid currentServiceRunGuid = Guid.NewGuid();

        public SessionTimeMonitor(IHostEnvironment environment, IHostApplicationLifetime applicationLifetime, ILoggerFactory loggerFactory, IOptions<HostOptions> optionsAccessor)
            : base(environment, applicationLifetime, loggerFactory, optionsAccessor)
        {                        
            _logger = loggerFactory.CreateLogger<SessionTimeMonitor>();
            
            try
            {
                // InitializeLifetimeService();
                CanPauseAndContinue = false;
                CanHandleSessionChangeEvent = true;
                ServiceName = "SampleService";
            }
            catch (Exception ex)
            {                
                _logger.LogError(ex.ToString());
            }
        }

        #region Event handlers

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            try
            {
                _logger.LogInformation("OnStart Event handled.");

                //SessionManager.LogSessionData(
                //    currentServiceRunGuid,
                //    -1,
                //    SessionTrackingEvents.OnStart,
                //    String.Empty,
                //    true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        protected override void OnStop()
        {
            base.OnStop();
            
            try
            {
                _logger.LogInformation("OnStop Event handled.");

                //SessionManager.LogSessionData(
                //    currentServiceRunGuid,
                //    -1,
                //    SessionTrackingEvents.OnStop,
                //    String.Empty,
                //    true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            base.OnSessionChange(changeDescription);

            try
            {
                if (changeDescription.Reason == SessionChangeReason.SessionLogon
                    || changeDescription.Reason == SessionChangeReason.SessionLogoff
                    || changeDescription.Reason == SessionChangeReason.SessionLock
                    || changeDescription.Reason == SessionChangeReason.SessionUnlock
                    )
                {
                    _logger.LogInformation("OnSessionChange Event handled. SessionChange Description is: " + changeDescription.Reason.ToString());

                    //SessionManager.LogSessionData(
                    //    currentServiceRunGuid,
                    //    changeDescription.SessionId,
                    //    SessionTrackingEvents.OnSessionChange,
                    //    changeDescription.Reason.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        #endregion

    }
}
