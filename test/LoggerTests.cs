using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Targets;
using Xunit;

namespace NLog.Extensions.Logging.Tests
{
    public class LoggerTests
    {
        private static Lazy<IServiceProvider> ServiceProvider = new Lazy<IServiceProvider>(BuildDi);

        public LoggerTests()
        {
            var target = GetTarget();
            target?.Logs.Clear();
        }

        [Fact]
        public void TestInit()
        {
            GetRunner().Init();

            var target = GetTarget();
            Assert.Equal("NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|init runner |0", target.Logs.FirstOrDefault());
           
        }

        [Fact]
        public void TestEventId()
        {
            GetRunner().LogDebugWithId();

            var target = GetTarget();
            Assert.Equal("NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|message with id |20", target.Logs.FirstOrDefault());
           
        }

        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL|message Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|message Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR|message Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO|message Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE|message Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN|message Exception of type 'System.Exception' was thrown.|20")]
        public void TestExceptionWithMessage(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, new Exception(), "message");
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault());
        }

        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL| Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG| Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR| Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO| Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE| Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN| Exception of type 'System.Exception' was thrown.|20")]
        public void TestExceptionWithEmptyMessage(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, new Exception(), string.Empty);
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault());
        }
        
        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL|[null] Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|[null] Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR|[null] Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO|[null] Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE|[null] Exception of type 'System.Exception' was thrown.|20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN|[null] Exception of type 'System.Exception' was thrown.|20")]
        public void TestExceptionWithNullMessage(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, new Exception(), null);
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault());
        }
        
        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL|message |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|message |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR|message |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO|message |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE|message |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN|message |20")]
        public void TestMessageWithNullException(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, null, "message");
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault()); 
        }
        
        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL|[null] |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG|[null] |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR|[null] |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO|[null] |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE|[null] |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN|[null] |20")]
        public void TestWithNullMessageAndNullException(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, null, null);
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault()); 
        }
        
        [Theory]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Critical, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|FATAL| |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Debug, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|DEBUG| |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Error, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|ERROR| |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Information, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|INFO| |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Trace, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|TRACE| |20")]
        [InlineData(Microsoft.Extensions.Logging.LogLevel.Warning, "NLog.Extensions.Logging.Tests.LoggerTests.Runner|WARN| |20")]
        public void TestWithEmptyMessageAndNullException(Microsoft.Extensions.Logging.LogLevel logLevel, string expectedLogMessage)
        {
            GetRunner().Log(logLevel, 20, null, string.Empty);
            
            var target = GetTarget();
            Assert.Equal(expectedLogMessage, target.Logs.FirstOrDefault()); 
        }
        
        private static Runner GetRunner()
        {
            var serviceProvider = ServiceProvider.Value;

            // Start program
            var runner = serviceProvider.GetRequiredService<Runner>();
            return runner;
        }

        private static MemoryTarget GetTarget()
        {
            var target = LogManager.Configuration.FindTargetByName<MemoryTarget>("target1");
            return target;
        }

        private static IServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            services.AddTransient<Runner>();
            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            loggerFactory.AddNLog();
            loggerFactory.ConfigureNLog("nlog.config");
            return serviceProvider;
        }


        public class Runner
        {
            private readonly ILogger<Runner> _logger;

            public Runner(ILoggerFactory fac)
            {
                _logger = fac.CreateLogger<Runner>();
            }


            public void LogDebugWithId()
            {
                _logger.LogDebug(20, "message with id");
            }
            
            public void Log(Microsoft.Extensions.Logging.LogLevel logLevel, int eventId, Exception exception, string message)
            {
                switch (logLevel)
                {
                    case Microsoft.Extensions.Logging.LogLevel.Trace:
                        _logger.LogTrace(eventId, exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Debug:
                        _logger.LogDebug(eventId, exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Information:
                        _logger.LogInformation(eventId, exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Warning:
                        _logger.LogWarning(eventId, exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Error:
                        _logger.LogError(eventId, exception, message);
                        break;
                    case Microsoft.Extensions.Logging.LogLevel.Critical:
                        _logger.LogCritical(eventId, exception, message);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
                }
            }

            public void LogWithScope()
            {
                using (_logger.BeginScope("scope1"))
                {
                    _logger.LogDebug(20, "message with id and {0} parameters", 1);
                }
            }

            public void Init()
            {
                _logger.LogDebug("init runner");

            }
        }
    }
}
