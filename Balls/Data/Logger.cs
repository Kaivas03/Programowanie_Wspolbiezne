using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Data
{
    public class Logger
    {
        //Queue of logs.
        private Queue<string> logs = new Queue<string>();
        //A thread to be dispatched to logging.
        private Task LoggingTask;
        //A cancelation token to end the task.
        private CancellationToken token;
        //File name.
        private String file;
        //Slowdown in miliseconds.
        private int latency = 100;
        //And a mutex for us to secure the logger.
        private Mutex mutex = new Mutex();

        //Constructor for our logger.
        public Logger()
        {
            //Setting name of the file based on current date and time.
            file = $"LoggerData---{DateTime.Now.ToString("dd/MM/yyyy_HH/mm/ss")}.txt";
            //Dispatching new task responsible for logging.
            LoggingTask = new Task(Logging, token);
        }

        //Starting the logging task.
        public void StartLogging()
        {
            LoggingTask.Start();
        }

        public void Logging()
        {
            //Stoper. Do odmierzania czasu.
            Stopwatch stopwatch = Stopwatch.StartNew();
            //And the task itself withn a loop.
            while (true)
            {
                //File.AppendAllText(file, stopwatch.ElapsedMilliseconds.ToString() + "\n");
                if (stopwatch.ElapsedMilliseconds % 10 == 0)
                {
                    //We create a temporary queue witch is a product of th GetLogs method.
                    //It gives us all new logs that haven't been written to the file yet.
                    Queue<String> tmp = GetLogs();

                    //As long as there are elements within our queue...
                    while (tmp.Count > 0)
                    {
                        //..we append them to our file from the queue.       
                        File.AppendAllText(file, tmp.Dequeue() + "\n\n");
                    }


                }
            }
        }

        //This method adds new log to our logs.
        public void AddLog(String doZapisania)
        {
            //We won't allow anyone mess with the process while we commit ourselfes to it, so
            //we lock the thread with mutex.
            mutex.WaitOne();
            //We add new log to the end of our queue.
            logs.Enqueue(doZapisania);
            //Now other processes can ask to log their data again.
            mutex.ReleaseMutex();
        }

        //Getter for logs.
        private Queue<String> GetLogs()
        {
            //We trigger our mutex to block current thread.
            mutex.WaitOne();
            //We create a queue of strings.
            Queue<String> tmp = new Queue<String>(logs);
            //We clear all our stored logs (They have been written to the file already).
            logs.Clear();
            //We realease the mutex letting our thread record new locks.
            mutex.ReleaseMutex();
            return tmp;
        }

    }
}