#region Using Directives

using System;
using System.Reflection;
using log4net;

#endregion

namespace Tattoo.Common
{
    public static class Logger
    {
        static Logger()
        {
            Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        private static ILog Log { get; set; }

        public static bool IsDebugEnabled
        {
            get { return Log.IsDebugEnabled; }
        }

        // Debugs logging
        public static void Debug(object msg)
        {
            Log.Debug(msg);
        }

        public static void Debug(object msg, Exception ex)
        {
            Log.Debug(msg, ex);
        }

        public static void Debug(Exception ex)
        {
            Log.Debug(ex.Message, ex);
        }

        // Infoes logging
        public static void Info(object msg)
        {
            Log.Info(msg);
        }

        public static void Info(object msg, Exception ex)
        {
            Log.Info(msg, ex);
        }

        public static void Info(Exception ex)
        {
            Log.Info(ex.Message, ex);
        }

        // Warnings logging
        public static void Warn(object msg)
        {
            Log.Warn(msg);
        }

        public static void Warn(object msg, Exception ex)
        {
            Log.Warn(msg, ex);
        }

        public static void Warn(Exception ex)
        {
            Log.Warn(ex.Message, ex);
        }

        // Errors logging
        public static void Error(object msg)
        {
            Log.Error(msg);
        }

        public static void Error(object msg, Exception ex)
        {
            Log.Error(msg, ex);
        }

        public static void Error(Exception ex)
        {
            Log.Error(ex.Message, ex);
        }

        // Fatals logging
        public static void Fatal(object msg)
        {
            Log.Fatal(msg);
        }

        public static void Fatal(object msg, Exception ex)
        {
            Log.Fatal(msg, ex);
        }

        public static void Fatal(Exception ex)
        {
            Log.Fatal(ex.Message, ex);
        }
    }
}