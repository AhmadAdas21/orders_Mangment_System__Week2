select MONTH(created_date)as month
, sum(total) as reveneue
from [order]
group by MONTH(created_date);
