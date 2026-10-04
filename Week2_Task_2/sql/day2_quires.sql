select *
from product
where active==true;
select *
from products
where stock<5;
select *
from customer
where order o!=null;
select c.id,c.name,sum(o.total) as totall
FROM Customers c
INNER JOIN [order] o
    ON c.id = o.customer_id
GROUP BY c.name;
select top 5 p.id,p.name,
    sum(oi.quantity) as totall
FROM prod p
inner join oi
    on p.id = oi.product_id
group by p.id,p.name
order by  total_sold DESC;