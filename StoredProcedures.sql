CREATE PROCEDURE GetallLogs
    @URLid UNIQUEIDENTIFIER
AS
SELECT URLId, ServiceName, Time, Wasup, UsedPing, UsedHttp, Latency
FROM LogHistorys
WHERE URLid=@URLid
GO


CREATE PROCEDURE GetServices
AS
SELECT *
FROM URLs
GO


CREATE PROCEDURE GetLogs
AS
SELECT *
FROM LogHistorys
GO


CREATE TRIGGER [dbo].[tr_SERVICENAME_Modified]
   ON [dbo].[URLs]
   AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE LogHistorys SET ServiceName = I.ServiceName

    FROM LogHistorys L
        INNER JOIN Inserted I on L.URLId = I.Id 
	WHERE L.ServiceName <> I.ServiceName

END