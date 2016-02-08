CREATE PROC [dbo].[GetMemberLogin]
(
@Email VARCHAR(255)=NULL
)
AS
 BEGIN 
       SELECT * FROM  Member (NOLOCK) WHERE  (Email=@Email OR NickName=@Email) 
 END
 ;
 GO
 ;
 USE [Membership]
GO
/****** Object:  StoredProcedure [dbo].[UpdateMemberDetail]    Script Date: 8.2.2016 22:47:30 ******/
 
CREATE PROC [dbo].[UpdateMemberDetail] 
(
@MemberId INT,@PasswordHash VARCHAR(100) NULL,@PasswordSalt VARCHAR(50) NULL, @NickName VARCHAR(50) 
)
AS
BEGIN
/***********Şifreler değiştirildi ise güncelleniyor*******************************/
IF ISNULL(@PasswordHash,'')<>'' AND ISNULL(@PasswordSalt,'')<>''
BEGIN
  UPDATE Member SET PasswordHash=@PasswordHash,PasswordSalt=@PasswordSalt  WHERE Id=@MemberId
END
/*********************************************************************************/
UPDATE Member        SET NickName=@NickName   WHERE Id=@MemberId
EXEC GetMemberDetail @MemberId=@MemberId
  
END
;
GO
;
USE [Membership]
GO
/****** Object:  StoredProcedure [dbo].[GetMemberDetail]    Script Date: 8.2.2016 22:48:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Author OKK
--exec GetMemberDetail @MemberId=2093797
ALTER PROC [dbo].[GetMemberDetail]
(
@MemberId INT 
)
AS
BEGIN
SELECT TOP 1 * FROM  Member WHERE m.Id=@MemberId 
END


