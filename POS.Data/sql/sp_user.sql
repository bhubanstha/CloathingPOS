select top 1 * from [User]

go

create proc GetAllUser
@UserName varchar(50),
@BranchId bigint
as
begin
	select 
	 u.Id
	,u.UserName
	,u.DisplayName
	,u.Password
	,u.IsAdmin
	,u.IsActive
	,u.PromptForPasswordReset
	,u.ProfileImage
	,u.CreatedDate
	,u.DeactivationDate
	,u.LastPasswordChangeDate
	,u.BranchId
	,u.CanAccessAllBranch
	from [User] u with (nolock)
	where u.UserName <> 'sysadmin' and u.UserName <> @UserName and u.BranchId = @BranchId
end

go

create proc LoginUser
@UserName varchar(50),
@Password varchar(200),
@BranchId bigint
as
begin
	select 
	u.Id
	,u.UserName
	,u.DisplayName
	,u.Password
	,u.IsAdmin
	,u.IsActive
	,u.PromptForPasswordReset
	,u.ProfileImage
	,u.CreatedDate
	,u.DeactivationDate
	,u.LastPasswordChangeDate
	,u.BranchId
	,u.CanAccessAllBranch
	from [User] u
	where u.UserName = @UserName
	and u.[Password] = @Password
	and (u.BranchId = @BranchId or u.CanAccessAllBranch = 1)
end

go

create proc SaveUser
@UserName	varchar(15),
@DisplayName	varchar(40),
@Password	varchar(60),
@IsAdmin	bit,
@IsActive	bit,
@PromptForPasswordReset	bit,
@ProfileImage	varchar(30),
@CreatedDate	datetime,
@DeactivationDate	datetime,
@LastPasswordChangeDate	datetime,
@BranchId	bigint,
@CanAccessAllBranch	bit
as
begin
	insert into [User] (UserName, DisplayName, Password, IsAdmin, IsActive, PromptForPasswordReset, ProfileImage, CreatedDate, DeactivationDate, LastPasswordChangeDate, BranchId, CanAccessAllBranch) values
	(@UserName, @DisplayName, @Password, @IsAdmin, @IsActive, @PromptForPasswordReset, @ProfileImage, @CreatedDate, @DeactivationDate, @LastPasswordChangeDate, @BranchId, @CanAccessAllBranch) 
end

go

create proc UpdateUser
@Id bigint,
@UserName	varchar(15),
@DisplayName	varchar(40),
@Password	varchar(60),
@IsAdmin	bit,
@IsActive	bit,
@PromptForPasswordReset	bit,
@ProfileImage	varchar(30),
@CreatedDate	datetime,
@DeactivationDate	datetime,
@LastPasswordChangeDate	datetime,
@BranchId	bigint,
@CanAccessAllBranch	bit
as
begin
	update [User] set
	UserName = @UserName,
	DisplayName = @DisplayName,
	Password = @Password,
	IsAdmin = @IsAdmin,
	IsActive = @IsActive,
	PromptForPasswordReset = @PromptForPasswordReset,
	ProfileImage = @ProfileImage,
	CreatedDate = @CreatedDate,
	DeactivationDate = @DeactivationDate,
	LastPasswordChangeDate = @LastPasswordChangeDate,
	BranchId = @BranchId,
	CanAccessAllBranch = @CanAccessAllBranch
	where Id = @Id
end

go


create proc GetUserByUserName
@UserName varchar(50)
as
begin
	select 
	u.Id
	,u.UserName
	,u.DisplayName
	,u.Password
	,u.IsAdmin
	,u.IsActive
	,u.PromptForPasswordReset
	,u.ProfileImage
	,u.CreatedDate
	,u.DeactivationDate
	,u.LastPasswordChangeDate
	,u.BranchId
	,u.CanAccessAllBranch
	from [User] u
	where u.UserName = @UserName
end
