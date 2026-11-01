create view GetProductsByCategoryName_VW
as
select 
	c.Name as CategoryName,
	c.IsDeleted as CategoryStatus,
	p.code, 
	p.Name as ProductName,
	p.Price,
	p.Quantity,
	p.IsDeleted as ProductStatus
from Categories c 
inner join Products p on c.Id = p.CategoryID