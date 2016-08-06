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
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApi_SampleIntegration
{
    /// <summary>
    /// This sample program demonstrates how a full featured integration with Sage 300 can be
    /// created with very little code using the Sage 300 Web API. In this example, we will
    /// automate the creation of an OE order for a brand new customer and coordinate the 
    /// workflow of the order's entire life cycle: from generating the associated OE shipment
    /// and OE invoice, to generating the AR invoice through IC day end and producing the
    /// eventual GL batch.
    /// By using the System.Net.Http.HttpClient library, the program has complete control
    /// over the specific requests that are made, making it possible to invoke process enpoints
    /// which are not strictly OData.
    /// </summary>
    class Program
    {
        private static string NewCustomerNumber { get; set; }
        private static string ItemNumber { get; set; }
        private static string QuantityToOrder { get; set; }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Set up the input parameters (can be customized to be passed in externally)
            Console.Write("Enter a new customer number:");
            NewCustomerNumber = Console.ReadLine();
            ItemNumber = @"A1-103/0"; 
            QuantityToOrder = "2";

            // Start the workflow
            CreateCustomer().Wait();
            UpdateCustomer().Wait();
            CreateOEOrder().Wait();
            InvokeICDayEnd().Wait();
            PostARInvoice().Wait();
            CreateGLBatch().Wait();

            Console.WriteLine("\nPress any key to end.");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates how to create a simple record
        /// (Creates a customer)
        /// </summary>
        public static async Task CreateCustomer()
        {
            var customer = new
            {
                CustomerNumber = NewCustomerNumber,
                GroupCode = "WHL",
                TaxGroup = "BCTAX",
                ShortName = "Old Name",
            };
            await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers", customer);
        }

        /// <summary>
        /// Demonstrates how to update a record
        /// (Updates the customer created previously)
        /// </summary>
        public static async Task UpdateCustomer()
        {
            var customer = new
            {
                ShortName = "New Name"
            };
            await SendRequest(new HttpMethod("PATCH"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/Customers('" + NewCustomerNumber + "')", customer);
        }

        /// <summary>
        /// Demonstrates how to create a complex record with header and detail relationships and read the resulting response
        /// (Creates an OE Order)
        /// </summary>
        public static async Task CreateOEOrder()
        {
            var detail = new
            {
                Item = ItemNumber,
                QuantityOrdered = QuantityToOrder,
                QuantityShipped = QuantityToOrder,
            };
            var order = new
            {
                CustomerNumber = NewCustomerNumber,
                InvoiceWillBeProduced = true,
                OrderDetails = new[] { detail }
            };
            dynamic newOrder = await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/OE/Orders", order);

            Console.WriteLine("Created OE Order Number: {0} with Shipment Number: {1} Invoice Number: {2}", newOrder.OrderNumber, newOrder.LastShipmentNumber, newOrder.LastInvoiceNumber);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Starts IC Day End to generate AR invoices from the OE Order)
        /// </summary>
        public static async Task InvokeICDayEnd()
        {
            var dayendprocess = new
            {
                ClearHistory = false
            };
            await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/IC/DayEndProcessing($process)", dayendprocess);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Post the new AR invoices)
        /// </summary>
        public static async Task PostARInvoice()
        {
            var postinvoice = new
            {
                PostAllBatches = "Postallbatches"
            };
            await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/PostInvoices($process)", postinvoice);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Create the GL batch)
        /// </summary>
        public static async Task CreateGLBatch()
        {
            var createglbatch = new
            {
                ProcessInvoiceBatch = "PostInvoicebatches",
                InvoiceThroughPostingSequenceNumber = "999999999"
            };
            await SendRequest(new HttpMethod("POST"), @"http://localhost/Sage300WebApi/-/SAMLTD/AR/CreateGLBatch($process)", createglbatch);
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
            // Create the Web API client with the appropriate authentication
            using (var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("ADMIN", "ADMIN") })
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Console.WriteLine("{0} {1}", method.Method, requestUri);

                string payloadString = null;

                // Serialize the payload if one is present
                if (payload != null)
                {
                    payloadString = JsonConvert.SerializeObject(payload, Formatting.Indented);
                    Console.WriteLine("{0}", payloadString);
                }

                // Create the Web API request
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = string.IsNullOrWhiteSpace(payloadString) ? null : new StringContent(payloadString, Encoding.UTF8, "application/json")
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
