create or alter procedure ImportCategoryWithProduct_SP
    @CategoryName nvarchar(100),
    @CategoryIsDeleted bit,
    @Code varchar(10),
    @ProductName nvarchar(100),
    @Price money,
    @Quantity int,
    @ProductIsDeleted bit
as
begin
    set nocount on;

    declare @CategoryId int;
    declare @CategoryStatus int;

    select @CategoryId = c.ID, @CategoryStatus = c.IsDeleted
    from Categories ca
    where c.Name = @CategoryName;

    if @CategoryId is null
    begin
        insert into Categories(name, IsDeleted)
        values (@CategoryName, @CategoryIsDeleted);

        set @CategoryId = SCOPE_IDENTITY();

        if @CategoryIsDeleted = 1
        begin
            insert into Products(CategoryID, Code, Name, Price, Quantity, IsDeleted)
            values (@CategoryId, @Code, @ProductName, @Price, @Quantity, @CategoryIsDeleted)
            return 0;
        end;
        else
        begin
            insert into Products(CategoryID, Code, Name, Price, Quantity, IsDeleted)
            values (@CategoryId, @Code, @ProductName, @Price, @Quantity, @ProductIsDeleted)
            return 0;
        end;
    end;

    if @CategoryIsDeleted = 1
    begin
        insert into Products(CategoryID, Code, Name, Price, Quantity, IsDeleted)
        values (@CategoryId, @Code, @ProductName, @Price, @Quantity, @CategoryIsDeleted)

        update Categories
        set IsDeleted = 1
        where Id = @CategoryId;

        update Products
        set IsDeleted = 1
        where categoryId = @CategoryId
        return 0;
    end;
    else
    begin
        update Categories
        set IsDeleted = 0
        where Id = @CategoryId;

        insert into Products(CategoryID, Code, Name, Price, Quantity, IsDeleted)
        values (@CategoryId, @Code, @ProductName, @Price, @Quantity, @ProductIsDeleted)
    end;

    return 0;
end;