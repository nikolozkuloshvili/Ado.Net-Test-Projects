create view GetCategories_VW
as
select
	c.Name as CategoryName,
	c.IsDeleted as CategoryStatus
from Categories c
