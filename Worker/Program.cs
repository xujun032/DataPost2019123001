using Message.Client;
using System;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            MessageManService.Subsribe();
        }
    }
}
