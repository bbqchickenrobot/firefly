﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Dragonfly.Http;
using Dragonfly.Utils;
using Gate.Owin;

namespace Profile.Server
{
    internal class Tracer : IServerTrace
    {
        public void Event(TraceEventType type, TraceMessage message)
        {
#if DEBUG
            Console.WriteLine("[{0} {1}]", type, message);
#endif
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (new ServerFactory(new Tracer()).Create(App, 9090))
            {
                Console.WriteLine("Enter exit to exit:");
                for (; ; )
                {
                    var cmd = Console.ReadLine();
                    switch (cmd)
                    {
                        case "gc":
                            GC.Collect();
                            break;
                        case "exit":
                            return;
                    }
                }
            }
        }

        static void App(IDictionary<string, object> env, ResultDelegate result, Action<Exception> fault)
        {
            try
            {
                var requestPath = (string)env[OwinConstants.RequestPath];
                switch (requestPath)
                {
                    case "/baseline":
                        Baseline(env, result, fault);
                        break;
                    case "/favicon.ico":
                        Baseline(env, result, fault);
                        break;
                    case "/":
                        Welcome(env, result, fault);
                        break;
                    default:
                        NotFound(env, result, fault);
                        break;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    fault(ex);
                }
                catch
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void NotFound(IDictionary<string, object> env, ResultDelegate result, Action<Exception> fault)
        {
            result(
                "404 Not Found",
                new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Content-Type", new[] {"text/plain"}},
                        {"Content-Length", new[] {"0"}},
                    },
                    (write, error, end) =>
                    {
                        try
                        {
                            end();
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                error(ex);
                            }
                            catch
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        return () => { };
                    }
                );
        }

        private static void Baseline(IDictionary<string, object> env, ResultDelegate result, Action<Exception> fault)
        {
            result(
                "200 OK",
                new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Content-Type", new[] {"text/plain"}},
                        {"Content-Length", new[] {"8"}},
                    },
                    (write, error, end) =>
                    {
                        try
                        {
                            var bytes = Encoding.Default.GetBytes("Baseline");
                            if (!write(new ArraySegment<byte>(bytes),
                                () =>
                                {
                                    try
                                    {
                                        end();
                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            error(ex);
                                        }
                                        catch
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }))
                            {
                                end();
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                error(ex);
                            }
                            catch
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        return () => { };
                    }
                );
        }

        private static void Welcome(IDictionary<string, object> env, ResultDelegate result, Action<Exception> fault)
        {
            result(
                "200 OK",
                new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
                    {
                        {"Content-Type", new[] {"text/html"}},
                        {"Content-Length", new[] {WelcomeText.Length.ToString()}},
                    },
                    (write, error, end) =>
                    {
                        try
                        {
                            var bytes = Encoding.Default.GetBytes(WelcomeText);
                            if (!write(new ArraySegment<byte>(bytes),
                                () =>
                                {
                                    try
                                    {
                                        end();
                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            error(ex);
                                        }
                                        catch
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }))
                            {
                                end();
                            }
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                error(ex);
                            }
                            catch
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        return () => { };
                    }
                );
        }

        private static string WelcomeText = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Strict//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd"">

    }

}