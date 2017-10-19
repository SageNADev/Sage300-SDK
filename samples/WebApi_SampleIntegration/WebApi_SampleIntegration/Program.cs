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
        private static int QuantityToOrder { get; set; }

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string Sage300WebAPIURI = "http://localhost/Sage300WebApi/v1.0/-/SAMLTD/";

            Console.WriteLine(@"This sample code demonstrates how a full featured integration with Sage 300 can be created with very little code using the Sage 300 Web API");
            Console.WriteLine();
            Console.WriteLine(@"Please confirm the Sage 300 Web API service root URL is http://localhost/Sage300WebApi/v1.0/-/SAMLTD/");
            Console.WriteLine();
            Console.Write(@"Enter (Y) to continue. (N) to edit the root URL: ");
            string answer = Console.ReadLine();
            if (answer.ToUpper() == "N")
            {
                Console.WriteLine();
                Console.WriteLine(@"Please enter the Sage 300 Web API service root URL (e.g. http://localhost/Sage300WebApi/v1.0/-/SAMLTD/):");
                Console.WriteLine();
                Sage300WebAPIURI = Console.ReadLine();
            }

            // Set up the input parameters (can be customized to be passed in externally)
            Console.Write("Enter a new customer number:");
            NewCustomerNumber = Console.ReadLine();
            ItemNumber = @"A1-103/0"; 
            QuantityToOrder = 2;

            // Start the workflow
            CreateCustomer(Sage300WebAPIURI).Wait();
            UpdateCustomer(Sage300WebAPIURI).Wait();
            CreateOEOrder(Sage300WebAPIURI).Wait();
            InvokeICDayEnd(Sage300WebAPIURI).Wait();
            PostARInvoice(Sage300WebAPIURI).Wait();
            CreateGLBatch(Sage300WebAPIURI).Wait();

            Console.WriteLine("\nPress any key to end.");
            Console.ReadKey();
        }

        /// <summary>
        /// Demonstrates how to create a simple record
        /// (Creates a customer)
        /// </summary>
        public static async Task CreateCustomer(string uri)
        {
            var customer = new
            {
                CustomerNumber = NewCustomerNumber,
                GroupCode = "WHL",
                TaxGroup = "BCTAX",
                ShortName = "Old Name",
            };
            await SendRequest(new HttpMethod("POST"), uri + @"AR/ARCustomers", customer);
        }

        /// <summary>
        /// Demonstrates how to update a record
        /// (Updates the customer created previously)
        /// </summary>
        public static async Task UpdateCustomer(string uri)
        {
            var customer = new
            {
                ShortName = "New Name"
            };
            await SendRequest(new HttpMethod("PATCH"), uri + @"AR/ARCustomers('" + NewCustomerNumber + "')", customer);
        }

        /// <summary>
        /// Demonstrates how to create a complex record with header and detail relationships and read the resulting response
        /// (Creates an OE Order)
        /// </summary>
        public static async Task CreateOEOrder(string uri)
        {
            var detail = new
            {
                Item = ItemNumber,
                QuantityOrdered = QuantityToOrder,
                QuantityShipped = QuantityToOrder
            };
            var order = new
            {
                CustomerNumber = NewCustomerNumber,
                InvoiceWillBeProduced = true,
                OrderDetails = new[] { detail }
            };
            dynamic newOrder = await SendRequest(new HttpMethod("POST"), uri + @"OE/OEOrders", order);

            Console.WriteLine("Created OE Order Number: {0} with Shipment Number: {1} Invoice Number: {2}", newOrder.OrderNumber, newOrder.LastShipmentNumber, newOrder.LastInvoiceNumber);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Starts IC Day End to generate AR invoices from the OE Order)
        /// </summary>
        public static async Task InvokeICDayEnd(string uri)
        {
            var dayendprocess = new
            {
                ClearHistory = false
            };
            await SendRequest(new HttpMethod("POST"), uri + @"IC/ICDayEndProcessing('$process')", dayendprocess);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Post the new AR invoices)
        /// </summary>
        public static async Task PostARInvoice(string uri)
        {
            var postinvoice = new
            {
                PostAllBatches = "Postallbatches"
            };
            await SendRequest(new HttpMethod("POST"), uri + @"AR/ARPostInvoices('$process')", postinvoice);
        }

        /// <summary>
        /// Demonstrates how to invoke a process endpoint
        /// (Create the GL batch)
        /// </summary>
        public static async Task CreateGLBatch(string uri)
        {
            var createglbatch = new
            {
                ProcessInvoiceBatch = "PostInvoicebatches",
                InvoiceThroughPostingSequenceNumber = 999999999
            };
            await SendRequest(new HttpMethod("POST"), uri + @"AR/ARCreateGLBatch('$process')", createglbatch);
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

            string responsePayload = "";
            // Serialize the payload if one is present
            if (payload != null)
            {
                var payloadString = JsonConvert.SerializeObject(payload);
                content = new StringContent(payloadString, Encoding.UTF8, "application/json");
            }

            // Create the Web API client with the appropriate authentication
            using (var httpClientHandler = new HttpClientHandler { Credentials = new NetworkCredential("WEBAPI", "WEBAPI") })
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                Console.WriteLine("\n{0} {1}", method.Method, requestUri);

                // Create the Web API request
                var request = new HttpRequestMessage(method, requestUri)
                {
                    Content = content
                };

                // Send the Web API request
                try
                {
                    var response = await httpClient.SendAsync(request);
                    responsePayload = await response.Content.ReadAsStringAsync();

                    var statusNumber = (int)response.StatusCode;
                    Console.WriteLine("\n{0} {1}", statusNumber, response.StatusCode);

                    if (statusNumber < 200 || statusNumber >= 300)
                    {
                        Console.WriteLine(responsePayload);
                        throw new ApplicationException(statusNumber.ToString());
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine("\n{0} Exception caught.", e);
                    Console.WriteLine("\n\nPlease ensure the service root URI entered is valid.");
                    Console.WriteLine("\n\nPress any key to end.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            return string.IsNullOrWhiteSpace(responsePayload) ? null : JsonConvert.DeserializeObject(responsePayload);
        }
    }
}

