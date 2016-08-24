-- The MIT License (MIT) 
-- Copyright (c) 1994-2016 The Sage Group plc or its licensors.  All rights reserved.
-- 
-- Permission is hereby granted, free of charge, to any person obtaining a copy of 
-- this software and associated documentation files (the "Software"), to deal in 
-- the Software without restriction, including without limitation the rights to use, 
-- copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
-- Software, and to permit persons to whom the Software is furnished to do so, 
-- subject to the following conditions:
-- 
-- The above copyright notice and this permission notice shall be included in all 
-- copies or substantial portions of the Software.
-- 
-- THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
-- INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
-- PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
-- HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
-- CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
-- OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

-- Insert_WorkerRole_Data.sql
-- Insert data

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

BEGIN
INSERT INTO [dbo].[WorkflowKind](WorkflowKindId, UniqueName, MaxRetries)VALUES (9128, 'TUClearStatistics', 3)
END
GO

SET IDENTITY_INSERT UnitOfWorkKind ON

BEGIN
	INSERT INTO [dbo].[UnitOfWorkKind](UnitOfWorkKindId, WorkflowKindId, UniqueName, AssemblyName, TypeName, ExecutionOrder, IsAsynchronous) VALUES(9128, 9128, 'TUClearStatistics', 'ValuedPartner.TU.Services', 'ValuedPartner.TU.Services.UnitOfWork.ClearStatisticsUow', 1, 1)
END
GO

SET IDENTITY_INSERT UnitOfWorkKind OFF
GO