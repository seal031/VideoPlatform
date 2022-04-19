using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Utility.Log4net
{
    public class ActionLoggerInfo
    {
        public string user_id { get; set; }
        public string user_ip { get; set; }
        public string operate_content { get; set; }

        //public DateTime operate_time { get; set; }

        public ActionLoggerInfo(string username, string userip, string message,DateTime time)
        {
            this.user_id = username;
            this.user_ip = userip;
            this.operate_content = message;
            //this.operate_time = time;
        }

    }
    public class ActionConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
        {
            var actionInfo = loggingEvent.MessageObject as ActionLoggerInfo;

            if (actionInfo == null)
            {
                writer.Write("");
            }
            else
            {
                switch (this.Option.ToLower())
                {
                    case "user_id":
                        writer.Write(actionInfo.user_id);
                        break;
                    case "user_ip":
                        writer.Write(actionInfo.user_ip);
                        break;
                    case "operate_content":
                        writer.Write(actionInfo.operate_content);
                        break;
                    //case "operate_time":
                    //    writer.Write(actionInfo.operate_time);
                    //    break;
                    default:
                        writer.Write("");
                        break;
                }
            }
        }
    }
    public class ActionLayoutPattern : PatternLayout
    {
        public ActionLayoutPattern()
        {
            this.AddConverter("actionInfo", typeof(ActionConverter));
        }
    }

    public class LogHelper
    {
        public static readonly LogHelper Instance = new LogHelper();
        private static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        //public static log4net.ILog loginfo = LogManager.GetLogger("loginfo");

        public LogHelper()
        {
            string text = "";
            text = System.AppDomain.CurrentDomain.BaseDirectory + "log4net.config";
             var a=XmlConfigurator.ConfigureAndWatch(new FileInfo(text));
        }

        private static ActionLoggerInfo _message = null;
        private static log4net.ILog _log;

        public static log4net.ILog Log
        {
            get
            {
                if (_log == null)
                {
                    _log = LogManager.GetLogger("OperateLogger");
                }
                return _log;
            }
        }

        public static void Debug()
        {
            if (Log.IsDebugEnabled)
            {
                Log.Debug(_message);
            }
        }
        public static void Error()
        {
            if (Log.IsErrorEnabled)
            {
                Log.Error(_message);
            }
        }
        public static void Fatal()
        {
            if (Log.IsFatalEnabled)
            {
                Log.Fatal(_message);
            }
        }
        public static void Info()
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(_message);
            }
        }
        public static void Warn()
        {
            try
            {
                if (Log.IsWarnEnabled)
                {
                    Log.Warn(_message);
                }
            }
            catch (Exception e)
            {
                var t = e;
            }
        }

        public static void SaveMessage(string username, string userip, string message,DateTime time, int level)
        {
            _message = new ActionLoggerInfo(username, userip, message,time);
            switch (level)
            {
                case 1: Info(); break;
                case 2: Warn(); break;
                case 3: Error(); break;
                case 4: Fatal(); break;
                default: break;
            }
        }
    }
}