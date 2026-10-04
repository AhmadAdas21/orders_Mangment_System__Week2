select sum(o.total) AS total_amount,c.name
from Customers c
inner join [order] o
on c.id=o.customer_id
group by c.name