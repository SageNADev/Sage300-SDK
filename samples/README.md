# Samples

## Environment setup for compilation

For successful compilation, the environment variable **Sage300WebDir** must be set to the 
install location of the Sage 300 Online Web folder.

By default, the SDK **Sage300WebDir** variable is set to the Sage 300 Online Web install path 
of the workstation that ran the Sage 300 code generation wizard. This variable must be 
overridden before building locally on other workstations since the project files need to provide
a hint mechanism to the Visuial Studio files in order to properly located the referenced files.

This can be accomplished in the following ways:

* Set the system variable **Sage300WebDir** on the machine to the Sage 300 Online Web folder

  * Using the Powershell Command Window:
  > ps> [Environment]::SetEnvironmentVariable("Sage300WebDir", "{Online\Web path}", "Machine")

  * Using the Windows System Environment Variables window:
    * Control Panel -> System -> Advanced system settings: System Properties window
    * System Properties --> Advanced tab --> Environment Variables...: Environment Variables window
    * Environment Variables --> System Variables --> New...: New System Variables
    * New System Variables:
      * Variable name: Sage300WebDir
      * Variable value: {Online\Web path}

## Database and Credentials

By default, **SAMLTD** is the database and **ADMIN/ADMIN** are the credentials utilized by the samples. 

These references are located in the Web project's **web.config** and **Global.asax** files. If a different 
database or other credentials are required, these files will require modification.

## Web API WCF Data Services

This sample is no longer applicable as the newer version of the OData specification does not allow
the behavior being demonstated. However, the sample is being left in the SDK merely for reference
and potential future enhancement.

## Resx Generation

Sample source files are provided as examples of a Resource Information Text File and Settings Text
File, which can be supplied to the Resx Generation Utility. Refer to the Resx Generation Utility document
located in the **docs\utilities** folder for details.

## Web API Samples

The Admin user does not have Sage 300 Web API privileges. In order to make various Sage 300 Web API 
requests, you will need to configure a new user and give it the proper user authorizations. Documentation
for the samples will assume you use **SAMLTD** as the database and **WEBAPI/WEBAPI** as the credentials.

Configuring a new user can be accomplished as follows:

* In Sage 300, navigate to Administrative Services -> Users
* Enter **WEBAPI** as the User ID, **WEBAPI** as the User Name, **WEBAPI** as the Password and **WEBAPI** in Verify
* Click Save when finished
* Go to Administrative Services -> Security Groups
* Ensure that for each application that has Sage 300 Web API rights that there is a group ID associated with it
* Go to Administrative Services -> User Authorizations
* For the WEBAPI user, assign the group ID that has Sage 300 Web API rights to each corresponding application

## Web API Sample POST Payloads

Sample **POST** payload files are provided in the **samples\WebApi_SamplePostPayloads** folder and these are  
further categorized by module. These sample payloads will illustrate a typical POST payload and have been 
populated with values from the **SAMLTD** database.

### How to use

1.	Run Swagger (http://localhost/sage300webapi)
2.  Click the **Open Swagger UI** button
2.	In the **samples\WebApi_SamplePostPayloads** folder, look for the text file for the endpoint that you want 
    to use and open it in your editor of choice (Example: OE\OEOrders_OrdersOnly.txt)
3.	Copy the contents from the text file
4.	Back in Swagger, expand the endpoint that you want to use (Example: OEOrders)
5.	Click **POST** to expand it the endpoint
6.	Paste the contents of the sample payload to the **Value** field
7.	Click **Try it out!**  
8.	Enter Username and Password if prompted (remember to upper case your credentials)
9.	The Record will be created and the responses displayed in the Response Body, 
    Response Code and Response Headers fields

### Notes

>The contents of the sample payload files are based upon the **SAMLTD** database

*	Running a POST endpoint with same content for setup screens like GL Accounts will result in an error 
  since the record already exists. Either delete the record first or modify the contents of the payload
  
*	For some endpoints, there are different examples:
  *	OEOrders endpoint:
    *	OEOrders_OrdersOnly.txt
    *	OEOrders_WithOptFields.txt
    *	OEOrders_WithShipmentsandInvoice.txt
  *	ARReceiptAndAdjustmentBatches endpoint:
    *	ARReceiptAndAdjustmentBatches_Adjustment.txt (AR Adjustment Entry)
    *	ARReceiptAndAdjustmentBatches_Prepayment.txt (AR Receipts – Prepayment type)
