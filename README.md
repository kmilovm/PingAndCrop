# PingAndCrop

This test was intended to fullfill all the requirements from R0ckw3ll Aut0m4tion.

Technologies Used:

# FrontEnd: 
  Angular 17.2.0
  
  Added Libraries: 
    -RxJs
    -Material
    -Guid
    
# BackEnd:
  .Net 8 

  Added Packages
    Azure Tables
    Azure Queues
    EF Core
    EF Sql

Main Features:

Compose basically for a services which grabs records from one data source, process it and put it into another datasource, this datasource can be defined very quicky by DI, and can be a Queue, and Azure Table or SQl Table, this is possible due the implementation of various services dedicated to each purpose

Pending Features:

And abstract provider to ease the switching of this data provider

# Prerequisites 

Azure Related Providers:

An azure storage account with the right permissions to create and read data from Queues or Tables, and also the Azure Storage Connection string which goes at App.Settings file at PingAndCrop.RestAPI

Sql Server Provider:

A valid user with enough permissions to modify master database, you can grab the following script to create it as well:

USE [master]
GO

CREATE LOGIN [PingAndCropSA] WITH PASSWORD=N'PutHereYourPassword', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

ALTER LOGIN [PingAndCropSA] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [PingAndCropSA]
GO

# Remember to modify App.Settings at project PingAndCrop.RestAPI 





