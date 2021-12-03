/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT TOP (1000) [ID]
      ,[FEN]
      ,[Status]
  FROM [CHESS].[dbo].[Games]