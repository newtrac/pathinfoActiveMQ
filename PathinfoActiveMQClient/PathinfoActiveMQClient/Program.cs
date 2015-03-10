using PathinfoActiveMQClient.Impl;
using PathinfoActiveMQClient.MessageImpl;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            if (args.Length == 0)
            {
                Console.WriteLine("Please specify running mode. Must be \"publisher\" or \"listener\" (without quote).");
                Environment.Exit(-1);
            }

            if ("publisher".Equals(args[0]))
            {
                Client client = new DefaultClient("Camera-Request", "Camera-Response");
                while (true)
                {
                    Console.WriteLine("Type \"send\" to send a camera capture request: (type \"exit\" to exit)");
                    String text = Console.ReadLine();
                    if (text.Equals("send"))
                    {
                        CameraCaptureRequest request = new CameraCaptureRequest();
                        Message message = client.SendAndWait(request);
                        if(message != null)
                            logger.Log("Got response with id " + message.GetId());
                    }
                    else if (text.Equals("exit"))
                    {
                        Environment.Exit(0);
                    }
                }
            }
            else if ("listener".Equals(args[0]))
            {
                CaptureRequestHandler handler = new CaptureRequestHandler();
                Client client = new DefaultClient("Camera-Response", "Camera-Request");
                Console.WriteLine("Listener will never exit. Please kill the process if you want to quit.");
                client.WaitAndResponse(handler);
            }
            else
            {
                Console.WriteLine("Please specify running mode as parameter. Must be \"publisher\" or \"listener\" (without quote).");
                Environment.Exit(-2);
            }
        }
    }
}
