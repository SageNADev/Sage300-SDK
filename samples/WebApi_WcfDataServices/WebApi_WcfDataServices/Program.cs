// The MIT License (MIT) 
// Copyright (c) 1994-2018 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using WebApi_WcfDataServices.Sage300AR;

namespace WebApi_WcfDataServices
{
    /// <summary>
    /// This sample program demonstrates how Sage 300 Web API can be accessed through the use of
    /// Windows Communication Foundation (WCF) Data Services. This method of integration is
    /// extremely simple since all necessary models and methods are generated with next to no effort.
    /// </summary>
    class Program
    {
        private const string CustomerNumber = "WCF_CUSTOMER";

        /// <summary>
        /// Main progam
        /// 
        /// To add a Data Service, right click on "References" in Solution Explorer and
        /// choose "Add Service Reference". In the Address field, enter the root URL of any Sage 300
        /// application module. (e.g. http://localhost/Sage300WebApi/-/SAMLTD/AR) Enter an
        /// appropriate Namespace (e.g. ARService) and click OK. All models and method for the
        /// Sage 300 application module are now generated and can be accessed via the service.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Specify the server to bind the service container to. This can be different than
            // the address used for adding the service reference.
            var uri = new Uri("http://localhost/Sage300WebApi/-/SAMLTD/AR");

            // Set up the service container with the appropriate credentials
            var container = new Sage300AR.Container(uri);
            container.Credentials = new NetworkCredential("WEBAPI", "WEBAPI");

            // Display all requests made through the service container in the Console
            container.SendingRequest2 += (sender, eventArgs) =>
            {
                Console.WriteLine("{0} {1}", eventArgs.RequestMessage.Method, eventArgs.RequestMessage.Url);
                using (var file = new StreamWriter(@"C:\Sage300WebAPIRequestPayload.txt", true))
                {
                    file.WriteLine("{0} {1}", eventArgs.RequestMessage.Method, eventArgs.RequestMessage.Url);
                }
            };

            //// Write all outbound payloads to file instead of Console
            //container.WritingEntity += (sender, eventArgs) =>
            //{
            //    using (var file = new StreamWriter(@"C:\SamplePayload.txt", true))
            //    {
            //        file.WriteLine("{0}", eventArgs.Data);
            //    }
            //};

            // Call each of the examples

            GetCustomers(container);

            GetRangeOfCustomers(container);

            GetCustomersWithFilter(container);

            GetCustomersByKey(container);

            CreateCustomer(container);

            UpdateCustomer(container);

            DeleteCustomer(container);

            Console.WriteLine("\nPress any key to end.");
            Console.ReadKey(); // wait for key press
        }

        /// <summary>
        /// Displays a customer through the Console
        /// </summary>
        /// <param name="customers"></param>
        public static void DisplayCustomers(List<Customer> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine("{0} {1} {2}", customer.CustomerNumber, customer.CustomerName, customer.City);
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(); // wait for key press
            Console.Clear();
        }

        /// <summary>
        /// Demonstrates how to retrieve the first page of AR Customers
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers
        /// </summary>
        public static void GetCustomers(Container container)
        {
            var customers = container.Customers;
            DisplayCustomers(customers.ToList());
        }

        /// <summary>
        /// Demonstrates how to retrieve AR Customers of a specific range
        /// The same method can be used to retrieve subsequent pages of results
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers()?$skip=5&$top=2
        /// </summary>
        public static void GetRangeOfCustomers(Container container)
        {
            var customers = container.Customers.Skip(5).Take(2);
            DisplayCustomers(customers.ToList());
        }

        /// <summary>
        /// Demonstrates the use of a filter
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers()?$filter=City eq 'Los Angeles'
        /// </summary>
        public static void GetCustomersWithFilter(Container container)
        {
            var customers = container.Customers.Where(x => x.City == "Los Angeles");
            DisplayCustomers(customers.ToList());
        }

        /// <summary>
        /// Demonstrates the retrieval of a single Customer using a key value
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('1100')
        /// </summary>
        private static void GetCustomersByKey(Container container)
        {
            var customer = container.Customers.Where(x => x.CustomerNumber == "1100").Single();
            DisplayCustomers(new List<Customer> { customer });
        }

        /// <summary>
        /// Demonstrates the creation of a Customer record
        /// POST http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers
        /// </summary>
        private static void CreateCustomer(Container container)
        {
            var customer = container.Customers.Where(x => x.CustomerNumber == "1100").Single();

            var newCustomer = new Customer();
            var properties = newCustomer.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var value = property.GetValue(customer);
                property.SetValue(newCustomer, value);
            }
            newCustomer.CustomerNumber = CustomerNumber;
            container.AddToCustomers(newCustomer);
            container.SaveChanges();
        }

        /// <summary>
        /// Demonstrates how to update a Customer
        /// PATCH http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('WCF_CUSTOMER')
        /// </summary>
        private static void UpdateCustomer(Container container)
        {
            var customer = container.Customers.Where(x => x.CustomerNumber == CustomerNumber).Single();
            customer.ShortName = "New Name";
            container.UpdateObject(customer);
            container.SaveChanges(SaveChangesOptions.PatchOnUpdate);
        }

        /// <summary>
        /// Demonstrates how to delete a Customer
        /// DELETE http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('WCF_CUSTOMER')
        /// </summary>
        private static void DeleteCustomer(Container container)
        {
            var customer = container.Customers.Where(x => x.CustomerNumber == CustomerNumber).Single();
            container.DeleteObject(customer);
            container.SaveChanges();
        }
    }
}
