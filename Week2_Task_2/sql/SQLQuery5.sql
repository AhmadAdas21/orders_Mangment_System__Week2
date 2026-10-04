select top 5
p.name,p.id,p.price,sum(x.quantity)as total
from prod p
inner join oi x
on p.id =x.product_id
group by p.name,p.id,p.price
order by total desc;