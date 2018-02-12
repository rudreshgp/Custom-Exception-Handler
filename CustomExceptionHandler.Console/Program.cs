namespace CustomExceptionHandler.Console
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(@"Null exception");
                string x = null;
                Console.WriteLine(x.ToString());
            }
            catch (Exception ex)
            {
                ex.HandleException();
                //This should be logged
            }
            try
            {
                Console.WriteLine(@"Throwing custom exception");
                throw new CustomException(new List<string>());
            }
            catch (Exception ex)
            {
                ex.HandleException(); //This should not be logged
            }
            

        }
    }
}
