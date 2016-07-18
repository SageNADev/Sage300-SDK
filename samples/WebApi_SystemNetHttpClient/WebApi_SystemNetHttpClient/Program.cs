// The MIT License (MIT) 
// Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApi_SystemNetHttpClient
{
    /// <summary>
    /// This sample program demonstrates how Sage 300 Web API can be accessed through the use of
    /// System.Net.Http.HttpClient. This method of integration not only allows complete control
    /// over the specific requests being sent but also make possible, invocation of Process
    /// endpoints.
    /// </summary>
    class Program
    {
        private const string SampleCustomerNumber = "SAM_CUSTOMER";

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            GetCustomer().Wait();

            GetRangeOfCustomers().Wait();

            GetCustomersWithFilter().Wait();

            GetCustomersByKey().Wait();

            CreateCustomer().Wait();

            UpdateCustomer().Wait();

            DeleteCustomer().Wait();

            CreateInvoice().Wait();

            GenerateRecurringCharges().Wait();

            Console.WriteLine("\nPress any key to end.");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays a customer through the Console
        /// </summary>
        /// <param name="customers"></param>
        public static void DisplayCustomers(List<dynamic> customers)
        {
            foreach (var customer in customers)
            {
                Console.WriteLine("{0} {1} {2}", customer.CustomerNumber, customer.CustomerName, customer.City);
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        /// <summary>
        /// Demonstrates how to retrieve the first page of AR Customers
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers
        /// </summary>
        public static async Task GetCustomer()
        {
            dynamic customerFeed = await SendRequest(new HttpMethod("GET"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers");
            DisplayCustomers(customerFeed.value.ToObject<List<dynamic>>());
        }

        /// <summary>
        /// Demonstrates how to retrieve AR Customers of a specific range
        /// The same method can be used to retrieve subsequent pages of results
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers()?$skip=5&$top=2
        /// </summary>
        public static async Task GetRangeOfCustomers()
        {
            dynamic customerFeed = await SendRequest(new HttpMethod("GET"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers?$skip=5&$top=2");
            DisplayCustomers(customerFeed.value.ToObject<List<dynamic>>());
        }

        /// <summary>
        /// Demonstrates the use of a filter
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers()?$filter=City eq 'Los Angeles'
        /// </summary>
        public static async Task GetCustomersWithFilter()
        {
            dynamic customerFeed = await SendRequest(new HttpMethod("GET"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers?$filter=City eq 'Los Angeles'");
            DisplayCustomers(customerFeed.value.ToObject<List<dynamic>>());
        }

        /// <summary>
        /// Demonstrates the retrieval of a single Customer using a key value
        /// GET http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('1100')
        /// </summary>
        public static async Task GetCustomersByKey()
        {
            dynamic customer = await SendRequest(new HttpMethod("GET"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('1100')");
            DisplayCustomers(new List<dynamic> { customer });
        }

        /// <summary>
        /// Demonstrates the creation of a Customer record using a partial payload
        /// POST http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers
        /// </summary>
        public static async Task CreateCustomer()
        {
            var customer = new
            {
                // Required fields
                CustomerNumber = SampleCustomerNumber,
                ShortName = "Old Name",
                GroupCode = "WHL",
                TaxGroup = "BCTAX",

                // Display fields
                CustomerName = "Sample Customer Name",
                City = "Sample City",
            };
            dynamic newCustomer = await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers", customer);
            DisplayCustomers(new List<dynamic> { newCustomer });
        }

        /// <summary>
        /// Demonstrates how to update a Customer
        /// PATCH http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('SAM_CUSTOMER')
        /// </summary>
        public static async Task UpdateCustomer()
        {
            var customer = new
            {
                ShortName = "New Name"
            };
            await SendRequest(new HttpMethod("PATCH"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('" + SampleCustomerNumber + "')", customer);
        }

        /// <summary>
        /// Demonstrates how to delete a Customer
        /// DELETE http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('SAM_CUSTOMER')
        /// </summary>
        public static async Task DeleteCustomer()
        {
            await SendRequest(new HttpMethod("DELETE"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('" + SampleCustomerNumber + "')");
        }

        /// <summary>
        /// Demonstrates how to create records with header and detail relationships (Create an AR Invoice)
        /// POST http://localhost/Sage300WebApi/-/SAMLTD/AR/InvoiceBatches
        /// </summary>
        public static async Task CreateInvoice()
        {
            var detail = new
            {
                ItemNumber = "BK-360",
                Quantity = "2"
            };
            var invoice = new
            {
                CustomerNumber = "1100",
                DueDate = DateTime.Now,
                InvoiceDetails = new[] { detail }
            };
            var batch = new
            {
                Description = "Sample Invoice Batch",
                Invoices = new[] { invoice }
            };
            dynamic newBatch = await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/InvoiceBatches", batch);
            Console.WriteLine("AR Invoice Batch {0} was created.", newBatch.BatchNumber);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint (Generates a recurring charge)
        /// POST http://localhost/Sage300WebApi/-/SAMLTD/AR/CreateRecurringCharge($process)
        /// </summary>
        public static async Task GenerateRecurringCharges()
        {
            // Get the recurring charge to find next schedule date
            dynamic recurringCharge = await SendRequest(new HttpMethod("GET"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/RecurringCharges('ONCALL', '1100')");

            // Generate a recurring charge using the next schedule date
            var createRecurringCharge = new
            {
                RangeType = "RecurringChargeCode",
                RangeFrom = "ONCALL",
                RangeThrough = "ONCALL",
                RunDate = recurringCharge.NextScheduledDate,
                DateGenerationMethod = "NextScheduleDate",
                BatchGenerationMethod = "CreateaNewBatch",
            };
            await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/CreateRecurringCharge($process)", createRecurringCharge);
        }

        /// <summary>
        /// Sends a Sage 300 Web API request with a request payload and returns the object within the response payload
        /// </summary>
        /// <param name="method">The method representing the HTTP verb to request with</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="payload">Optional, The payload to b</param>
        /// <returns></returns>
        public static async Task<object> SendRequest(HttpMethod method, string requestUri, object payload = null)
        {
            HttpContent content = null;

            // Serialize the payload if one is present
            if (payload != null)
            {
                var payloadString = JsonConvert.SerializeObject(payload);
                content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            }

            // Create the Web API client with the appropriate authentication
            using (var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("ADMIN", "ADMIN") })
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Console.WriteLine("{0} {1}", method.Method, requestUri);

                // Create the Web API request
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                // Send the Web API request
                var response = await httpClient.SendAsync(request);
                var responsePayload = await response.Content.ReadAsStringAsync();

                var statusNumber = (int)response.StatusCode;
                Console.WriteLine("{0} {1}", statusNumber, response.StatusCode);

                if (statusNumber < 200 || statusNumber >= 300)
                {
                    Console.WriteLine(responsePayload);
                    throw new ApplicationException(statusNumber.ToString());
                }

                return string.IsNullOrWhiteSpace(responsePayload) ? null : JsonConvert.DeserializeObject(responsePayload);
            }
        }
    }
}
