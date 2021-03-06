﻿/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM ApplicationConfigurations)
BEGIN
    INSERT INTO [dbo].[ApplicationConfigurations]
               ([Created]
               ,[Updated]
               ,[RegistrationSupported]
               ,[UsersLimit])
         VALUES
               (CURRENT_TIMESTAMP,
               CURRENT_TIMESTAMP,
               4,
               1)
END
